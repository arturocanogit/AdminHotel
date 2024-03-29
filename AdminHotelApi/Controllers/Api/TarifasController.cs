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
using AdminHotelApi.Models.Dtos;
using Global;

namespace AdminHotelApi.Controllers.Api
{
    public class TarifasController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/Tarifas
        [ResponseType(typeof(ResultadoDto<IEnumerable<TarifaDto>>))]
        public IHttpActionResult GetTarifas()
        {
            List<Tarifa> tarifas = db.Tarifas
                .Where(x => x.HotelId == Constantes.HotelId && x.Activo).ToList();

            List<TarifaDto> tarifasDto = tarifas
                .Select(x => Utilerias.Mapeador<TarifaDto, Tarifa>(x))
                .ToList();

            HotelDto hotel = Utilerias.Mapeador<HotelDto, Hotel>(db.Hoteles.First(x => x.HotelId == Constantes.HotelId));
            tarifasDto.ForEach(x => { x.Hotel = hotel; });
            tarifasDto.ForEach(x => 
            { 
                x.TipoHabitacion = Utilerias.Mapeador<TipoHabitacionDto, TipoHabitacion>(
                    tarifas.First(y => y.TipoHabitacionId == x.TipoHabitacionId).TipoHabitacion); 
            });

            return Ok(new ResultadoDto<IEnumerable<TarifaDto>>
            {
                Datos = tarifasDto
            });
        }

        // GET: api/Tarifas/5
        [ResponseType(typeof(TarifaDto))]
        public IHttpActionResult GetTarifa(int id)
        {
            Tarifa tarifa = db.Tarifas
                .FirstOrDefault(x => x.HotelId == Constantes.HotelId && x.Activo && x.TarifaId == id);

            if (tarifa == null)
            {
                return NotFound();
            }

            TarifaDto tarifaDto = Utilerias.Mapeador<TarifaDto, Tarifa>(tarifa);

            tarifaDto.Hotel = Utilerias.Mapeador<HotelDto, Hotel>(db.Hoteles.First(x => x.HotelId == Constantes.HotelId));
            tarifaDto.TipoHabitacion = Utilerias.Mapeador<TipoHabitacionDto, TipoHabitacion>(tarifa.TipoHabitacion);

            return Ok(new ResultadoDto<TarifaDto>
            {
                Datos = tarifaDto
            });
        }

        // PUT: api/Tarifas/5
        [ResponseType(typeof(ResultadoDto))]
        public IHttpActionResult PutTarifa(int id, TarifaDto cliente)
        {
            if (id != cliente.TarifaId)
            {
                return BadRequest();
            }

            Tarifa currentTarifa = db.Tarifas.Find(Constantes.HotelId, id);

            currentTarifa.Precio = cliente.Precio;
            currentTarifa.Personas = cliente.Personas;
            currentTarifa.FechaUpdate = DateTime.Now;

            db.Entry(currentTarifa).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new ResultadoDto
            {
                Mensaje = "La tarifa se actualizó correctamente."
            });
        }

        // POST: api/Tarifas
        [ResponseType(typeof(ResultadoDto<TarifaDto>))]
        public IHttpActionResult PostTarifa(TarifaDto tarifa)
        {
            int tarifaId = (db.Tarifas
                    .Where(x => x.HotelId == Constantes.HotelId)
                    .Max(x => (int?)x.TarifaId) ?? 0) + 1;

            tarifa.TarifaId = tarifaId;
            tarifa.HotelId = Constantes.HotelId;

            var nuevaTarifa = db.Tarifas.Add(Utilerias.Mapeador<Tarifa, TarifaDto>(tarifa));
            db.SaveChanges();

            return Created(string.Empty, new ResultadoDto<TarifaDto>
            {
                Mensaje = "La tarifa se guardó correctamente.",
                Datos = Utilerias.Mapeador<TarifaDto, Tarifa>(nuevaTarifa)
            });
        }

        // DELETE: api/Tarifas/5
        [ResponseType(typeof(ResultadoDto))]
        public IHttpActionResult DeleteTarifa(int id)
        {
            Tarifa tarifa = db.Tarifas.Find(Constantes.HotelId, id);
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
            return db.Tarifas.Count(e => e.HotelId == id) > 0;
        }
    }
}