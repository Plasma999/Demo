using System.Web.Mvc;

namespace APIDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult TestAPI()
        {
            ViewBag.Title = "Test API";

            return View();
        }

        public ActionResult MassInsert()
        {
            ViewBag.Title = "Mass Insert";

            return View();
        }
    }
}
