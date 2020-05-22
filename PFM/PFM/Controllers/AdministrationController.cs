using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PFM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace PFM.Controllers
{
    public class AdministrationController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public AdministrationController()
        {
        }
        public AdministrationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Administration
        public ActionResult Index()
        {
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
            if(result.Succeeded)
            {
                ViewData["JavaScriptFunction"] = "successALert();";
               
            }
            return RedirectToAction("Details/"+usr.Id,"Administration");

        }

        // POST: Administration/Delete/5
        [HttpPost]
        public JsonResult DeleteClient(string id)
        {

            var usr = db.Users.Find(id);
            if (usr!=null)
            {
                db.Users.Remove(usr);
                db.SaveChanges();
            }
            return Json(db.Users.ToList(),JsonRequestBehavior.AllowGet);
        }
    }
}
