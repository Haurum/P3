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

        // GET: Match/Details/5 - Fetches the details of the class, takes the "id" parameter to determine the corresponding Match object.
        // Returns a Json object, which contains a copy of the corresponding Field variables.
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

        // POST: Match/Create - Tries to create a Match object, with the parameters "team1Id", "team2Id" and "tournamentStageId".
        // Sets the teams and tournament stage to the objects found through "team1Id", "team2Id" and "tournamentStageId".
        // Adds the Match object to the database MatchSet, and saves the changes in the database.
        // Returns a Json object with the match, or an exception if it failed creating the match.
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

        // GET: Match/Schedule - Finds the corresponding match for the MatchId parameter,
        // converts the startTime to a DateTime, finds the corresponding field for the fieldId
        // and schedules the match with the field object, and startTime parameter.
        // Saves this to the database, and returns a JSON object, indicating
        // wether the scheduling succeeded or not.
        [HttpPost]
        public ActionResult Schedule(int matchId, string startTime, int fieldId)
        {
            try
            {
                Match m = db.MatchSet.Find(matchId);
                DateTime st = Convert.ToDateTime(startTime);
                Field f = db.FieldSet.Find(fieldId);

                m.StartTime = st;
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

        // GET: Match/Edit/5 - Tries to edit a match, found by the id parameter.
        // Tries to edit the startTime and/or duration of the match, and saves to the database.
        // Returns a JSON object indicating, wether the edit was successful or not.
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

        // GET: Match/Delete/5 - Tries to delete a match, determined by it's id.
        // Removes the match from all teams containing the specific match,
        // and removes it from the database MatchSet. Saves the changes,
        // and returns a JSON object containing a status, indicating
        // wether the delete was successful or not.
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
