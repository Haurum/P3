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
                

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Team/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Team/Edit/5
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

        // GET: Team/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Team/Delete/5
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
