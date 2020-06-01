using PFM.Models;
using System.Web.Mvc;

namespace PFM.Controllers
{
    public class SharedController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Shared
        public ActionResult Index()
        {
            return View();
        }
    }
}