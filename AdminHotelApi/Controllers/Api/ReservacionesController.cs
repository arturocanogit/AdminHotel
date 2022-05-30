using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Description;
using AdminHotelApi.Data;
using AdminHotelApi.Models;
using AdminHotelApi.Models.Dtos;
using Global;

namespace AdminHotelApi.Controllers.Api
{
    public class ReservacionesController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/Reservaciones
        public IHttpActionResult GetReservaciones()
        {
            return Ok(db.Reservaciones.ToList());
        }

        // GET: api/Reservaciones/5
        [HttpGet]
        [Route("api/GetReservacionPdf")]
        [ResponseType(typeof(ReservacionPdfDto))]
        public IHttpActionResult GetReservacionPdf(string id)
        {
            var resultado = new ReservacionPdfDto
            {
                FolioReservacion = id
            };
            IEnumerable<Reservacion> reservaciones = db.Reservaciones.Where(x => x.Folio == id);
            if (!reservaciones.Any())
            {
                return NotFound();
            }
            using (MemoryStream pdf = ReservacionesToPdf(reservaciones))
            {
                resultado.Archivo.Contenido = pdf.ToArray();
                resultado.Archivo.Nombre =
                    $"Reservacion{reservaciones.First().FechaAlta.Date}{reservaciones.First().HabitacionId}.pdf";
            }
            return Ok(resultado);
        }

        // GET: api/Reservaciones/5
        [ResponseType(typeof(ResultadoDto<ReservacionDto>))]
        public IHttpActionResult GetReservacion(string id)
        {
            List<Reservacion> reservaciones = db.Reservaciones
                .Where(x => x.Folio == id).ToList();

            if (!reservaciones.Any())
            {
                return NotFound();
            }
            ReservacionDto result = new ReservacionDto
            {
                FechaEntrada = reservaciones.First().FechaEntrada,
                FechaSalida = reservaciones.First().FechaSalida,
            };
            int clienteId = reservaciones.First().ClienteId;
            Cliente cliente = db.Clientes
                .Where(x => x.HotelId == Constantes.HotelId && x.ClienteId == clienteId)
                .First();

            result.Cliente = Utilerias.Mapeador<ClienteDto, Cliente>(cliente);
            result.Resevaciones = new List<ReservacionDetalleDto>();
            foreach (var item in reservaciones)
            {
                result.Resevaciones.Add(Utilerias
                    .Mapeador<ReservacionDetalleDto, Reservacion>(item));
            };
            return Ok(new ResultadoDto<ReservacionDto> 
            { 
                Datos = result 
            });
        }

        /// <summary>
        /// Genera el pdf de la reservacion a partir de los datos de la misma
        /// </summary>
        /// <param name="reservaciones"></param>
        /// <returns></returns>
        private MemoryStream ReservacionesToPdf(IEnumerable<Reservacion> reservaciones)
        {
            string template = File.ReadAllText($"{AppContext.BaseDirectory}/Files/TemplateReservacion");
            template.Replace("[FechaEntrada]", reservaciones.First().FechaEntrada.ToString());
            template.Replace("[FechaSalida]", reservaciones.First().FechaSalida.ToString());

            StringBuilder iterator = new StringBuilder();
            foreach (var item in reservaciones)
            {
                iterator.Append(item.TipoHabitacion.Nombre + item.Personas + Environment.NewLine);
            }
            template.Replace("[Reservaciones]", iterator.ToString());
            return Utilerias.HtmlToPdf(template);
        }

        // PUT: api/Reservaciones/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReservacion(int id, Reservacion reservacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservacion.HotelId)
            {
                return BadRequest();
            }

            db.Entry(reservacion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Reservaciones
        [ResponseType(typeof(Reservacion))]
        [Route("api/Reservacion")]
        public IHttpActionResult PostReservacion(Reservacion reservacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cliente cliente = db.Clientes
                .Where(x => x.Nombre == reservacion.Cliente.Nombre)
                .FirstOrDefault();

            if (cliente.IsNull())
            {
                int clienteId = db.Clientes.Where(x => x.HotelId == 1).Max(x => (int?)x.ClienteId) ?? 0;
                reservacion.Cliente.HotelId = Constantes.HotelId;
                reservacion.Cliente.ClienteId = clienteId + 1;
                reservacion.Cliente.Activo = true;
                reservacion.Cliente.FechaAlta = DateTime.Now;

                cliente = db.Clientes.Add(reservacion.Cliente);
            }
            else
            {
                reservacion.Cliente = cliente;
            }

            int reservacionId = db.Reservaciones.Where(x => x.HotelId == 1).Max(x => (int?)x.ReservacionId) ?? 0;
            reservacion.HotelId = Constantes.HotelId;
            reservacion.ReservacionId = reservacionId + 1;
            reservacion.Activo = true;
            reservacion.FechaAlta = DateTime.Now;
            reservacion.ClienteId = cliente.ClienteId;

            db.Reservaciones.Add(reservacion);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = reservacion.HotelId }, reservacion);
        }

        // POST: api/Reservaciones
        [ResponseType(typeof(ResultadoDto<dynamic>))]
        public IHttpActionResult PostReservaciones(ReservacionDto nuevaReservacion)
        {
            var folio = Guid.NewGuid().ToString().Split('-')[0];
            using (var scope = new TransactionScope())
            {
                foreach (var item in nuevaReservacion.Resevaciones)
                {
                    PostReservacion(new Reservacion
                    {
                        Folio = folio,
                        FechaEntrada = nuevaReservacion.FechaEntrada,
                        FechaSalida = nuevaReservacion.FechaSalida,
                        TipoHabitacionId = item.TipoHabitacionId,
                        Cliente = Utilerias.Mapeador<Cliente, ClienteDto>(nuevaReservacion.Cliente),
                        Personas = item.Personas,
                        Precio = item.Precio
                    });
                }
                scope.Complete();
            }
            return Created(string.Empty, new ResultadoDto<dynamic>
            {
                Mensaje = "Las reservaciones se guardarón correctamente.",
                Datos = new { Folio = folio }
            });
        }

        // DELETE: api/Reservaciones/5
        [ResponseType(typeof(ResultadoDto<dynamic>))]
        public IHttpActionResult DeleteReservacion(string id)
        {
            IEnumerable<Reservacion> reservaciones = db.Reservaciones.Where(x => x.Folio == id);
            if (!reservaciones.Any())
            {
                return NotFound();
            }

            foreach (var item in reservaciones)
            {
                item.Activo = false;
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();
            return Created(string.Empty, new ResultadoDto<dynamic>
            {
                Mensaje = "La reservación se elimino correctamente.",
                Datos = new { Folio = id }
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservacionExists(int id)
        {
            return db.Reservaciones.Count(e => e.HotelId == id) > 0;
        }

        // GET: api/Disponibilidades
        [HttpGet]
        [Route("api/Disponibilidades")]
        [ResponseType(typeof(IEnumerable<DisponibilidadDto>))]
        public IHttpActionResult GetDisponibilidades(DateTime fechaEntrada, DateTime fechaSalida)
        {
            ReservacionDao.Db = db;
            var disponibilidaes = ReservacionDao.GetDisponibilidades(fechaEntrada, fechaSalida);
            TarifaDao.Db = db;
            foreach (var item in disponibilidaes)
            {
                item.Tarifas = TarifaDao.GetTarifas(item.TipoHabitacionId, fechaEntrada, fechaSalida);
            }
            return Ok(disponibilidaes);
        }
    }
}