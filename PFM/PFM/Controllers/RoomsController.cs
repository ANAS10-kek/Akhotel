using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PFM.Models;

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
    }
}
