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

namespace AdminHotelApi.Controllers
{
    public class TipoHabitacionesController : Controller
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: TipoHabitaciones
        public ActionResult Index()
        {
            return View(db.TipoHabitacions.ToList());
        }

        // GET: TipoHabitaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoHabitacion tipoHabitacion = db.TipoHabitacions.Find(id);
            if (tipoHabitacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoHabitacion);
        }

        // GET: TipoHabitaciones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoHabitaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TipoHabitacionId,Nombre,Capacidad")] TipoHabitacion tipoHabitacion)
        {
            if (ModelState.IsValid)
            {
                db.TipoHabitacions.Add(tipoHabitacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoHabitacion);
        }

        // GET: TipoHabitaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoHabitacion tipoHabitacion = db.TipoHabitacions.Find(id);
            if (tipoHabitacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoHabitacion);
        }

        // POST: TipoHabitaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoHabitacionId,Nombre,Capacidad")] TipoHabitacion tipoHabitacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoHabitacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoHabitacion);
        }

        // GET: TipoHabitaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoHabitacion tipoHabitacion = db.TipoHabitacions.Find(id);
            if (tipoHabitacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoHabitacion);
        }

        // POST: TipoHabitaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoHabitacion tipoHabitacion = db.TipoHabitacions.Find(id);
            db.TipoHabitacions.Remove(tipoHabitacion);
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
