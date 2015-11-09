using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CupPlaner.Controllers
{
    public class TournamentController : Controller
    {
        CupDBContainer db = new CupDBContainer();
        // GET: Tournament
        public ActionResult Index()
        {
            return View();
        }

        // GET: Tournament/Details/5
        public ActionResult Details(int id)
        {
            Tournament t = db.TournamentSet.Find(id);
            List<object> divs = new List<object>();
            List<object> times = new List<object>();
            List<object> dts = new List<object>();
            if (t.Divisions != null)
            {
                foreach (Division d in t.Divisions)
                {
                    divs.Add(new { Id = d.Id, Name = d.Name });
                } 
            }
            if (t.TimeIntervals != null)
            {
                foreach (TimeInterval ti in t.TimeIntervals)
                {
                    times.Add(new { Id = ti.Id, StartTime = ti.StartTime, EndTime = ti.EndTime});
                }
            }
            if (t.DivisionTournaments != null)
            {
                foreach (DivisionTournament dt in t.DivisionTournaments)
                {
                    dts.Add(new { Id = dt.Id });
                }
            }

            object obj = new { Id = t.Id, Name = t.Name, Divisions = divs, TimeIntervals = times, DivisionTournaments = dts };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        // POST: Tournament/Create
        [HttpPost]
        public ActionResult Create(string name, string password, List<DateTime> startTimes, List<DateTime> endTimes )
        {
            try
            {
                // TODO: Add insert logic here
                List<TimeInterval> tis = new List<TimeInterval>();
                for (int i = 0; i < startTimes.Count; i++)
                {
                    tis.Add(new TimeInterval() { StartTime = startTimes[i], EndTime = endTimes[i] });
                }
                Tournament t = new Tournament() { Name = name, Password = password, TimeIntervals = tis };
                db.TournamentSet.Add(t);
                db.SaveChanges();

                return Json(new { state = "new Tournament added" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: new Tournament not added" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Tournament/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string name, string password, List<DateTime> startTimes, List<DateTime> endTimes)
        {
            
            try
            {
                List<TimeInterval> tis = new List<TimeInterval>();
                for (int i = 0; i < startTimes.Count; i++)
                {
                    tis.Add(new TimeInterval() { StartTime = startTimes[i], EndTime = endTimes[i] });
                }
                Tournament t = db.TournamentSet.Find(id);
                t.Name = name;
                t.Password = password;
                t.TimeIntervals = tis;

                db.Entry(t).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { state = "Tournament edited" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: Tournament not edited" }, JsonRequestBehavior.AllowGet);
            }
        }


        // POST: Tournament/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Tournament t = db.TournamentSet.Find(id);
                DivisionController dc = new DivisionController();
                foreach (Division d in t.Divisions)
                {                  
                    dc.Delete(d.Id);
                }
                db.TournamentSet.Remove(t);
                db.SaveChanges();
                return Json(new { state = "Tournament deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: Tournament did not get deleted" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
