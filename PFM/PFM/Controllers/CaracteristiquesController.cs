using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PFM.Models;
using PFM.Models.ModelsReservation;

namespace PFM.Controllers
{
    public class CaracteristiquesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Caracteristiques
        public ActionResult Index()
        {
            var caracteristiques = db.Caracteristiques.Include(c => c.Room);
            return View(caracteristiques.ToList());
        }

        // GET: Caracteristiques/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caracteristique caracteristique = db.Caracteristiques.Find(id);
            if (caracteristique == null)
            {
                return HttpNotFound();
            }
            return View(caracteristique);
        }

        // GET: Caracteristiques/Create
        public ActionResult Create()
        {
            ViewBag.RoomId = new SelectList(db.Rooms, "ChambreId", "Titre");
            return View();
        }

        // POST: Caracteristiques/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CaracId,Description,RoomId")] Caracteristique caracteristique)
        {
            if (ModelState.IsValid)
            {
                db.Caracteristiques.Add(caracteristique);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomId = new SelectList(db.Rooms, "ChambreId", "Titre", caracteristique.RoomId);
            return View(caracteristique);
        }

        // GET: Caracteristiques/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caracteristique caracteristique = db.Caracteristiques.Find(id);
            if (caracteristique == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "ChambreId", "Titre", caracteristique.RoomId);
            return View(caracteristique);
        }

        // POST: Caracteristiques/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CaracId,Description,RoomId")] Caracteristique caracteristique)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caracteristique).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "ChambreId", "Titre", caracteristique.RoomId);
            return View(caracteristique);
        }

        // GET: Caracteristiques/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caracteristique caracteristique = db.Caracteristiques.Find(id);
            if (caracteristique == null)
            {
                return HttpNotFound();
            }
            return View(caracteristique);
        }

        // POST: Caracteristiques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Caracteristique caracteristique = db.Caracteristiques.Find(id);
            db.Caracteristiques.Remove(caracteristique);
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
