using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdminHotelApi.Data;
using AdminHotelApi.Models;
using AdminHotelApi.Models.Dtos;
using AdminHotelApi.Models.Entities;
using Global;

namespace AdminHotelApi.Controllers
{
    public class TiposHabitacionesController : Controller
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: TiposHabitaciones
        public ActionResult Index()
        {
            var tiposHabitaciones = db.TiposHabitaciones.Include(t => t.Hotel);
            return View(tiposHabitaciones.ToList());
        }

        // GET: TiposHabitaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoHabitacion tipoHabitacion = db.TiposHabitaciones.Find(id);
            if (tipoHabitacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoHabitacion);
        }

        // GET: TiposHabitaciones/Create
        public ActionResult Create()
        {
            ViewBag.HotelId = new SelectList(db.Hoteles, "HotelId", "Nombre");
            return View();
        }

        // POST: TiposHabitaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HotelId,TipoHabitacionId,Nombre,Descripcion,Activo,FechaAlta,FechaUpdate,UploadFoto")] TipoHabitacionDto tipoHabitacionDto)
        {
            TipoHabitacion tipoHabitacion =
                    Utilerias.Mapeador<TipoHabitacion, TipoHabitacionDto>(tipoHabitacionDto);

            var tipoHabitacionFoto = Utilerias.Mapeador<TipoHabitacionFoto, ArchivoDto>(
                Project.PostedFileToDto(tipoHabitacionDto.UploadFoto));

            if (ModelState.IsValid)
            {
                int tipoHabitacionId = db.TiposHabitaciones
                    .Where(x => x.HotelId == 1)
                    .Max(x => (int?)x.TipoHabitacionId) ?? 0;

                tipoHabitacion.TipoHabitacionId = tipoHabitacionId + 1;

                int tipoHabitacionFotoId = db.TiposHabitacionesFotos
                    .Where(x => x.HotelId == tipoHabitacion.HotelId && x.TipoHabitacionId == tipoHabitacion.TipoHabitacionId)
                    .Max(x => (int?)x.TipoHabitacionId) ?? 0;

                tipoHabitacionFoto.TipoHabitacionFotoId = tipoHabitacionFotoId + 1;
                tipoHabitacionFoto.HotelId = tipoHabitacion.HotelId;
                tipoHabitacionFoto.TipoHabitacionId = tipoHabitacion.TipoHabitacionId;
                tipoHabitacionFoto.Activo = true;
                tipoHabitacionFoto.FechaAlta = tipoHabitacion.FechaAlta;

                db.TiposHabitaciones.Add(tipoHabitacion);
                db.TiposHabitacionesFotos.Add(tipoHabitacionFoto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelId = new SelectList(db.Hoteles, "HotelId", "Nombre", tipoHabitacion.HotelId);
            return View(tipoHabitacion);
        }

        // GET: TiposHabitaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoHabitacion tipoHabitacion = db.TiposHabitaciones.Find(id);
            if (tipoHabitacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelId = new SelectList(db.Hoteles, "HotelId", "Nombre", tipoHabitacion.HotelId);
            return View(tipoHabitacion);
        }

        // POST: TiposHabitaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HotelId,TipoHabitacionId,Nombre,Capacidad,Activo,FechaAlta,FechaUpdate")] TipoHabitacion tipoHabitacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoHabitacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelId = new SelectList(db.Hoteles, "HotelId", "Nombre", tipoHabitacion.HotelId);
            return View(tipoHabitacion);
        }

        // GET: TiposHabitaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoHabitacion tipoHabitacion = db.TiposHabitaciones.Find(id);
            if (tipoHabitacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoHabitacion);
        }

        // POST: TiposHabitaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoHabitacion tipoHabitacion = db.TiposHabitaciones.Find(id);
            db.TiposHabitaciones.Remove(tipoHabitacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
