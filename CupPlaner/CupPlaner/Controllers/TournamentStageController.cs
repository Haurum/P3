﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CupPlaner.Controllers
{
    public class TournamentStageController : Controller
    {
        CupDBContainer db = new CupDBContainer();
        // GET: TournamentStage
        public ActionResult Index()
        {
            return View();
        }

        // GET: TournamentStage/Details/5
        public ActionResult Details(int id)
        {
            TournamentStage ts = db.TournamentStageSet.Find(id);
            List<object> matchlist = new List<object>();
            if (ts.Matches != null)
            {
                foreach (Match m  in ts.Matches)
                {
                    matchlist.Add(new Match() { Id = m.Id, StartTime = m.StartTime, Duration = m.Duration });
                }
            }

            object obj = new { Id = ts.Id, TournamentStructure = ts.TournamentStructure, Matches = matchlist };

            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        // POST: TournamentStage/Create
        [HttpPost]
        public ActionResult Create(int tournamentStructure, int divisionTournamentId, int poolId)
        {
            try
            {
                // TODO: Add insert logic here
                DivisionTournament dt = db.DivisionTournamentSet.Find(divisionTournamentId);
                Pool p = db.PoolSet.Find(poolId);
                db.TournamentStageSet.Add(new TournamentStage() { TournamentStructure = (TournamentStructure)tournamentStructure, DivisionTournament = dt, Pool = p });
                db.SaveChanges();

                return Json(new { state = "new tournament stage added" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { state = "ERROR: new tournament stage not added", error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: TournamentStage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, List<int> matchIds)
        {
            try
            {
                TournamentStage ts = db.TournamentStageSet.Find(id);
                ts.Matches.Clear();
                foreach (int matchId in matchIds)
                {
                    ts.Matches.Add(db.MatchSet.Find(matchId));
                }

                db.Entry(ts).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { state = "´tournament edited" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { state = "ERROR: tournament stage not edited", ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        // POST: TournamentStage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                TournamentStage ts = db.TournamentStageSet.Find(id);
                MatchController mc = new MatchController();
                foreach (Match m in ts.Matches)
                {
                    mc.Delete(m.Id);
                }
                db.TournamentStageSet.Remove(ts);
                db.SaveChanges();

                return Json(new { state = "tournament stage deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "ERROR: tournament stage not deleted", error = ex.Message });
            }
        }
    }
}