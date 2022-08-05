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
using AdminHotelApi.Models.Entities;
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
            List<TipoHabitacion> tiposHabitaciones = db.TiposHabitaciones
                .Where(x => x.HotelId == Constantes.HotelId && x.Activo).ToList();

            List<TipoHabitacionDto> tiposHabitacionesDto = new List<TipoHabitacionDto>();

            foreach (var tipoHabitacion in tiposHabitaciones.Select(x => Utilerias.Mapeador<TipoHabitacionDto, TipoHabitacion>(x)))
            {
                tipoHabitacion.Hotel = Utilerias
                    .Mapeador<HotelDto, Hotel>(db.Hoteles
                    .Where(x => x.HotelId == tipoHabitacion.HotelId).First());

                TipoHabitacionFoto foto = db.TiposHabitacionesFotos
                    .Where(x => x.HotelId == tipoHabitacion.HotelId && x.TipoHabitacionId == tipoHabitacion.TipoHabitacionId).FirstOrDefault();

                if (foto.IsNotNull())
                {
                    tipoHabitacion.Foto = new ArchivoDto
                    {
                        Nombre = foto.Nombre,
                        Contenido = foto.Contenido
                    };
                }
                tiposHabitacionesDto.Add(tipoHabitacion);
            }

            return Ok(new ResultadoDto<IEnumerable<TipoHabitacionDto>>
            {
                Datos = tiposHabitacionesDto
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
            TipoHabitacionDto tipoHabitacionDto = Utilerias.Mapeador<TipoHabitacionDto, TipoHabitacion>(tipoHabitacion);
            tipoHabitacionDto.Hotel = Utilerias
                    .Mapeador<HotelDto, Hotel>(db.Hoteles
                    .Where(x => x.HotelId == tipoHabitacionDto.HotelId).First());

            return Ok(new ResultadoDto<TipoHabitacionDto>
            {
                Datos = tipoHabitacionDto
            });
        }

        // PUT: api/TiposHabitaciones/5
        [ResponseType(typeof(ResultadoDto))]
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

            tipoHabitacion.HotelId = currentTipoHabitacion.HotelId;
            tipoHabitacion.TipoHabitacionId = currentTipoHabitacion.TipoHabitacionId;
            ActualizarTipoHabitacionFoto(tipoHabitacion);
            return Ok(new ResultadoDto
            {
                Mensaje = "El tipo de habitación se actualizó correctamente."
            });
        }

        // POST: api/TiposHabitaciones
        [ResponseType(typeof(ResultadoDto<TipoHabitacionDto>))]
        public IHttpActionResult PostTipoHabitacion(TipoHabitacionDto tipoHabitacion)
        {
            int tipoHabitacionId = (db.TiposHabitaciones
                    .Where(x => x.HotelId == Constantes.HotelId)
                    .Max(x => (int?)x.TipoHabitacionId) ?? 0) + 1;

            tipoHabitacion.TipoHabitacionId = tipoHabitacionId;
            tipoHabitacion.HotelId = Constantes.HotelId;

            var nuevoTipoHabitacion = db.TiposHabitaciones.Add(Utilerias.Mapeador<TipoHabitacion, TipoHabitacionDto>(tipoHabitacion));
            db.SaveChanges();

            GuardarTipoHabitacionFoto(tipoHabitacion);
            return Created(string.Empty, new ResultadoDto<TipoHabitacionDto>
            {
                Mensaje = "El tipo de habitación se guardó correctamente.",
                Datos = Utilerias.Mapeador<TipoHabitacionDto, TipoHabitacion>(nuevoTipoHabitacion)
            });
        }

        private void GuardarTipoHabitacionFoto(TipoHabitacionDto tipoHabitacion)
        {
            int tipoHabitacionFotoId = (db.TiposHabitacionesFotos
                              .Where(x => x.HotelId == tipoHabitacion.HotelId)
                              .Max(x => (int?)x.TipoHabitacionId) ?? 0) + 1;

            TipoHabitacionFoto tipoHabitacionFoto = Utilerias.Mapeador<TipoHabitacionFoto, ArchivoDto>(tipoHabitacion.Foto);

            tipoHabitacionFoto.TipoHabitacionFotoId = tipoHabitacionFotoId;
            tipoHabitacionFoto.HotelId = tipoHabitacion.HotelId;
            tipoHabitacionFoto.TipoHabitacionId = tipoHabitacion.TipoHabitacionId;

            db.TiposHabitacionesFotos.Add(tipoHabitacionFoto);
            db.SaveChanges();
        }

        private void ActualizarTipoHabitacionFoto(TipoHabitacionDto tipoHabitacion)
        {
            TipoHabitacionFoto foto = db.TiposHabitacionesFotos
                    .Where(x => x.HotelId == tipoHabitacion.HotelId && x.TipoHabitacionId == tipoHabitacion.TipoHabitacionId).FirstOrDefault();

            if (foto.IsNotNull())
            {
                foto.Nombre = tipoHabitacion.Foto.Nombre;
                foto.Contenido = tipoHabitacion.Foto.Contenido;
                foto.FechaUpdate = DateTime.Now;

                db.Entry(foto).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        // DELETE: api/TiposHabitaciones/5
        [ResponseType(typeof(ResultadoDto))]
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