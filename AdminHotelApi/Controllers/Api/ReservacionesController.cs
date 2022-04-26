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
        public IQueryable<Reservacion> GetReservacions()
        {
            return db.Reservacions;
        }

        // GET: api/Reservaciones/5
        [ResponseType(typeof(Reservacion))]
        public IHttpActionResult GetReservacion(int id)
        {
            Reservacion reservacion = db.Reservacions.Find(id);
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

            if (id != reservacion.ReservacionId)
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

            db.Reservacions.Add(reservacion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = reservacion.ReservacionId }, reservacion);
        }

        // DELETE: api/Reservaciones/5
        [ResponseType(typeof(Reservacion))]
        public IHttpActionResult DeleteReservacion(int id)
        {
            Reservacion reservacion = db.Reservacions.Find(id);
            if (reservacion == null)
            {
                return NotFound();
            }

            db.Reservacions.Remove(reservacion);
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
            return db.Reservacions.Count(e => e.ReservacionId == id) > 0;
        }
    }
}