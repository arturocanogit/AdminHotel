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
    public class ClientesController : ApiController
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: api/Clientes
        [ResponseType(typeof(ResultadoDto<IEnumerable<ClienteDto>>))]
        public IHttpActionResult GetClientes()
        {
            var hoteles = db.Clientes.Where(x => x.HotelId == Constantes.HotelId && x.Activo).ToList();
            return Ok(new ResultadoDto<IEnumerable<ClienteDto>>
            {
                Datos = hoteles.Select(x => Utilerias
                .Mapeador<ClienteDto, Cliente>(x))
            });
        }

        // GET: api/Clientes/5
        [ResponseType(typeof(ClienteDto))]
        public IHttpActionResult GetCliente(int id)
        {
            Cliente tipoCliente = db.Clientes.Find(Constantes.HotelId, id);
            if (tipoCliente == null)
            {
                return NotFound();
            }
            return Ok(new ResultadoDto<ClienteDto>
            {
                Datos = Utilerias.Mapeador<ClienteDto, Cliente>(tipoCliente)
            });
        }

        // PUT: api/Clientes/5
        [ResponseType(typeof(ResultadoDto))]
        public IHttpActionResult PutCliente(int id, ClienteDto cliente)
        {
            if (id != cliente.ClienteId)
            {
                return BadRequest();
            }

            Cliente currentCliente = db.Clientes.Find(Constantes.HotelId, id);

            currentCliente.Nombre = cliente.Nombre;
            currentCliente.Telefono = cliente.Telefono;
            currentCliente.Email = cliente.Email;
            currentCliente.FechaUpdate = DateTime.Now;

            db.Entry(currentCliente).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new ResultadoDto
            {
                Mensaje = "El cliente se actualizó correctamente."
            });
        }

        // POST: api/Clientes
        [ResponseType(typeof(ResultadoDto<ClienteDto>))]
        public IHttpActionResult PostCliente(ClienteDto cliente)
        {
            int tipoClienteId = (db.Clientes
                    .Where(x => x.HotelId == Constantes.HotelId)
                    .Max(x => (int?)x.ClienteId) ?? 0) + 1;

            cliente.ClienteId = tipoClienteId;
            cliente.HotelId = Constantes.HotelId;

            var nuevoCliente = db.Clientes.Add(Utilerias.Mapeador<Cliente, ClienteDto>(cliente));
            db.SaveChanges();

            return Created(string.Empty, new ResultadoDto<ClienteDto>
            {
                Mensaje = "El cliente se guardó correctamente.",
                Datos = Utilerias.Mapeador<ClienteDto, Cliente>(nuevoCliente)
            });
        }

        // DELETE: api/Clientes/5
        [ResponseType(typeof(ResultadoDto))]
        public IHttpActionResult DeleteCliente(int id)
        {
            Cliente tipoCliente = db.Clientes.Find(Constantes.HotelId, id);
            if (tipoCliente == null)
            {
                return NotFound();
            }

            tipoCliente.FechaUpdate = DateTime.Now;
            tipoCliente.Activo = false;

            db.Entry(tipoCliente).State = EntityState.Modified;
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

        private bool ClienteExists(int id)
        {
            return db.Clientes.Count(e => e.HotelId == id) > 0;
        }
    }
}