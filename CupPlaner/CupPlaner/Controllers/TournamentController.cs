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

            object obj = new { Id = t.Id, Name = t.Name, Divisions = divs, TimeIntervals = times};

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IdFromPass(string password)
        {
            Tournament t = db.TournamentSet.SingleOrDefault(x => x.Password == password);
            if (t == null)
            {
                return Json(new { Id = 0 });
            }
            return Json(new { Id = t.Id });
        }

        // POST: Tournament/Create
        [HttpPost]
        public ActionResult Create(string name, string password, List<DateTime> startTimes, List<DateTime> endTimes )
        {
            try
            {
                if (!db.TournamentSet.Any(x => x.Password == password))
                {
                    List<TimeInterval> tis = new List<TimeInterval>();
                    for (int i = 0; i < startTimes.Count; i++)
                    {
                        tis.Add(new TimeInterval() { StartTime = startTimes[i], EndTime = endTimes[i] });
                    }

                    db.TournamentSet.Add(new Tournament() { Name = name, Password = password, TimeIntervals = tis });
                    db.SaveChanges();

                    return Json(new { status = "success", message = "New tournament added" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = "error", message = "Password already exists" }, JsonRequestBehavior.AllowGet);          
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "New tournament not added", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Tournament/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string name, string password, List<DateTime> startTimes, List<DateTime> endTimes)
        {
            try
            {

                Tournament t = db.TournamentSet.Find(id);
                List<TimeInterval> tis = new List<TimeInterval>();
                for (int i = 0; i < startTimes.Count; i++)
                {
                    tis.Add(new TimeInterval() { StartTime = startTimes[i], EndTime = endTimes[i] });
                }
                t.Name = name;
                t.Password = password;
                t.TimeIntervals = tis;

                db.Entry(t).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { status = "success", message = "Tournament edited" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Tournament not edited", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        // POST: Tournament/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Tournament t = db.TournamentSet.Find(id);
                DivisionController dc = new DivisionController();
                foreach (Division d in t.Divisions)
                {
                    dc.Delete(d.Id);
                }
                db.TimeIntervalSet.RemoveRange(t.TimeIntervals);
                db.TournamentSet.Remove(t);
                db.SaveChanges();
                return Json(new { status = "success", message = "Tournament deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Tournament not deleted", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
