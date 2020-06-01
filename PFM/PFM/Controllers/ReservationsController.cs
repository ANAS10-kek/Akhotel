

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
        public ActionResult Create([Bind(Include = "Name,DateDebut,DateFin,NbChambres,NbPers")] Reservation reservation, int id)
        {
            if (ModelState.IsValid)
            {
                var room = db.Rooms.Find(id);
                reservation.RoomId = id;
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
