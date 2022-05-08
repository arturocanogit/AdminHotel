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
    public class TarifasController : Controller
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: Tarifas
        public ActionResult Index()
        {
            var tarifas = db.Tarifas.Include(t => t.TipoHabitacion);
            return View(tarifas.ToList());
        }

        // GET: Tarifas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarifa tarifa = db.Tarifas.Find(id);
            if (tarifa == null)
            {
                return HttpNotFound();
            }
            return View(tarifa);
        }

        // GET: Tarifas/Create
        public ActionResult Create()
        {
            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre");
            return View();
        }

        // POST: Tarifas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HotelId,TipoHabitacionId,TarifaId,DiaSemanaId,Personas,Precio,Activo,FechaAlta,FechaUpdate")] Tarifa tarifa)
        {
            if (ModelState.IsValid)
            {
                db.Tarifas.Add(tarifa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre", tarifa.HotelId);
            return View(tarifa);
        }

        // GET: Tarifas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarifa tarifa = db.Tarifas.Find(id);
            if (tarifa == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre", tarifa.HotelId);
            return View(tarifa);
        }

        // POST: Tarifas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HotelId,TipoHabitacionId,TarifaId,DiaSemanaId,Personas,Precio,Activo,FechaAlta,FechaUpdate")] Tarifa tarifa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarifa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre", tarifa.HotelId);
            return View(tarifa);
        }

        // GET: Tarifas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarifa tarifa = db.Tarifas.Find(id);
            if (tarifa == null)
            {
                return HttpNotFound();
            }
            return View(tarifa);
        }

        // POST: Tarifas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tarifa tarifa = db.Tarifas.Find(id);
            db.Tarifas.Remove(tarifa);
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
