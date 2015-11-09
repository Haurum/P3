using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CupPlaner.Controllers
{
    public class HomeController : Controller
    {
        CupDBContainer db = new CupDBContainer();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Hejhej(string name)
        {
            db.TournamentSet.Add(new Tournament() { Name = name });
            db.SaveChanges();
            return Json(new { LOL = "hej" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHejs()
        {
            List<Tournament> ts = db.TournamentSet.ToList();
            return Json(new { Name = ts.First().Name }, JsonRequestBehavior.AllowGet);
        }
    }
}