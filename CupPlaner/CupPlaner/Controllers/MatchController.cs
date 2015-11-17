﻿using System;
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

        [HttpPost]
        public ActionResult Create(int team1Id, int team2Id )
        {
            try
            {
                Team t1 = db.TeamSet.Find(team1Id);
                Team t2 = db.TeamSet.Find(team2Id);
                List<Team> teams = new List<Team>() { t1, t2 };
                db.MatchSet.Add(new Match() { Teams = teams, Duration = t1.Pool.Division.MatchDuration });
                db.SaveChanges();
                return Json(new { status = "success", message = "New match added" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "New match not added", details = ex.Message }, JsonRequestBehavior.AllowGet);

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
