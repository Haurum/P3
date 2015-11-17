using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CupPlaner.Controllers
{
    public class DivisionTournamentController : Controller
    {
        CupDBContainer db = new CupDBContainer();

        // GET: DivisionTournament
        public ActionResult Index()
        {
            return View();
        }

        // GET: DivisionTournament/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                DivisionTournament dt = db.DivisionTournamentSet.Find(id);
                List<object> tournamentStages = new List<object>();
                if (dt.TournamentStage != null)
                {
                    foreach (TournamentStage ts in dt.TournamentStage)
                    {
                        List<object> matches = new List<object>();
                        if (ts.Matches != null)
                        {
                            foreach (Match m in ts.Matches)
                            {
                                matches.Add(new { Id = m.Id, StartTime = m.StartTime, Duration = m.Duration });
                            }
                        }
                        tournamentStages.Add(new { Id = ts.Id, TournamentStructure = ts.TournamentStructure, Matches = matches });
                    }
                }

                object obj = new { Id = dt.Id, TournamentStructure = dt.TournamentStructure, Division = dt.Division, TournamentStages = tournamentStages };

                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Could not find division tournament", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        // GET: DivisionTournament/Create
        public ActionResult Create(int divisionID)
        {
            try
            {
                Division d = db.DivisionSet.Find(divisionID);
                TournamentStructure ts = d.TournamentStructure.Value;
                DivisionTournament dt = db.DivisionTournamentSet.Add(new DivisionTournament() { TournamentStructure = ts, Division = d });
                db.SaveChanges();
                return Json(new { status = "success", message = "New division tournament added", id = dt.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "New division tournament not added", details = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        // GET: DivisionTournament/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                DivisionTournament dt = db.DivisionTournamentSet.Find(id);
                TournamentStageController tsc = new TournamentStageController();
                foreach (TournamentStage ts in dt.TournamentStage)
                {
                    tsc.Delete(ts.Id);
                }
                db.DivisionTournamentSet.Remove(dt);
                db.SaveChanges();
                return Json(new { status = "success", message = "Division tournament deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Division tournament not deleted", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
