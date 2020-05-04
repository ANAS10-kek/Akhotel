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
    public class RoomImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RoomImages
        public ActionResult Index()
        {
            var roomImages = db.RoomImages.Include(r => r.Room);
            return View(roomImages.ToList());
        }

        // GET: RoomImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomImage roomImage = db.RoomImages.Find(id);
            if (roomImage == null)
            {
                return HttpNotFound();
            }
            return View(roomImage);
        }

        // GET: RoomImages/Create
        public ActionResult Create()
        {
            ViewBag.RoomId = new SelectList(db.Rooms, "ChambreId", "Titre");
            return View();
        }

        // POST: RoomImages/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ImageId,Name,Size,ImageData,RoomId")] RoomImage roomImage)
        {
            if (ModelState.IsValid)
            {
                db.RoomImages.Add(roomImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomId = new SelectList(db.Rooms, "ChambreId", "Titre", roomImage.RoomId);
            return View(roomImage);
        }

        // GET: RoomImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomImage roomImage = db.RoomImages.Find(id);
            if (roomImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "ChambreId", "Titre", roomImage.RoomId);
            return View(roomImage);
        }

        // POST: RoomImages/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ImageId,Name,Size,ImageData,RoomId")] RoomImage roomImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomId = new SelectList(db.Rooms, "ChambreId", "Titre", roomImage.RoomId);
            return View(roomImage);
        }

        // GET: RoomImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomImage roomImage = db.RoomImages.Find(id);
            if (roomImage == null)
            {
                return HttpNotFound();
            }
            return View(roomImage);
        }

        // POST: RoomImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomImage roomImage = db.RoomImages.Find(id);
            db.RoomImages.Remove(roomImage);
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
