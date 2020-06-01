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
    public class RoomsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rooms    
        public ActionResult RoomListUser()
        {

            var roomsOutdated = from ro in db.Rooms
                                join rea in db.Reservations on ro.ChambreId equals rea.RoomId
                                where rea.DateFin < DateTime.Today
                                select new { RoomId = ro.ChambreId,nbChamber=rea.NbChambres};

            var rooms = db.Rooms.ToList();
            foreach(var r in rooms)
            {
                foreach(var roomOut in roomsOutdated)
                {
                    if(r.ChambreId == roomOut.RoomId)
                    {
                        r.Disponibilité += roomOut.nbChamber;
                    }
                }
            }

            db.SaveChanges();
      
            ViewBag.ImagesRooms = db.RoomImages.ToList();
            return View(rooms);
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }


            return RedirectToAction("Create", "Reservations");
        }
        // GET: Rooms/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChambreId,Titre,ImageId,Prix,TypeDeLit,Disponibilité,NbChambres,ShortDescription,LongDescription")] Room room)
        {

            if (ModelState.IsValid)
            {

                db.Rooms.Add(room);
                db.SaveChanges();
                int id = db.Rooms.ToList().Last().ChambreId;

                return RedirectToAction("Create/" + id, "roomImages");
            }

            return View(room);
        }


        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChambreId,Titre,ImageId,Prix,TypeDeLit,Disponibilité,NbChambres,ShortDescription,LongDescription")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("roomList");
            }
            return View(room);
        }

        // GET: Rooms/Delete/5

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
