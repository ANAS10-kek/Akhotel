using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PFM.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PFM.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ManageController()
        {
        }
        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: /Manage/Index
        public ActionResult Index()
        {
            if(!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }else
            {
                if (User.Identity.GetUserId() == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                string id = User.Identity.GetUserId();
                var model = db.Users.Where(m => m.Id == id).Single();
                if (model == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
        }  
        public ActionResult SecurityView()
        {
            if(!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }else
            {
                if (User.Identity.GetUserId() == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                string id = User.Identity.GetUserId();
                var model = db.Users.Where(m => m.Id == id).Single();
                if (model == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            }
        }
        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }
       //a
        // GET: /Manage/editEmail
        public ActionResult editEmail()
        {
            string id = User.Identity.GetUserId();
            var model = db.Users.Where(m => m.Id == id).Single();
            return View("editEmail", "_LayoutManage", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> editEmail(ApplicationUser model)
        {
            bool find = false;
            if (ModelState.IsValid)
            {

                string id = User.Identity.GetUserId();
                var user = db.Users.Where(m => m.Id == id).Single();
                foreach (var us in db.Users)
                {
                    if (us.Email == model.Email)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    user.Email = model.Email;
                    user.EmailConfirmed = false;
                    db.SaveChanges();
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirmez votre compte", "Confirmez votre compte en cliquant <a href=\"" + callbackUrl + "\">ici</a>");
                    ViewData["JavaScriptFunction"] = "successALert();";
                    return View("editEmail", "_LayoutManage", model);

                }
                else if (find)
                {
                    if (user.Email == model.Email)
                    {
                        ViewData["JavaScriptFunction"] = "successALert();";
                    }
                    else
                    {
                        ViewData["JavaScriptFunction"] = "errorAlert();";
                    }
                    return View("editEmail", "_LayoutManage", model);
                }
            }
            return View("editEmail", "_LayoutManage");
        }
        // POST: /Manage/AddPhoneNumber
        public ActionResult editPhoneNumber()
        {
            string id = User.Identity.GetUserId();
            var model = db.Users.Where(m => m.Id == id).Single();
            return View(model);
        }
        [HttpPost]
        public ActionResult editPhoneNumber(ApplicationUser model)
        {
            bool find = false;
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId();
                var user = db.Users.Where(m => m.Id == id).Single();
                foreach (var us in db.Users)
                {
                    if (us.PhoneNumber == model.PhoneNumber)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    user.PhoneNumber = model.PhoneNumber;
                    db.SaveChanges();
                    ViewData["JavaScriptFunction"] = "successALert();";
                    return View("editPhoneNumber", "_LayoutManage", user);
                }
                else if (find)
                {
                    if (user.PhoneNumber == model.PhoneNumber)
                    {
                        ViewData["JavaScriptFunction"] = "successALert();";
                    }
                    else
                    {
                        ViewData["JavaScriptFunction"] = "errorAlert();";
                    }
                    db.SaveChanges();
                    return View("editPhoneNumber", "_LayoutManage", user);
                }
            }
            return View();
        }
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("editPhoneNumber", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("editPhoneNumber",new { Message = ManageMessageId.RemovePhoneSuccess });
        }
        //// GET: /Manage/SetotherInfo
        public ActionResult SetotherInfo()
        {

            string id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Users.Where(m => m.Id == id).Single();
            if (model.Country != null)
            {
                int idcountry = int.Parse(model.Country);
                ViewBag.CountryUser = new SelectList(db.Countries.Where(m => m.id == idcountry), "id", "name");
            }
            else
            {
                ViewBag.CountryUser = "Empty";
            }
            if (model.City != null)
            {
                int idCity = int.Parse(model.City);
                ViewBag.CityUser = new SelectList(db.Cities.Where(m => m.id == idCity), "id", "name");
            }
            else
            {
                ViewBag.CityUser = "Empty";
            }
            if (model.State != null)
            {
                int idcState = int.Parse(model.State);
                ViewBag.SateUser = new SelectList(db.States.Where(m => m.id == idcState), "id", "name");
            }
            else
            {
                ViewBag.SateUser = "Empty";
            }
            var countries = db.Countries.ToList();
            ViewBag.Countries = new SelectList(countries, "id", "name");
            return View("SetotherInfo", "_LayoutManage",model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetotherInfo(ApplicationUser model)
        {
            bool enter = false;
            string id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Where(m => m.Id == id).Single();

            if (model.City != null && model.Country != null && model.State != null)
            {
                user.City = model.City;
                user.Country = model.Country;
                user.State = model.State;
                enter = true;
            }
            if (model.Address != null)
            {
                user.Address = model.Address;
                enter = true;

            }
            db.SaveChanges();
            if (user.Country != null)
            {
                int idcountry = int.Parse(user.Country);
                ViewBag.CountryUser = new SelectList(db.Countries.Where(m => m.id == idcountry), "id", "name");
            }
            else
            {
                ViewBag.CountryUser = null;
            }
            if (user.City != null)
            {
                int idCity = int.Parse(user.City);
                ViewBag.CityUser = new SelectList(db.Cities.Where(m => m.id == idCity), "id", "name");
            }
            else
            {
                ViewBag.CityUser = null;
            }
            if (user.State != null)
            {
                int idcState = int.Parse(user.State);
                ViewBag.SateUser = new SelectList(db.States.Where(m => m.id == idcState), "id", "name");
            }
            else
            {
                ViewBag.SateUser = null;
            }
            var countries = db.Countries.ToList();
            ViewBag.Countries = new SelectList(countries, "id", "name");
            if (enter)
            {
                ViewData["JavaScriptFunction"] = "successALert();";
            }
            else
            {
                ViewData["JavaScriptFunction"] = "errorAlert();";
            }
            return View(user);
        }
        [HttpPost]
        public JsonResult GetStatesList(string Countries_id)
        {
            int id = int.Parse(Countries_id);
            var states = db.States.Where(m => m.Countries.id == id).ToList();
            return Json(new SelectList(states, "id", "name"));
        }
        [HttpPost]
        public JsonResult GetCitiesList(string State_id)
        {
            int id = int.Parse(State_id);
            var cities = db.Cities.Where(m => m.States.id == id).ToList();
            return Json(new SelectList(cities, "id", "name"));
        }
        //
        // GET: /Manage/ChangePassword
        //PAsswordEdit.Add
        public ActionResult editPassword()
        {
            if (User.Identity.GetUserId() == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string id = User.Identity.GetUserId();
            var model = db.Users.Where(m => m.Id == id).Single();
            if (model == null)
            {
                return HttpNotFound();
            }
            if (model.PasswordHash == null)
            {
                return RedirectToAction("SetPassword");
            }
            else
            {
                return RedirectToAction("ChangePassword");
            }
           
        }
        //
        // POST: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            string id = User.Identity.GetUserId();
            var Info = db.Users.Where(m => m.Id == id).Single();
            if (Info.PasswordHash == null)
            {
                return RedirectToAction("SetPassword", "Manage");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            else
            {
                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    ViewData["JavaScriptFunction"] = "successALert();";
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                }
                AddErrors(result);
                return View(model);
            }
        }
        //
        // POST: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            string id = User.Identity.GetUserId();
            var Info = db.Users.Where(m => m.Id == id).Single();
            if (Info.PasswordHash != null)
            {
                return RedirectToAction("ChangePassword", "Manage");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    ViewData["JavaScriptFunction"] = "successALert();";
                    return RedirectToAction("ChangePassword");
                }
                AddErrors(result);
            }
          
            return View(model);
        }
        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "La connexion externe a été supprimée."
                : message == ManageMessageId.Error ? "Une erreur s'est produite."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }
        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Demander une redirection vers le fournisseur de connexion externe afin de lier une connexion pour l'utilisateur actuel
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }
        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }
        public ActionResult editUsername()
        {
            var model = db.Users.Find(User.Identity.GetUserId());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editUsername(ApplicationUser model)
        {
            bool find = false;
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId();
                var user = db.Users.Where(m => m.Id == id).Single();
                foreach (var us in db.Users)
                {
                    if (us.UserName == model.UserName)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    user.UserName = model.UserName;
                    db.SaveChanges();
                    ViewData["JavaScriptFunction"] = "successALert();";
                    return View(user);
                }
                else if(find)
                {
                    if (user.UserName == model.UserName)
                    {
                        ViewData["JavaScriptFunction"] = "successALert();";
                    }
                    else
                    {
                        ViewData["JavaScriptFunction"] = "errorAlert();";
                    }
                    return View(user);
                }
            }
            db.SaveChanges();
            return View(model);
        }
        public ActionResult _MenuRightSide()
        {
            var model = db.Users.Find(User.Identity.GetUserId());
            return View(model);
        }
        public ActionResult info()
        {
            return View();
        } 
        public ActionResult GeneralSettingsSection()
        {
            return RedirectToAction("Index","Manage");
        }

        //delete Account
        public ActionResult RemoveAccount()
        {
            if (User.Identity.GetUserId() == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string id = User.Identity.GetUserId();
            var user = db.Users.Where(m => m.Id == id).Single();
            if (user == null)
            {
                return HttpNotFound();
            }
            if (user != null)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                db.Users.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Programmes d'assistance
        // Utilisé pour la protection XSRF lors de l'ajout de connexions externes
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }


        #endregion
    }
}