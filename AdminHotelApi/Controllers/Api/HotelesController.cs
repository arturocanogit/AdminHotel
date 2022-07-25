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
    public class HotelesController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/Hoteles
        [ResponseType(typeof(ResultadoDto<IEnumerable<HotelDto>>))]
        public IHttpActionResult GetHoteles()
        {
            var hoteles = db.Hoteles.Where(x => x.Activo).ToList();
            return Ok(new ResultadoDto<IEnumerable<HotelDto>>
            {
                Datos = hoteles.Select(x => Utilerias
                .Mapeador<HotelDto, Hotel>(x))
            });
        }

        // GET: api/Hoteles/5
        [ResponseType(typeof(ResultadoDto<HotelDto>))]
        public IHttpActionResult GetHotel(int id)
        {
            Hotel hotel = db.Hoteles.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(new ResultadoDto<HotelDto>
            {
                Datos = Utilerias.Mapeador<HotelDto, Hotel>(hotel)
            });
        }

        // PUT: api/Hoteles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHotel(int id, HotelDto hotel)
        {
            if (id != hotel.HotelId)
            {
                return BadRequest();
            }

            Hotel currentHotel = db.Hoteles.Find(id);

            currentHotel.Nombre = hotel.Nombre;
            currentHotel.FechaUpdate = DateTime.Now;

            db.Entry(currentHotel).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new ResultadoDto
            {
                Mensaje = "El hotel se actualizó correctamente."
            });
        }

        // POST: api/Hoteles
        [ResponseType(typeof(ResultadoDto<HotelDto>))]
        public IHttpActionResult PostHotel(HotelDto hotel)
        {
            var nuevoHotel = db.Hoteles.Add(Utilerias.Mapeador<Hotel, HotelDto>(hotel));
            db.SaveChanges();

            return Created(string.Empty, new ResultadoDto<HotelDto>
            {
                Mensaje = "El hotel se guardó correctamente.",
                Datos = Utilerias.Mapeador<HotelDto, Hotel>(nuevoHotel)
            });
        }

        // DELETE: api/Hoteles/5
        [ResponseType(typeof(ResultadoDto))]
        public IHttpActionResult DeleteHotel(int id)
        {
            Hotel hotel = db.Hoteles.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }

            hotel.FechaUpdate = DateTime.Now;
            hotel.Activo = false;

            db.Entry(hotel).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new ResultadoDto 
            {
                Mensaje = "El hotel se elimino correctamente."
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

        private bool HotelExists(int id)
        {
            return db.Hoteles.Count(e => e.HotelId == id) > 0;
        }
    }
}