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
    public class ReservacionesController : Controller
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: Reservaciones
        public ActionResult Index()
        {
            var reservaciones = db.Reservaciones.Include(r => r.Cliente).Include(r => r.Habitacion).Include(r => r.TipoHabitacion);
            return View(reservaciones.ToList());
        }

        // GET: Reservaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservacion reservacion = db.Reservaciones.Find(id);
            if (reservacion == null)
            {
                return HttpNotFound();
            }
            return View(reservacion);
        }

        // GET: Reservaciones/Create
        public ActionResult Create()
        {
            ViewBag.HotelId = new SelectList(db.Clientes, "HotelId", "Nombre");
            ViewBag.HotelId = new SelectList(db.Habitaciones, "HotelId", "NumeroHabitacion");
            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre");
            return View();
        }

        // POST: Reservaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HotelId,ReservacionId,ClienteId,HabitacionId,TipoHabitacionId,Activo,FechaAlta,FechaUpdate")] Reservacion reservacion)
        {
            if (ModelState.IsValid)
            {
                db.Reservaciones.Add(reservacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelId = new SelectList(db.Clientes, "HotelId", "Nombre", reservacion.HotelId);
            ViewBag.HotelId = new SelectList(db.Habitaciones, "HotelId", "NumeroHabitacion", reservacion.HotelId);
            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre", reservacion.HotelId);
            return View(reservacion);
        }

        // GET: Reservaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservacion reservacion = db.Reservaciones.Find(id);
            if (reservacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelId = new SelectList(db.Clientes, "HotelId", "Nombre", reservacion.HotelId);
            ViewBag.HotelId = new SelectList(db.Habitaciones, "HotelId", "NumeroHabitacion", reservacion.HotelId);
            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre", reservacion.HotelId);
            return View(reservacion);
        }

        // POST: Reservaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HotelId,ReservacionId,ClienteId,HabitacionId,TipoHabitacionId,Activo,FechaAlta,FechaUpdate")] Reservacion reservacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelId = new SelectList(db.Clientes, "HotelId", "Nombre", reservacion.HotelId);
            ViewBag.HotelId = new SelectList(db.Habitaciones, "HotelId", "NumeroHabitacion", reservacion.HotelId);
            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre", reservacion.HotelId);
            return View(reservacion);
        }

        // GET: Reservaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservacion reservacion = db.Reservaciones.Find(id);
            if (reservacion == null)
            {
                return HttpNotFound();
            }
            return View(reservacion);
        }

        // POST: Reservaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservacion reservacion = db.Reservaciones.Find(id);
            db.Reservaciones.Remove(reservacion);
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
