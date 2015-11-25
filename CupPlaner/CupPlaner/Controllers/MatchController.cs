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
            try
            {
                Match m = db.MatchSet.Find(id);
                object obj = new { status = "success", Id = m.Id, StartTime = m.StartTime, Duration = m.Duration, Number = m.Number };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Could not find match", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public Match Create(int team1Id, int team2Id, int tournamentStageId )
        {
            try
            {
                Team team1 = db.TeamSet.Find(team1Id);
                Team team2 = db.TeamSet.Find(team2Id);
                List<Team> teams = new List<Team>() { team1, team2 };
                TournamentStage ts = db.TournamentStageSet.Find(tournamentStageId);
                Match m = db.MatchSet.Add(new Match() { Teams = teams, Duration = team1.Pool.Division.MatchDuration, TournamentStage = ts });
                db.SaveChanges();
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Match/Create
        [HttpPost]
        public ActionResult Schedule(int matchId, string startTime, int fieldId)
        {
            try
            {
                Match m = db.MatchSet.Find(matchId);
                DateTime st = Convert.ToDateTime(startTime);
                m.StartTime = st;
                Field f = db.FieldSet.Find(fieldId);
                m.Field = f;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { status = "success", message = "New match added" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "New match not added", details = ex.Message }, JsonRequestBehavior.AllowGet);

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

                return Json(new { status = "success", message = "Match edited" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Match not edited", details = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Match/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Match m = db.MatchSet.Find(id);
                foreach (Team t in m.Teams)
                {
                    t.Matches.Remove(m);
                }
                db.MatchSet.Remove(m);
                db.SaveChanges();

                return Json(new { status = "success", message = "Match deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error",  message = "Match not deleted", details = ex.Message });
            }
        }
    }
}
