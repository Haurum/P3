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
        // Database container, has functionalities to connect to the database classes.
        CupDBContainer db = new CupDBContainer();

        // GET: Team/Details/5 - Fetches the details of the class, takes the "id" parameter to determine the corresponding Team object.
        // Returns a Json object, which contains a copy of the corresponding Team variables.
        public ActionResult Details(int id)
        {
            try
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

                object obj = new { status = "success", Id = t.Id, Name = t.Name, TimeIntervals = times };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Could not find team", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Team/Create - Tries to create a Team object, with the parameters "name" and "poolId".
        // Tracks the corresponding Pool the team is to be contained in with the "poolId".
        // Sets the team name and pool to the parameters (name and corresponding Pool object).
        // Adds the Team object to the database TeamSet, and saves the changes in the database.
        // Returns a Json object with a state, indicating whether it succeeded creating the Team object or not.
        [HttpPost]
        public ActionResult Create(string name, int poolId)
        {
            try
            {
                Pool p = db.PoolSet.Find(poolId);

                Team t = db.TeamSet.Add(new Team() { Name = name, Pool = p, TimeIntervals = p.Division.Tournament.TimeIntervals });
                db.SaveChanges();

                return Json(new { status = "success", message = "New team added", id = t.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "New team not added", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Team/Edit/5 - Tries to edit a Team, determined by the "id" parameter and "poolId".
        // Edits a Teams name and list of TimeIntervals. Saves the changes to the database, if succeeded.
        // Returns a Json object with a state, indicating whether it succeeded editing the Team object or not.
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
                t.TimeIntervals.Clear();
                t.TimeIntervals = tis;

                db.Entry(t).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { status = "success", message = "Team edited" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Team not edited", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Team/Delete/5 - Tries to delete a Team object, determined by the "id".
        // Deletes the Team object and saves to the database, if succeeded.
        // Returns a Json object, indicating whether it succeeded deleting the Team object or not.
        [HttpPost]
        public ActionResult Delete(int id)
        {
           // try
           // {
                Team t = db.TeamSet.Find(id);
                db.MatchSet.RemoveRange(t.Matches);
                db.TeamSet.Remove(t);
                db.SaveChanges();

                return Json(new { status = "success", message = "Team Deleted" }, JsonRequestBehavior.AllowGet);
            //}
            /*catch (Exception ex)
            {
                return Json(new { status = "error", message = "Team not deleted", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }*/
        }
    }
}
