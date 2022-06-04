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
using AdminHotelApi.Models.Entities;

namespace AdminHotelApi.Controllers
{
    public class ReservacionesDetallesController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/ReservacionesDetalles
        public IQueryable<ReservacionDetalle> GetReservacionDetalles()
        {
            return db.ReservacionDetalles;
        }

        // GET: api/ReservacionesDetalles/5
        [ResponseType(typeof(ReservacionDetalle))]
        public IHttpActionResult GetReservacionDetalle(int id)
        {
            ReservacionDetalle reservacionDetalle = db.ReservacionDetalles.Find(id);
            if (reservacionDetalle == null)
            {
                return NotFound();
            }

            return Ok(reservacionDetalle);
        }

        // PUT: api/ReservacionesDetalles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReservacionDetalle(int id, ReservacionDetalle reservacionDetalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reservacionDetalle.HotelId)
            {
                return BadRequest();
            }

            db.Entry(reservacionDetalle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservacionDetalleExists(id))
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

        // POST: api/ReservacionesDetalles
        [ResponseType(typeof(ReservacionDetalle))]
        public IHttpActionResult PostReservacionDetalle(ReservacionDetalle reservacionDetalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ReservacionDetalles.Add(reservacionDetalle);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ReservacionDetalleExists(reservacionDetalle.HotelId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = reservacionDetalle.HotelId }, reservacionDetalle);
        }

        // DELETE: api/ReservacionesDetalles/5
        [ResponseType(typeof(ReservacionDetalle))]
        public IHttpActionResult DeleteReservacionDetalle(int id)
        {
            ReservacionDetalle reservacionDetalle = db.ReservacionDetalles.Find(id);
            if (reservacionDetalle == null)
            {
                return NotFound();
            }

            db.ReservacionDetalles.Remove(reservacionDetalle);
            db.SaveChanges();

            return Ok(reservacionDetalle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservacionDetalleExists(int id)
        {
            return db.ReservacionDetalles.Count(e => e.HotelId == id) > 0;
        }
    }
}