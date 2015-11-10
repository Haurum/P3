using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CupPlaner.Controllers
{
    public class TeamController : Controller
    {
        CupDBContainer db = new CupDBContainer();
        // GET: Team
        public ActionResult Index()
        {
            return View();
        }

        // GET: Team/Details/5
        public ActionResult Details(int id)
        {
            Team t = db.TeamSet.Find(id);
            List<object> times = new List<object>();
            if (t.TimeIntervals != null)
            {
                foreach (TimeInterval ti in t.TimeIntervals)
                {
                    times.Add(new { Id = ti.Id, StartTime = ti.StartTime, EndTime = ti.EndTime });
                }
            }

            object obj = new { Id = t.Id, Name = t.Name, TimeIntervals = times };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        // POST: Team/Create
        [HttpPost]
        public ActionResult Create(string name, int poolId)
        {
            try
            {
                Pool p = db.PoolSet.Find(poolId);
                db.TeamSet.Add(new Team() { Name = name, Pool = p });
                db.SaveChanges();

                return Json(new { state = "new team added" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: new team not added" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Team/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string name, int poolId, List<DateTime> startTimes, List<DateTime> endTimes)
        {
            try
            {
                List<TimeInterval> tis = new List<TimeInterval>();
                for (int i = 0; i < startTimes.Count; i++)
                {
                    tis.Add(new TimeInterval() { StartTime = startTimes[i], EndTime = endTimes[i] });
                }
                Team t = db.TeamSet.Find(id);
                t.Name = name;
                t.TimeIntervals = tis;

                db.Entry(t).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { state = "Team edited" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: Team not edited" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Team/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Team t = db.TeamSet.Find(id);
                db.TeamSet.Remove(t);
                db.SaveChanges();

                return Json(new { state = "Team Deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "Team not deleted" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
