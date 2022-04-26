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
    public class HabitacionesController : Controller
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: Habitaciones
        public ActionResult Index()
        {
            var habitacions = db.Habitacions.Include(h => h.TipoHabitacion);
            return View(habitacions.ToList());
        }

        // GET: Habitaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Habitacion habitacion = db.Habitacions.Find(id);
            if (habitacion == null)
            {
                return HttpNotFound();
            }
            return View(habitacion);
        }

        // GET: Habitaciones/Create
        public ActionResult Create()
        {
            ViewBag.TipoHabitacionId = new SelectList(db.TipoHabitacions, "TipoHabitacionId", "Nombre");
            return View();
        }

        // POST: Habitaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HabitacionId,TipoHabitacionId")] Habitacion habitacion)
        {
            if (ModelState.IsValid)
            {
                db.Habitacions.Add(habitacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipoHabitacionId = new SelectList(db.TipoHabitacions, "TipoHabitacionId", "Nombre", habitacion.TipoHabitacionId);
            return View(habitacion);
        }

        // GET: Habitaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Habitacion habitacion = db.Habitacions.Find(id);
            if (habitacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoHabitacionId = new SelectList(db.TipoHabitacions, "TipoHabitacionId", "Nombre", habitacion.TipoHabitacionId);
            return View(habitacion);
        }

        // POST: Habitaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HabitacionId,TipoHabitacionId")] Habitacion habitacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(habitacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoHabitacionId = new SelectList(db.TipoHabitacions, "TipoHabitacionId", "Nombre", habitacion.TipoHabitacionId);
            return View(habitacion);
        }

        // GET: Habitaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Habitacion habitacion = db.Habitacions.Find(id);
            if (habitacion == null)
            {
                return HttpNotFound();
            }
            return View(habitacion);
        }

        // POST: Habitaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Habitacion habitacion = db.Habitacions.Find(id);
            db.Habitacions.Remove(habitacion);
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
