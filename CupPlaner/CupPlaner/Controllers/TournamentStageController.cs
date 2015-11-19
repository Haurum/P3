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
            try
            {
                TournamentStage ts = db.TournamentStageSet.Find(id);
                List<object> matchlist = new List<object>();
                if (ts.Matches != null)
                {
                    foreach (Match m in ts.Matches)
                    {
                        matchlist.Add(new Match() { Id = m.Id, StartTime = m.StartTime, Duration = m.Duration });
                    }
                }

                object obj = new { status = "success", Id = ts.Id, TournamentStructure = ts.TournamentStructure, Matches = matchlist };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Could not find tournament stage", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: TournamentStage/Create
        [HttpPost]
        public TournamentStage Create(int divisionTournamentId, int poolId)
        {
            try
            {
                // TODO: Add insert logic here
                DivisionTournament dt = db.DivisionTournamentSet.Find(divisionTournamentId);
                Pool p = db.PoolSet.Find(poolId);
                TournamentStage ts = db.TournamentStageSet.Add(new TournamentStage() { TournamentStructure = dt.TournamentStructure, DivisionTournament = dt, Pool = p });
                db.SaveChanges();
                return ts;
            }
            catch (Exception ex)
            {
                throw ex;
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

                return Json(new { status = "success", message = "Tournament stage edited" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Tournament stage not edited", details = ex.Message }, JsonRequestBehavior.AllowGet);

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
                //db.MatchSet.RemoveRange(ts.Matches);
                db.TournamentStageSet.Remove(ts);
                db.SaveChanges();

                return Json(new { status = "success", message = "Tournament stage deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Tournament stage not deleted", details = ex.Message });
            }
        }
    }
}
