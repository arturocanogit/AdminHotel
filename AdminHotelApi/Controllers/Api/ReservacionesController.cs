using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        [ResponseType(typeof(ReservacionDto))]
        public IHttpActionResult GetReservacion(int id)
        {
            Reservacion reservacion = db.Reservaciones.Find(id);
            if (reservacion == null)
            {
                return NotFound();
            }
            ReservacionDto resultado = Utilerias.Mapeador<ReservacionDto, Reservacion>(reservacion);
            string template = File.ReadAllText($"{AppContext.BaseDirectory}/Files/TemplateReservacion");

            resultado.Archivo.Nombre = 
                $"Reservacion{reservacion.FechaAlta.Date}{reservacion.HabitacionId}.pdf";

            using (MemoryStream pdf = Utilerias.HtmlToPdf(template))
            {
                resultado.Archivo.Contenido = pdf.ToArray();
            }
            return Ok(resultado);
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
        [ResponseType(typeof(ResultadoDto))]
        public IHttpActionResult PostReservaciones(IEnumerable<Reservacion> reservaciones)
        {
            using (var scope = new TransactionScope())
            {
                foreach (var item in reservaciones)
                {
                    PostReservacion(item);
                }
                scope.Complete();
            }
            return Created(string.Empty, new ResultadoDto 
            { 
                Mensaje = "Las reservaciones se guardarón crrectamente." 
            });
        }

        // DELETE: api/Reservaciones/5
        [ResponseType(typeof(Reservacion))]
        public IHttpActionResult DeleteReservacion(int id)
        {
            Reservacion reservacion = db.Reservaciones.Find(id);
            if (reservacion == null)
            {
                return NotFound();
            }

            db.Reservaciones.Remove(reservacion);
            db.SaveChanges();

            return Ok(reservacion);
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