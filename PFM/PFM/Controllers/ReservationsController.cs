

using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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
        public ActionResult Create(int id)
        {
            Session["IdRoom"] = id;
            ViewBag.chambre = db.Rooms.Where(c => c.ChambreId == id).Single();
            ViewBag.ImagesRooms = db.RoomImages.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(string dates_From_To, string NbChambres, string NbPers)
        {
            if (ModelState.IsValid)
            {
                string[] Dates = dates_From_To.Split('-');
                Reservation reservation = new Reservation
                {
                    RoomId = int.Parse(Session["IdRoom"].ToString()),
                    Name=User.Identity.GetUserName(),
                    DateDebut = DateTime.ParseExact(Dates[0].Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    DateFin = DateTime.ParseExact(Dates[1].Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    NbChambres = int.Parse(NbChambres),
                    NbPers = int.Parse(NbPers),
                    Confirmation = false,
                    UserId = User.Identity.GetUserId(),
                };
                db.Entry(reservation).State = EntityState.Added;
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
