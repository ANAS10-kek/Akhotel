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
    }
}