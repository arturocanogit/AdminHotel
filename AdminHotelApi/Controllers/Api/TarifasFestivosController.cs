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
    public class TarifasFestivosController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/TarifasFestivos
        [ResponseType(typeof(ResultadoDto<IEnumerable<TarifaFestivoDto>>))]
        public IHttpActionResult GetTarifasFestivos()
        {
            List<TarifaFestivo> tarifas = db.TarifasFestivos
                .Where(x => x.HotelId == Constantes.HotelId && x.Activo).ToList();

            List<TarifaFestivoDto> tarifasDto = tarifas
                .Select(x => Utilerias.Mapeador<TarifaFestivoDto, TarifaFestivo>(x))
                .ToList();

            HotelDto hotel = Utilerias.Mapeador<HotelDto, Hotel>(db.Hoteles.First(x => x.HotelId == Constantes.HotelId));
            tarifasDto.ForEach(x => { x.Hotel = hotel; });
            tarifasDto.ForEach(x => 
            { 
                x.TipoHabitacion = Utilerias.Mapeador<TipoHabitacionDto, TipoHabitacion>(
                    tarifas.First(y => y.TipoHabitacionId == x.TipoHabitacionId).TipoHabitacion); 
            });

            return Ok(new ResultadoDto<IEnumerable<TarifaFestivoDto>>
            {
                Datos = tarifasDto
            });
        }

        // GET: api/TarifasFestivos/5
        [ResponseType(typeof(TarifaFestivoDto))]
        public IHttpActionResult GetTarifa(int id)
        {
            TarifaFestivo tarifa = db.TarifasFestivos
                .FirstOrDefault(x => x.HotelId == Constantes.HotelId && x.Activo && x.TarifaId == id);

            if (tarifa == null)
            {
                return NotFound();
            }

            TarifaFestivoDto tarifaDto = Utilerias.Mapeador<TarifaFestivoDto, TarifaFestivo>(tarifa);

            tarifaDto.Hotel = Utilerias.Mapeador<HotelDto, Hotel>(db.Hoteles.First(x => x.HotelId == Constantes.HotelId));
            tarifaDto.TipoHabitacion = Utilerias.Mapeador<TipoHabitacionDto, TipoHabitacion>(tarifa.TipoHabitacion);

            return Ok(new ResultadoDto<TarifaFestivoDto>
            {
                Datos = tarifaDto
            });
        }

        // PUT: api/TarifasFestivos/5
        [ResponseType(typeof(ResultadoDto))]
        public IHttpActionResult PutTarifa(int id, TarifaFestivoDto tarifa)
        {
            if (id != tarifa.TarifaId)
            {
                return BadRequest();
            }

            TarifaFestivo currentTarifa = db.TarifasFestivos.Find(Constantes.HotelId, id);
            if (currentTarifa.IsNull())
            {
                return NotFound();
            }

            currentTarifa.Precio = tarifa.Precio;
            currentTarifa.Personas = tarifa.Personas;
            currentTarifa.TipoHabitacionId = tarifa.TipoHabitacionId;
            currentTarifa.Fecha = tarifa.Fecha;
            currentTarifa.FechaUpdate = DateTime.Now;

            db.Entry(currentTarifa).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new ResultadoDto
            {
                Mensaje = "La tarifa se actualizó correctamente."
            });
        }

        // POST: api/TarifasFestivos
        [ResponseType(typeof(ResultadoDto<TarifaFestivoDto>))]
        public IHttpActionResult PostTarifa(TarifaFestivoDto tarifa)
        {
            int tarifaId = (db.TarifasFestivos
                    .Where(x => x.HotelId == Constantes.HotelId)
                    .Max(x => (int?)x.TarifaId) ?? 0) + 1;

            tarifa.TarifaId = tarifaId;
            tarifa.HotelId = Constantes.HotelId;

            var nuevaTarifa = db.TarifasFestivos.Add(Utilerias.Mapeador<TarifaFestivo, TarifaFestivoDto>(tarifa));
            db.SaveChanges();

            return Created(string.Empty, new ResultadoDto<TarifaFestivoDto>
            {
                Mensaje = "La tarifa se guardó correctamente.",
                Datos = Utilerias.Mapeador<TarifaFestivoDto, TarifaFestivo>(nuevaTarifa)
            });
        }

        // DELETE: api/TarifasFestivos/5
        [ResponseType(typeof(ResultadoDto))]
        public IHttpActionResult DeleteTarifa(int id)
        {
            TarifaFestivo tarifa = db.TarifasFestivos.Find(Constantes.HotelId, id);
            if (tarifa == null)
            {
                return NotFound();
            }

            tarifa.FechaUpdate = DateTime.Now;
            tarifa.Activo = false;

            db.Entry(tarifa).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new ResultadoDto
            {
                Mensaje = "La tarifa se elimino correctamente."
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

        private bool TarifaExists(int id)
        {
            return db.TarifasFestivos.Count(e => e.HotelId == id) > 0;
        }
    }
}