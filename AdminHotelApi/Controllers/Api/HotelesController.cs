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

namespace AdminHotelApi.Controllers.Api
{
    public class HotelesController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/Hoteles
        [ResponseType(typeof(ResultadoDto<IEnumerable<Hotel>>))]
        public IHttpActionResult GetHoteles()
        {
            return Ok(new ResultadoDto<IEnumerable<Hotel>>
            {
                Datos = db.Hoteles.ToList()
            });
        }

        // GET: api/Hoteles/5
        [ResponseType(typeof(ResultadoDto<Hotel>))]
        public IHttpActionResult GetHotel(int id)
        {
            Hotel hotel = db.Hoteles.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(new ResultadoDto<Hotel>
            {
                Datos = hotel
            });
        }

        // PUT: api/Hoteles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHotel(int id, Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hotel.HotelId)
            {
                return BadRequest();
            }

            db.Entry(hotel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new ResultadoDto
            {
                Mensaje = "El hotel se actualizó correctamente."
            });
        }

        // POST: api/Hoteles
        [ResponseType(typeof(Hotel))]
        public IHttpActionResult PostHotel(Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            hotel = db.Hoteles.Add(hotel);
            db.SaveChanges();

            return Created(string.Empty, new ResultadoDto<Hotel>
            {
                Mensaje = "El hotel se guardó correctamente.",
                Datos = hotel
            });
        }

        // DELETE: api/Hoteles/5
        [ResponseType(typeof(Hotel))]
        public IHttpActionResult DeleteHotel(int id)
        {
            Hotel hotel = db.Hoteles.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }

            db.Hoteles.Remove(hotel);
            db.SaveChanges();

            return Ok(hotel);
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