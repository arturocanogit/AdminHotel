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
    public class TarifasFestivosController : Controller
    {
        private AdminHotelApiContext db = new AdminHotelApiContext();

        // GET: TarifasFestivos
        public ActionResult Index()
        {
            var tarifasFestivos = db.TarifasFestivos.Include(t => t.TipoHabitacion);
            return View(tarifasFestivos.ToList());
        }

        // GET: TarifasFestivos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifaFestivo tarifaFestivo = db.TarifasFestivos.Find(id);
            if (tarifaFestivo == null)
            {
                return HttpNotFound();
            }
            return View(tarifaFestivo);
        }

        // GET: TarifasFestivos/Create
        public ActionResult Create()
        {
            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre");
            return View();
        }

        // POST: TarifasFestivos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HotelId,TarifaId,TipoHabitacionId,Precio,PrecioExtra,Activo,FechaAlta,FechaUpdate")] TarifaFestivo tarifaFestivo)
        {
            if (ModelState.IsValid)
            {
                db.TarifasFestivos.Add(tarifaFestivo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre", tarifaFestivo.HotelId);
            return View(tarifaFestivo);
        }

        // GET: TarifasFestivos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifaFestivo tarifaFestivo = db.TarifasFestivos.Find(id);
            if (tarifaFestivo == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre", tarifaFestivo.HotelId);
            return View(tarifaFestivo);
        }

        // POST: TarifasFestivos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HotelId,TarifaId,TipoHabitacionId,Precio,PrecioExtra,Activo,FechaAlta,FechaUpdate")] TarifaFestivo tarifaFestivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarifaFestivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelId = new SelectList(db.TiposHabitaciones, "HotelId", "Nombre", tarifaFestivo.HotelId);
            return View(tarifaFestivo);
        }

        // GET: TarifasFestivos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TarifaFestivo tarifaFestivo = db.TarifasFestivos.Find(id);
            if (tarifaFestivo == null)
            {
                return HttpNotFound();
            }
            return View(tarifaFestivo);
        }

        // POST: TarifasFestivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TarifaFestivo tarifaFestivo = db.TarifasFestivos.Find(id);
            db.TarifasFestivos.Remove(tarifaFestivo);
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
