using System.Web.Mvc;

namespace PFM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {

            return View();
        }
        public ActionResult Gallery()
        {
            return View();
        }
    }
}