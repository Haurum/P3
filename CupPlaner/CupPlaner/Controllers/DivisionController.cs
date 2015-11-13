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
        // Sets the "FieldSize" and "MatchDuration" to the input given in the web input.
        // Adds the Division object to the database DivisionSet, and saves the changes in the database.
        // Returns a Json object with a state, indicating whether it succeeded creating the Division object or not.
        [HttpPost]
        public ActionResult Create(string name, int tournamentId, int MatchDuration, FieldSize FieldSize)
        {
            try
            {
                Tournament t = db.TournamentSet.Find(tournamentId);
                Division d = db.DivisionSet.Add(new Division() { Name = name, FieldSize = FieldSize, MatchDuration = MatchDuration, Tournament = t });
                //db.DivisionSet.Add(new Division() { Name = name, FieldSize = FieldSize, MatchDuration = MatchDuration, Tournament = t });
                db.SaveChanges();

                return Json(new { status = "success", message = "New division added", id = d.Id}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "New division not added", details = ex.Message }, JsonRequestBehavior.AllowGet);
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

                return Json(new { status = "success", message = "Division edited" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Division not edited", details = ex.Message }, JsonRequestBehavior.AllowGet);
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
                return Json(new { status = "success", message = "Division deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Division not deleted", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
