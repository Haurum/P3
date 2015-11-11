using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CupPlaner.Controllers
{
    public class MatchController : Controller
    {
        CupDBContainer db = new CupDBContainer();
        // GET: Match
        public ActionResult Index()
        {
            return View();
        }

        // GET: Match/Details/5
        public ActionResult Details(int id)
        {
            Match m = db.MatchSet.Find(id);
            object obj = new { Id = m.Id, StartTime = m.StartTime, Duration = m.Duration };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        // GET: Match/Create
        [HttpPost]
        public ActionResult Create(string startTime, int duration)
        {
            try
            {
                DateTime st = Convert.ToDateTime(startTime);
                db.MatchSet.Add(new Match() { StartTime = st, Duration = duration });
                db.SaveChanges();
                return Json(new { state = "new match added" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { state = "ERROR: new match not added", error = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        // GET: Match/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string startTime, int duration)
        {
            try
            {
                Match m = db.MatchSet.Find(id);
                if (startTime != null) m.StartTime = Convert.ToDateTime(startTime);
                if (duration > 0) m.Duration = duration;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { state = "match edited" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { state = "ERROR: match not edited", error = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Match/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Match m = db.MatchSet.Find(id);
                db.MatchSet.Remove(m);
                db.SaveChanges();

                return Json(new { state = "match deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "ERROR: match not deleted", error = ex.Message });
            }
        }
    }
}
