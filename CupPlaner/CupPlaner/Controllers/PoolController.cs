using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CupPlaner.Controllers
{
    public class PoolController : Controller
    {
        CupDBContainer db = new CupDBContainer();
        // GET: Pool
        public ActionResult Index()
        {
            return View();
        }

        // GET: Pool/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                Pool p = db.PoolSet.Find(id);
                List<object> teams = new List<object>();
                List<object> ffs = new List<object>();
                List<object> matches = new List<object>();
                if (p.Teams != null)
                {
                    foreach (Team t in p.Teams)
                    {
                        teams.Add(new { Id = t.Id, Name = t.Name });
                    }
                }
                if (p.FavoriteFields != null)
                {
                    foreach (Field f in p.FavoriteFields)
                    {
                        ffs.Add(new { Id = f.Id, Name = f.Name });
                    }
                }
                if (p.TournamentStage != null && p.TournamentStage.Matches > 0)
                {
                    foreach (Match m in p.TournamentStage.Matches)
                    {
                        matches.Add(new { Id = m.Id, Team1 = m.Teams.First(), Team2 = m.Teams.Last() });
                    }
                }
                object obj = new { status = "success", Id = p.Id, Name = p.Name, FieldSize = p.Division.FieldSize, DivisionName = p.Division.Name, Teams = teams, FavoriteFields = ffs, Matches = matches };
                


                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Could not find pool", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
      
        }

        // POST: Pool/
        [HttpPost]
        public ActionResult Create(string name, int divisionId)
        {
            try
            {
                Division d = db.DivisionSet.Find(divisionId);
                Pool p = db.PoolSet.Add(new Pool() { Name = name, Division = d });
                db.SaveChanges();

                return Json(new { status = "success", message = "New pool added", id = p.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "New pool not added", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // POST: Pool/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string name, int divisionId, List<int> fieldIds)
        {
            try
            {
                Pool p = db.PoolSet.Find(id);
                p.Name = name;
                p.Division = db.DivisionSet.Find(divisionId);
                if (fieldIds.Count > 0)
                {
                    p.FavoriteFields.Clear();
                    foreach (int fieldId in fieldIds)
                    {
                        p.FavoriteFields.Add(db.FieldSet.Find(fieldId));
                    }
                }
                

                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { status = "success", message = "Pool edited" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Pool not edited", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Pool/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Pool p = db.PoolSet.Find(id);
                TeamController tc = new TeamController();
                foreach (Team team in p.Teams)
                {
                    tc.Delete(team.Id);
                }
                db.PoolSet.Remove(p);
                db.SaveChanges();

                return Json(new { status = "success", message = "Pool deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Pool not deleted", details = ex.Message });
            }
        }
    }
}
