﻿using System;
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
    public class HabitacionesController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/Habitaciones
        public IQueryable<Habitacion> GetHabitaciones()
        {
            return db.Habitaciones;
        }

        // GET: api/Habitaciones/5
        [ResponseType(typeof(Habitacion))]
        public IHttpActionResult GetHabitacion(int id)
        {
            Habitacion habitacion = db.Habitaciones.Find(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            return Ok(habitacion);
        }

        // PUT: api/Habitaciones/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHabitacion(int id, Habitacion habitacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != habitacion.HotelId)
            {
                return BadRequest();
            }

            db.Entry(habitacion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HabitacionExists(id))
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

        // POST: api/Habitaciones
        [ResponseType(typeof(Habitacion))]
        public IHttpActionResult PostHabitacion(Habitacion habitacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Habitaciones.Add(habitacion);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (HabitacionExists(habitacion.HotelId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = habitacion.HotelId }, habitacion);
        }

        // DELETE: api/Habitaciones/5
        [ResponseType(typeof(Habitacion))]
        public IHttpActionResult DeleteHabitacion(int id)
        {
            Habitacion habitacion = db.Habitaciones.Find(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            db.Habitaciones.Remove(habitacion);
            db.SaveChanges();

            return Ok(habitacion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HabitacionExists(int id)
        {
            return db.Habitaciones.Count(e => e.HotelId == id) > 0;
        }
    }
}