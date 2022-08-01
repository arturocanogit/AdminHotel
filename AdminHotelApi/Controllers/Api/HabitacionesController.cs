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
using AdminHotelApi.Models.Dtos;
using Global;

namespace AdminHotelApi.Controllers.Api
{
    public class HabitacionesController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/Habitaciones
        [ResponseType(typeof(ResultadoDto<IEnumerable<HabitacionDto>>))]
        public IHttpActionResult GetHabitaciones()
        {
            List<Habitacion> habitaciones = db.Habitaciones
                .Where(x => x.HotelId == Constantes.HotelId && x.Activo).ToList();

            List<HabitacionDto> habitacionesDto = new List<HabitacionDto>();

            foreach (var habitacion in habitaciones.Select(x => Utilerias.Mapeador<HabitacionDto, Habitacion>(x)))
            {
                habitacion.Hotel = Utilerias
                    .Mapeador<HotelDto, Hotel>(db.Hoteles
                    .Where(x => x.HotelId == habitacion.HotelId).First());

                habitacion.TipoHabitacion = Utilerias
                   .Mapeador<TipoHabitacionDto, TipoHabitacion>(db.TiposHabitaciones
                   .Where(x => x.HotelId == habitacion.HotelId && x.TipoHabitacionId == habitacion.TipoHabitacionId).First());

                habitacionesDto.Add(habitacion);
            }

            return Ok(new ResultadoDto<IEnumerable<HabitacionDto>>
            {
                Datos = habitacionesDto
            });
        }

        // GET: api/Habitaciones/5
        [ResponseType(typeof(HabitacionDto))]
        public IHttpActionResult GetHabitacion(int id)
        {
            TipoHabitacion habitacion = db.TiposHabitaciones.Find(Constantes.HotelId, id);
            if (habitacion == null)
            {
                return NotFound();
            }
            HabitacionDto habitacionDto = Utilerias.Mapeador<HabitacionDto, TipoHabitacion>(habitacion);

            habitacionDto.Hotel = Utilerias
                    .Mapeador<HotelDto, Hotel>(db.Hoteles
                    .Where(x => x.HotelId == habitacionDto.HotelId).First());

            habitacionDto.TipoHabitacion = Utilerias
                   .Mapeador<TipoHabitacionDto, TipoHabitacion>(db.TiposHabitaciones
                   .Where(x => x.HotelId == habitacion.HotelId && x.TipoHabitacionId == habitacion.TipoHabitacionId).First());

            return Ok(new ResultadoDto<HabitacionDto>
            {
                Datos = habitacionDto
            });
        }

        // PUT: api/Habitaciones/5
        [ResponseType(typeof(ResultadoDto))]
        public IHttpActionResult PutHabitacion(int id, HabitacionDto tipoHabitacion)
        {
            if (id != tipoHabitacion.HabitacionId)
            {
                return BadRequest();
            }

            Habitacion currentHabitacion = db.Habitaciones.Find(Constantes.HotelId, id);

            currentHabitacion.TipoHabitacionId = tipoHabitacion.TipoHabitacionId;
            currentHabitacion.NumeroHabitacion = tipoHabitacion.NumeroHabitacion;
            currentHabitacion.Capacidad = tipoHabitacion.Capacidad;
            currentHabitacion.FechaUpdate = DateTime.Now;

            db.Entry(currentHabitacion).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new ResultadoDto
            {
                Mensaje = "La habitación se actualizó correctamente."
            });
        }

        // POST: api/Habitaciones
        [ResponseType(typeof(ResultadoDto<HabitacionDto>))]
        public IHttpActionResult PostHabitacion(HabitacionDto tipoHabitacion)
        {
            int tipoHabitacionId = (db.Habitaciones
                    .Where(x => x.HotelId == Constantes.HotelId)
                    .Max(x => (int?)x.HabitacionId) ?? 0) + 1;

            tipoHabitacion.HabitacionId = tipoHabitacionId;
            tipoHabitacion.HotelId = Constantes.HotelId;

            var nuevoHabitacion = db.Habitaciones.Add(Utilerias.Mapeador<Habitacion, HabitacionDto>(tipoHabitacion));
            db.SaveChanges();

            return Created(string.Empty, new ResultadoDto<HabitacionDto>
            {
                Mensaje = "La habitación se guardó correctamente.",
                Datos = Utilerias.Mapeador<HabitacionDto, Habitacion>(nuevoHabitacion)
            });
        }

        // DELETE: api/Habitaciones/5
        [ResponseType(typeof(ResultadoDto))]
        public IHttpActionResult DeleteHabitacion(int id)
        {
            Habitacion tipoHabitacion = db.Habitaciones.Find(Constantes.HotelId, id);
            if (tipoHabitacion == null)
            {
                return NotFound();
            }

            tipoHabitacion.FechaUpdate = DateTime.Now;
            tipoHabitacion.Activo = false;

            db.Entry(tipoHabitacion).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new ResultadoDto
            {
                Mensaje = "La habitación se elimino correctamente."
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

        private bool HabitacionExists(int id)
        {
            return db.Habitaciones.Count(e => e.HotelId == id) > 0;
        }
    }
}