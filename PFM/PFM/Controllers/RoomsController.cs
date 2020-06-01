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
            var rooms = db.Rooms.ToList();
            ViewBag.ImagesRooms = db.RoomImages.ToList();
            return View(rooms);
        }
    }
}
