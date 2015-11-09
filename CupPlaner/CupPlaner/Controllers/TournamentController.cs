using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Create(string Name, string Password, List<DateTime> StartTimes, List<DateTime> EndTimes )
        {
            try
            {
                // TODO: Add insert logic here
                List<TimeInterval> tis = new List<TimeInterval>();
                for (int i = 0; i < StartTimes.Count; i++)
                {
                    tis.Add(new TimeInterval() { StartTime = StartTimes[i], EndTime = EndTimes[i] });
                }
                Tournament t = new Tournament() { Name = Name, Password = Password, TimeIntervals = tis };

                return Json(new { state = "new Tournament added" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: new Tournament did not get added" }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Tournament/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tournament/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Tournament/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tournament/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
