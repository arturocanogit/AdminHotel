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
using AdminHotelApi.Models.Entities;
using Global;

namespace AdminHotelApi.Controllers.Api
{
    public class ReservacionesController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/Reservaciones
        [ResponseType(typeof(ResultadoDto<IEnumerable<ReservacionDto>>))]
        public IHttpActionResult GetReservaciones()
        {
            List<Reservacion> reservaciones = db.Reservaciones
                .Where(x => x.HotelId == Constantes.HotelId && x.Activo).ToList();

            IEnumerable<ReservacionDto> resultado = reservaciones
                .Select(x => ReservacionToReservacionDto(x));

            return Ok(new ResultadoDto<IEnumerable<ReservacionDto>>
            {
                Datos = resultado
            });
        }

        // GET: api/Reservaciones/5
        [HttpGet]
        [Route("api/ReservacionPdf/{Id}")]
        [ResponseType(typeof(ResultadoDto<ReservacionPdfDto>))]
        public IHttpActionResult ReservacionPdf(string id)
        {
            var resultado = new ReservacionPdfDto
            {
                FolioReservacion = id,
                Archivo = new ArchivoDto()
            };
            Reservacion reservacion = db.Reservaciones.FirstOrDefault(x => x.Folio == id);
            if (reservacion.IsNull())
            {
                return NotFound();
            }
            ReservacionDto reservacionDto = ReservacionToReservacionDto(reservacion);
            using (MemoryStream pdf = ReservacionToPdf(reservacionDto))
            {
                resultado.Archivo.Contenido = pdf.ToArray();
                resultado.Archivo.Nombre =
                    $"Reservacion_{reservacion.Folio}.pdf";
            }
            return Ok(new ResultadoDto<ReservacionPdfDto>
            {
                Datos = resultado
            });
        }

        /// <summary>
        /// Genera el pdf de la reservacion a partir de los datos de la misma
        /// </summary>
        /// <param name="reservaciones"></param>
        /// <returns></returns>
        private MemoryStream ReservacionToPdf(ReservacionDto reservacion)
        {
            string template = File.ReadAllText($"{AppContext.BaseDirectory}/Files/TemplateReservacion.html");
            template = template.Replace("[FechaEntrada]", string.Format("{0:MM/dd/yyyy}", reservacion.FechaEntrada));
            template = template.Replace("[FechaSalida]", string.Format("{0:MM/dd/yyyy}", reservacion.FechaSalida));

            StringBuilder iterator = new StringBuilder();
            foreach (var item in reservacion.Resevaciones)
            {
                iterator.Append($"<tr><td>{item.TipoHabitacion.Nombre + item.Personas}</td></tr>");
            }
            template = template.Replace("[Reservaciones]", iterator.ToString());
            return Utilerias.HtmlToPdf(template);
        }

        // GET: api/Reservaciones/5
        [ResponseType(typeof(ResultadoDto<ReservacionDto>))]
        public IHttpActionResult GetReservacion(string id)
        {
            Reservacion reservacion = db.Reservaciones
               .FirstOrDefault(x => x.Folio == id && x.Activo);

            if (reservacion.IsNull())
            {
                return NotFound();
            }
            var resultado = ReservacionToReservacionDto(reservacion);
            return Ok(new ResultadoDto<ReservacionDto> 
            { 
                Datos = resultado
            });
        }

        private ReservacionDto ReservacionToReservacionDto(Reservacion reservacion)
        {
            int clienteId = reservacion.ClienteId;
            Cliente cliente = db.Clientes
                .Where(x => x.HotelId == Constantes.HotelId && x.ClienteId == clienteId)
                .First();

            ReservacionDto resultado = new ReservacionDto();
            resultado.Folio = reservacion.Folio;
            resultado.FechaEntrada = reservacion.FechaEntrada;
            resultado.FechaSalida = reservacion.FechaSalida;

            resultado.Cliente = Utilerias.Mapeador<ClienteDto, Cliente>(cliente);

            var reservacionDetalles = db.ReservacionDetalles
                .Where(x => x.HotelId == Constantes.HotelId && x.ReservacionId == reservacion.ReservacionId).ToList();

            resultado.Resevaciones = new List<ReservacionDetalleDto>();
            foreach (var reservacionDetalle in reservacionDetalles)
            {
                ReservacionDetalleDto detalle = Utilerias
                    .Mapeador<ReservacionDetalleDto, ReservacionDetalle>(reservacionDetalle);

                detalle.TipoHabitacion = Utilerias
                    .Mapeador<TipoHabitacionDto, TipoHabitacion>(db.TiposHabitaciones
                    .First(x => x.HotelId == Constantes.HotelId && x.TipoHabitacionId == reservacionDetalle.TipoHabitacionId));

                resultado.Resevaciones.Add(detalle);
            };

            return resultado;
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
        [ResponseType(typeof(ReservacionDto))]
        public IHttpActionResult PostReservacion(ReservacionDto reservacion)
        {
            var clienteId = 0;

            Cliente cliente = db.Clientes
                .Where(x => x.Nombre == reservacion.Cliente.Nombre)
                .FirstOrDefault();

            //Si no existe el cliente se crea
            if (cliente.IsNull())
            {
                clienteId = (db.Clientes
                    .Where(x => x.HotelId == 1).Max(x => (int?)x.ClienteId) ?? 0) + 1;

                cliente = Utilerias.Mapeador<Cliente, ClienteDto>(reservacion.Cliente);
                cliente.HotelId = Constantes.HotelId;
                cliente.ClienteId = clienteId;
                cliente.Activo = true;
                cliente.FechaAlta = DateTime.Now;

                db.Clientes.Add(cliente);
                db.SaveChanges();
            }
            clienteId = cliente.ClienteId;

            int reservacionId = (db.Reservaciones
                .Where(x => x.HotelId == 1).Max(x => (int?)x.ReservacionId) ?? 0) + 1;

            string folio = Guid.NewGuid().ToString().Split('-')[0];

            //Se da de alta la nueva reservacion
            db.Reservaciones.Add(new Reservacion 
            {
                HotelId = Constantes.HotelId,
                ReservacionId = reservacionId,
                Folio = folio,
                ClienteId = clienteId,
                FechaEntrada = reservacion.FechaEntrada,
                FechaSalida = reservacion.FechaSalida,
                Activo = true,
                FechaAlta = DateTime.Now
            });
            db.SaveChanges();

            foreach (var reservacionDetalle in reservacion.Resevaciones)
            {
                int reservacionDetalleId = (db.ReservacionDetalles
                .Where(x => x.HotelId == 1 && x.ReservacionId == reservacionId)
                .Max(x => (int?)x.ReservacionDetalleId) ?? 0) + 1;

                //Se da de alta del detalle de la reservacion
                db.ReservacionDetalles.Add(new ReservacionDetalle
                {
                    HotelId = Constantes.HotelId,
                    ReservacionId = reservacionId,
                    ReservacionDetalleId = reservacionDetalleId,
                    TipoHabitacionId = reservacionDetalle.TipoHabitacionId,
                    Personas = reservacionDetalle.Personas,
                    Precio = reservacionDetalle.Precio,
                    Activo = true,
                    FechaAlta = DateTime.Now
                });
                db.SaveChanges();
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
        [ResponseType(typeof(ResultadoDto<IEnumerable<DisponibilidadDto>>))]
        public IHttpActionResult GetDisponibilidades(DateTime fechaEntrada, DateTime fechaSalida)
        {
            ReservacionDao.Db = db;
            var disponibilidaes = ReservacionDao.GetDisponibilidades(fechaEntrada, fechaSalida);
            TarifaDao.Db = db;
            foreach (var item in disponibilidaes)
            {
                item.Tarifas = TarifaDao.GetTarifas(item.TipoHabitacionId, fechaEntrada, fechaSalida);
            }
            return Ok(new ResultadoDto<IEnumerable<DisponibilidadDto>>
            {
                Datos = disponibilidaes
            });
        }
    }
}