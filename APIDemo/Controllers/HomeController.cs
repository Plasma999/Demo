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

        public ActionResult MassUpdate()
        {
            ViewBag.Title = "Mass Update";

            return View();
        }

        public ActionResult PieChart()
        {
            ViewBag.Title = "Pie Chart";

            return View();
        }

        public ActionResult Exam()
        {
            ViewBag.Title = "Exam";

            return View();
        }
    }
}
