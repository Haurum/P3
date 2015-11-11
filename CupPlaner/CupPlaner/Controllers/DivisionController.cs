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
        // Database container, has functionalities to connect to the database classes.
        CupDBContainer db = new CupDBContainer();

        // GET: Division/Details/5 - Fetches the details of the class, takes the "id" parameter to determine the corresponding Divison object.
        // Returns a Json object, which contains a copy of the corresponding Divisions variables.
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

        // POST: Division/Create - Tries to create a Division object, with the parameters "name" and "tournamentId".
        // Sets the "FieldSize" and "MatchDuration" to defaults values (FieldSize.ElevenMan, MatchDuration =60).
        // Adds the Division object to the database DivisionSet, and saves the changes in the database.
        // Returns a Json object with a state, indicating whether it succeeded creating the Division object or not.
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

        // POST: Division/Edit/5 - Tries to edit a Division, determined by the "id" parameter and "tournamentId".
        // Edits a divisions name, FieldSize and matchDuration. Saves the changes to the database, if succeeded.
        // Returns a Json object with a state, indicating whether it succeeded editing the Division object or not.
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

        // POST: Division/Delete/5 - Tries to delete a Division object, determined by the "id".
        // Deletes both the Division object, and all Pool objects contained in the Division, and saves to the database, if succeeded.
        // Returns a Json object, indicating whether it succeeded deleting the Division object and pools, or not.
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
