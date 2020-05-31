using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PFM.Models;
using PFM.Models.ModelsReservation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace PFM.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public AdministrationController()
        {
        }
        public AdministrationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            private set { _roleManager = value; }
        }
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Administration
        public ActionResult Index()
        {
            Session["CountUsers"] = db.Users.ToList().Count;
            Session["CountResr"] = db.Reservations.ToList().Count;
            Session["CountRooms"] = db.Rooms.ToList().Count;
            Session["CountRoomReserved"] = 0;
            ViewBag.data = this.db.Users
                                   .Where(x => x.Id != null)
                                   .GroupBy(s => new { date = s.Datesinup.Month })
                                   .Select(x => new { count = x.Count(), date = x.Key.date })
                                    .ToList();
            ViewBag.EmailNotConfirmed = this.db.Users.Where(x => x.EmailConfirmed == false).ToList().Count;
            ViewBag.EmailConfirmed = this.db.Users.Where(x => x.EmailConfirmed == true).ToList().Count;
            return View();
        }
        //Get List User
        public ActionResult GetListUser()
        {

            var users = db.Users.ToList();
            List<ApplicationUser> ListUs = new List<ApplicationUser>();
            foreach (var item in users)
            {
                ListUs.Add(item);
            }
            foreach (var item in ListUs)
            {
                if (item.Country == null)
                {
                    item.Country = "null";
                }
                else
                {
                    item.Country = db.Countries.Find(int.Parse(item.Country)).name;
                }
                if (item.State == null)
                {
                    item.State = "null";
                }
                else
                {
                    item.State = db.States.Find(int.Parse(item.State)).name;
                }
                if (item.City == null)
                {
                    item.City = "null";
                }
                else
                {
                    item.City = db.Cities.Find(int.Parse(item.City)).name;
                }
                if (item.Address == null)
                {
                    item.Address = "null";
                }


            }
            if (ListUs != null)
            {
                return View(ListUs);
            }
            else
            {
                return View("Error");
            }
        }

        // GET: Administration/Edit/5
        public ActionResult Details(string id)
        {

            ApplicationUser usrTmp = new ApplicationUser();
            var usr = db.Users.Find(id);
            usrTmp = usr;
            if (usr.Country == null)
            {
                usrTmp.Country = "null";
            }
            else
            {
                usrTmp.Country = db.Countries.Find(int.Parse(usr.Country)).name;
            }
            if (usr.State == null)
            {
                usr.State = "null";
            }
            else
            {
                usrTmp.State = db.States.Find(int.Parse(usr.State)).name;
            }
            if (usr.City == null)
            {
                usrTmp.City = "null";
            }
            else
            {
                usrTmp.City = db.Cities.Find(int.Parse(usr.City)).name;
            }
            if (usr.Address == null)
            {
                usrTmp.Address = "null";
            }
            if (usr == null)
            {
                return View("Error");
            }
            else
            {
                Session["id"] = id;
                return View(usrTmp);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewPassword(ChangePasswordViewModel model)
        {
            var usr = db.Users.Find(Session["id"].ToString());
            usr.PasswordHash = null;
            db.SaveChanges();
            var result = await UserManager.AddPasswordAsync(Session["id"].ToString(), model.NewPassword);
            if (result.Succeeded)
            {
                ViewData["JavaScriptFunction"] = "successALert();";

            }
            return RedirectToAction("Details/" + usr.Id, "Administration");

        }

        // POST: Administration/Delete/5
        [HttpPost]
        public JsonResult DeleteClient(string id)
        {

            var usr = db.Users.Find(id);
            if (usr != null)
            {
                db.Users.Remove(usr);
                db.SaveChanges();
            }
            return Json(db.Users.ToList(), JsonRequestBehavior.AllowGet);
        }
        //*******************************ROOMS****************************************

        //list Room
        public ActionResult roomList()
        {
            var rooms = db.Rooms.ToList();
            return View(rooms);
        }
        //create Rooms
        int idR;
        public ActionResult CreateRoom()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateRoom(IEnumerable<HttpPostedFileBase> files, [Bind(Include = "ChambreId,Titre,Prix,TypeDeLit,Disponibilité,NbChambres,ShortDescription,LongDescription")] Room room)
        {
            if (files == null)
            {
                return View();
            }
            else
            {
                RoomImage roomImage;
                db.Rooms.Add(room);
                db.SaveChanges();
                idR = db.Rooms.ToList().Last().ChambreId;
                foreach (var img in files)
                {
                    roomImage = new RoomImage();
                    roomImage.RoomId = idR;
                    roomImage.Name = Path.GetFileName(img.FileName);
                    roomImage.FullPath = Server.MapPath("/pic/rooms_pic/" + img.FileName);
                    img.SaveAs(roomImage.FullPath);
                    db.RoomImages.Add(roomImage);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("roomList");
        }
        //Edit Room

        public ActionResult DetailsRoom(int id)
        {
            var currentRoom = db.Rooms.Find(id);
            ViewBag.imagesCurrentRoom = db.RoomImages.ToList().Where(c => c.RoomId == id);
            idR = id;
            return View(currentRoom);
        }
        [HttpPost]
        public ActionResult DetailsRoom(Room UpdatedRoom, IEnumerable<HttpPostedFileBase> files, int id)
        {
            var currentRoom = db.Rooms.Find(id);
            currentRoom.Disponibilité = UpdatedRoom.Disponibilité;
            currentRoom.Prix = UpdatedRoom.Prix;
            currentRoom.NbChambres = UpdatedRoom.NbChambres;
            currentRoom.ShortDescription = UpdatedRoom.ShortDescription;
            currentRoom.LongDescription = UpdatedRoom.LongDescription;
            currentRoom.TypeDeLit = UpdatedRoom.TypeDeLit;
            currentRoom.Titre = UpdatedRoom.Titre;
            db.SaveChanges();
            foreach (var img in files)
            {
                if (img != null)
                {
                    RoomImage roomImage = new RoomImage();
                    roomImage.RoomId = id;
                    roomImage.Name = Path.GetFileName(img.FileName);
                    roomImage.FullPath = Server.MapPath("/pic/rooms_pic/" + img.FileName);
                    img.SaveAs(roomImage.FullPath);
                    db.RoomImages.Add(roomImage);
                    db.SaveChanges();
                }

            }
            ViewBag.imagesCurrentRoom = db.RoomImages.ToList().Where(c => c.RoomId == id);
            return View(currentRoom);
        }
        //Delete Room
        [HttpPost]
        public JsonResult DeleteRoom(int id)
        {
            var room = db.Rooms.Find(id);
            if (room != null)
            {
                db.Rooms.Remove(room);
                db.SaveChanges();
            }
            return Json(db.Rooms.ToList(), JsonRequestBehavior.AllowGet);
        }
        //delete Image Room
        [HttpPost]
        public JsonResult DeleteImageRoom(int id)
        {

            var roomImage = db.RoomImages.Find(id);
            if (roomImage != null)
            {
                db.RoomImages.Remove(roomImage);
                db.SaveChanges();
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        //role List
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [HttpPost]
        public JsonResult CreateRolee(string Name)
        {
            ApplicationRole role = new ApplicationRole();
            var roles = db.IdentityRoles.ToList();

            if (roles.Count <= 0)
            {
                role.Id = RandomString(1);
            }
            else
            {

                role.Id = roles.Last().Id + RandomString(1);
            }
            role.Name = Name;
            db.Roles.Add(role);
            db.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult DeleteRole(string id)
        {

            var role = db.Roles.Find(id);
            if (role != null)
            {
                db.Roles.Remove(role);
                db.SaveChanges();
            }
            return Json(db.Roles.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult roleList()
        {
            var roles = db.IdentityRoles.ToList();
            return View(roles);
        }
        public ActionResult ListUserInRole(string id)
        {
            var currentRole = db.Roles.Find(id);
            Session["nameRole"] = currentRole.Name.ToString();

            var CurrentRoles = db.Roles.Find(id).Users;
            var allUser = db.Users.ToList();
            ViewBag.listUser = from user in db.Users
                               where user.Roles.Any(r => r.RoleId == id)
                               select user;
            List<ApplicationUser> UsersOutRole = new List<ApplicationUser>();
            ViewBag.listAllUser = db.Users.Where(m => m.Roles.All(r => r.RoleId != id)).ToList();

            return View();
        }
        public JsonResult addUserTorole(string id, string role)
        {
            UserManager.AddToRole(id, role);
            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult deleteUserfromRole(string id, string role)
        {
            UserManager.RemoveFromRole(id, role);
            return Json(JsonRequestBehavior.AllowGet);
        }
        //index COunt
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChartView()
        {
            ViewBag.data = this.db.Users
                                 .Where(x => x.Id != null)
                                 .GroupBy(s => new { date = s.Datesinup.Month })
                                 .Select(x => new { count = x.Count(), date = x.Key.date })
                                  .ToList();
            ViewBag.EmailNotConfirmed = this.db.Users.Where(x => x.EmailConfirmed == false).ToList().Count;
            ViewBag.EmailConfirmed = this.db.Users.Where(x => x.EmailConfirmed == true).ToList().Count;
            return View();
         }
    }
}
