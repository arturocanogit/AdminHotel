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
    public class TiposHabitacionesController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/TiposHabitaciones
        [ResponseType(typeof(ResultadoDto<IEnumerable<TipoHabitacionDto>>))]
        public IHttpActionResult GetTiposHabitaciones()
        {
            var hoteles = db.TiposHabitaciones.Where(x => x.HotelId == Constantes.HotelId && x.Activo).ToList();
            return Ok(new ResultadoDto<IEnumerable<TipoHabitacionDto>>
            {
                Datos = hoteles.Select(x => Utilerias
                .Mapeador<TipoHabitacionDto, TipoHabitacion>(x))
            });
        }

        // GET: api/TiposHabitaciones/5
        [ResponseType(typeof(TipoHabitacionDto))]
        public IHttpActionResult GetTipoHabitacion(int id)
        {
            TipoHabitacion tipoHabitacion = db.TiposHabitaciones.Find(Constantes.HotelId, id);
            if (tipoHabitacion == null)
            {
                return NotFound();
            }
            return Ok(new ResultadoDto<TipoHabitacionDto>
            {
                Datos = Utilerias.Mapeador<TipoHabitacionDto, TipoHabitacion>(tipoHabitacion)
            });
        }

        // PUT: api/TiposHabitaciones/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoHabitacion(int id, TipoHabitacionDto tipoHabitacion)
        {
            if (id != tipoHabitacion.TipoHabitacionId)
            {
                return BadRequest();
            }

            TipoHabitacion currentTipoHabitacion = db.TiposHabitaciones.Find(Constantes.HotelId, id);

            currentTipoHabitacion.Nombre = tipoHabitacion.Nombre;
            currentTipoHabitacion.Descripcion = tipoHabitacion.Descripcion;
            currentTipoHabitacion.FechaUpdate = DateTime.Now;

            db.Entry(currentTipoHabitacion).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new ResultadoDto
            {
                Mensaje = "El tipo de habitación se actualizó correctamente."
            });
        }

        // POST: api/TiposHabitaciones
        [ResponseType(typeof(TipoHabitacionDto))]
        public IHttpActionResult PostTipoHabitacion(TipoHabitacionDto tipoHabitacion)
        {
            int tipoHabitacionId = (db.TiposHabitaciones
                    .Where(x => x.HotelId == Constantes.HotelId)
                    .Max(x => (int?)x.TipoHabitacionId) ?? 0) + 1;

            tipoHabitacion.TipoHabitacionId = tipoHabitacionId;
            tipoHabitacion.HotelId = Constantes.HotelId;
            
            var nuevoTipoHabitacion = db.TiposHabitaciones.Add(Utilerias.Mapeador<TipoHabitacion, TipoHabitacionDto>(tipoHabitacion));
            db.SaveChanges();

            return Created(string.Empty, new ResultadoDto<TipoHabitacionDto>
            {
                Mensaje = "El tipo de habitación se guardó correctamente.",
                Datos = Utilerias.Mapeador<TipoHabitacionDto, TipoHabitacion>(nuevoTipoHabitacion)
            });
        }

        // DELETE: api/TiposHabitaciones/5
        [ResponseType(typeof(TipoHabitacion))]
        public IHttpActionResult DeleteTipoHabitacion(int id)
        {
            TipoHabitacion tipoHabitacion = db.TiposHabitaciones.Find(Constantes.HotelId, id);
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
                Mensaje = "El tipo de habitación se elimino correctamente."
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

        private bool TipoHabitacionExists(int id)
        {
            return db.TiposHabitaciones.Count(e => e.HotelId == id) > 0;
        }
    }
}