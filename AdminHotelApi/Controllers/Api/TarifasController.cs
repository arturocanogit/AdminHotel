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
    public class TarifasController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/Tarifas
        [ResponseType(typeof(ResultadoDto<IEnumerable<TarifaDto>>))]
        public IHttpActionResult GetTarifas()
        {
            var hoteles = db.Tarifas.Where(x => x.HotelId == Constantes.HotelId && x.Activo).ToList();
            return Ok(new ResultadoDto<IEnumerable<TarifaDto>>
            {
                Datos = hoteles.Select(x => Utilerias
                .Mapeador<TarifaDto, Tarifa>(x))
            });
        }

        // GET: api/Tarifas/5
        [ResponseType(typeof(TarifaDto))]
        public IHttpActionResult GetTarifa(int id)
        {
            Tarifa tarifa = db.Tarifas.Find(Constantes.HotelId, id);
            if (tarifa == null)
            {
                return NotFound();
            }
            return Ok(new ResultadoDto<TarifaDto>
            {
                Datos = Utilerias.Mapeador<TarifaDto, Tarifa>(tarifa)
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
                Mensaje = "El cliente se actualizó correctamente."
            });
        }

        // POST: api/Tarifas
        [ResponseType(typeof(ResultadoDto<TarifaDto>))]
        public IHttpActionResult PostTarifa(TarifaDto cliente)
        {
            int tarifaId = (db.Tarifas
                    .Where(x => x.HotelId == Constantes.HotelId)
                    .Max(x => (int?)x.TarifaId) ?? 0) + 1;

            cliente.TarifaId = tarifaId;
            cliente.HotelId = Constantes.HotelId;

            var nuevoTarifa = db.Tarifas.Add(Utilerias.Mapeador<Tarifa, TarifaDto>(cliente));
            db.SaveChanges();

            return Created(string.Empty, new ResultadoDto<TarifaDto>
            {
                Mensaje = "El cliente se guardó correctamente.",
                Datos = Utilerias.Mapeador<TarifaDto, Tarifa>(nuevoTarifa)
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
                Mensaje = "El cliente se elimino correctamente."
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