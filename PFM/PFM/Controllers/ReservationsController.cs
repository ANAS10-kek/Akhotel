

using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PFM.Models;
using PFM.Models.ModelsReservation;

namespace PFM.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Reservations/Create
        int idRoom;
        public ActionResult Create(int id)
        {
            idRoom = id;
            ViewBag.chambre = db.Rooms.Where(c => c.ChambreId == id).Single();
            ViewBag.ImagesRooms = db.RoomImages.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(string DateDebut, string DateFin,string NbChambres,string NbPers)
        {
            if (ModelState.IsValid)
            {
                Reservation reservation = new Reservation();
                reservation.DateDebut =DateTime.Parse(DateDebut.ToString().Trim());
                reservation.DateFin = DateTime.Parse(DateFin.ToString().Trim());
                reservation.NbChambres =int.Parse(NbChambres);
                reservation.NbPers = int.Parse(NbPers);
                reservation.RoomId = idRoom;
                reservation.Confirmation = false;
                reservation.UserId = User.Identity.GetUserId();
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("ConfirmedReservation");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ConfirmedReservation()
        {
            return View();
        }
    }
}
