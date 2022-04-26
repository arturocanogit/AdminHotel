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
    public class TipoHabitacionesController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/TipoHabitaciones
        public IQueryable<TipoHabitacion> GetTipoHabitacions()
        {
            return db.TipoHabitacions;
        }

        // GET: api/TipoHabitaciones/5
        [ResponseType(typeof(TipoHabitacion))]
        public IHttpActionResult GetTipoHabitacion(int id)
        {
            TipoHabitacion tipoHabitacion = db.TipoHabitacions.Find(id);
            if (tipoHabitacion == null)
            {
                return NotFound();
            }

            return Ok(tipoHabitacion);
        }

        // PUT: api/TipoHabitaciones/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoHabitacion(int id, TipoHabitacion tipoHabitacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoHabitacion.TipoHabitacionId)
            {
                return BadRequest();
            }

            db.Entry(tipoHabitacion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoHabitacionExists(id))
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

        // POST: api/TipoHabitaciones
        [ResponseType(typeof(TipoHabitacion))]
        public IHttpActionResult PostTipoHabitacion(TipoHabitacion tipoHabitacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoHabitacions.Add(tipoHabitacion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoHabitacion.TipoHabitacionId }, tipoHabitacion);
        }

        // DELETE: api/TipoHabitaciones/5
        [ResponseType(typeof(TipoHabitacion))]
        public IHttpActionResult DeleteTipoHabitacion(int id)
        {
            TipoHabitacion tipoHabitacion = db.TipoHabitacions.Find(id);
            if (tipoHabitacion == null)
            {
                return NotFound();
            }

            db.TipoHabitacions.Remove(tipoHabitacion);
            db.SaveChanges();

            return Ok(tipoHabitacion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoHabitacionExists(int id)
        {
            return db.TipoHabitacions.Count(e => e.TipoHabitacionId == id) > 0;
        }
    }
}