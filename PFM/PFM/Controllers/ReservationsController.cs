using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PFM.Models;
using PFM.Models.ModelsReservation;

namespace PFM.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            var reservations = db.Reservations.Include(r => r.Room);
            return View(reservations.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create(int id)
        {
            ViewBag.chambre = db.Rooms.Where(c => c.ChambreId == id).Single();
            ViewBag.ImagesRooms = db.RoomImages.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,DateDebut,DateFin,NbChambres,NbPers")] Reservation reservation,int id)
        {
            if (ModelState.IsValid)
            {
                reservation.RoomId = id;
                reservation.Confirmation = "Non";
                reservation.UserId = User.Identity.GetUserId();
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("RoomListUser");
            }
            return View(reservation);
        }

          
            
    

    // GET: Reservations/Edit/5
    public ActionResult Edit(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Reservation reservation = db.Reservations.Find(id);
        if (reservation == null)
        {
            return HttpNotFound();
        }
        ViewBag.RoomId = new SelectList(db.Rooms, "ChambreId", "Titre", reservation.RoomId);
        return View(reservation);
    }

    // POST: Reservations/Edit/5
    // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
    // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "ReservationId,RoomId,UserId,DateDebut,DateFin,Confirmation,NbChambres")] Reservation reservation)
    {
        if (ModelState.IsValid)
        {
            db.Entry(reservation).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.RoomId = new SelectList(db.Rooms, "ChambreId", "Titre", reservation.RoomId);
        return View(reservation);
    }

    // GET: Reservations/Delete/5
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Reservation reservation = db.Reservations.Find(id);
        if (reservation == null)
        {
            return HttpNotFound();
        }
        return View(reservation);
    }

    // POST: Reservations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        Reservation reservation = db.Reservations.Find(id);
        db.Reservations.Remove(reservation);
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
