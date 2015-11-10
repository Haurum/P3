using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CupPlaner.Controllers
{
    public class DivisionController : Controller
    {
        CupDBContainer db = new CupDBContainer();
        // GET: Division
        public ActionResult Index()
        {
            return View();
        }

        // GET: Division/Details/5
        public ActionResult Details(int id)
        {
            Division d = db.DivisionSet.Find(id);
            List<object> pools = new List<object>();
            if (d.Pools != null)
            {
                foreach (Pool p in d.Pools)
                {
                    pools.Add(new { Id = p.Id, Name = p.Name });
                }
            }

            object obj = new { Id = d.Id, Name = d.Name, Pools = pools, FieldSize = d.FieldSize, MatchDuration = d.MatchDuration };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        // POST: Division/Create
        [HttpPost]
        public ActionResult Create(string name, int tournamentId)
        {
            try
            {
                FieldSize defaultFieldSize = FieldSize.ElevenMan;
                int defaultMatchDuration = 60;
                Tournament t = db.TournamentSet.Find(tournamentId);
                db.DivisionSet.Add(new Division() { Name = name, FieldSize = defaultFieldSize, MatchDuration = defaultMatchDuration, Tournament = t });
                db.SaveChanges();

                return Json(new { state = "new division added" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: new division not added" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Division/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string name, int tournamentId, int fieldSizeInt, int matchDuration)
        {
            try
            {

                Division d = db.DivisionSet.Find(id);
                d.Name = name;
                d.Tournament = db.TournamentSet.Find(tournamentId);
                d.FieldSize = (FieldSize)fieldSizeInt;
                d.MatchDuration = matchDuration;

                db.Entry(d).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { state = "Division edited" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: Division not edited" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Division/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Division d = db.DivisionSet.Find(id);
                PoolController pc = new PoolController();
                foreach (Pool p in d.Pools)
                {
                    pc.Delete(p.Id);
                }
                db.DivisionSet.Remove(d);
                db.SaveChanges();
                return Json(new { state = "division deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: division not deleted" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
