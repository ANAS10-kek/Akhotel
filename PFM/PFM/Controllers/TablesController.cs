using PFM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PFM.Controllers
{
    public class TablesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Tables
        public ActionResult Index()
        {
            ViewBag.listUsers = db.Users.ToList();
            ViewBag.listRooms = db.Rooms.ToList();
            return View();
        }

        //public JsonResult Index(string nameTable)
        //{
        //    switch (nameTable)
        //    {
        //        case "users":return Json(db.Users.ToList());
        //        case "room": return Json(db.Rooms.ToList());
        //        case "reser": return Json(db.Reservations.ToList());
        //        case "roomResd": return Json(JsonRequestBehavior.AllowGet);
        //        default: return Json(db.Users.ToList());
        //    }
        //}
    }
}