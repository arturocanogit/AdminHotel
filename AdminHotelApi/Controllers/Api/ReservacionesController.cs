using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AdminHotelApi.Data;
using AdminHotelApi.Models;

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
        [ResponseType(typeof(Reservacion))]
        public IHttpActionResult GetReservacion(int id)
        {
            Reservacion reservacion = db.Reservaciones.Find(id);
            if (reservacion == null)
            {
                return NotFound();
            }

            return Ok(reservacion);
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
        public IHttpActionResult PostReservacion(Reservacion reservacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reservaciones.Add(reservacion);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ReservacionExists(reservacion.HotelId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = reservacion.HotelId }, reservacion);
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
        public IHttpActionResult GetDisponibilidades()
        {
            ReservacionDal.Db = db;
            var disponibilidaes = ReservacionDal.GetDisponibilidades();
            return Ok(disponibilidaes);
        }
    }
}