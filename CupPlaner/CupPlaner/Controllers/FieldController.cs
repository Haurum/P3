using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CupPlaner.Controllers
{
    public class FieldController : Controller
    {
        // Database container, has functionalities to connect to the database classes.
        CupDBContainer db = new CupDBContainer();

        // GET: Field/Details/5 - Fetches the details of the class, takes the "id" parameter to determine the corresponding Field object.
        // Returns a Json object, which contains a copy of the corresponding Field variables.
        public ActionResult Details(int id)
        {
            Field f = db.FieldSet.Find(id);
            object obj = new { Id = f.Id, name = f.Name, size = f.Size };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        // POST: Field/Create - Tries to create a Field object, with the parameters "name" and "size".
        // Sets the FieldSize and Name to the parameters ("name" and "size").
        // Adds the Field object to the database FieldSet, and saves the changes in the database.
        // Returns a Json object with a state, indicating whether it succeeded creating the Field object or not.
        [HttpPost]
        public ActionResult Create(string name, int size)
        {
            try
            {
                // TODO: Add insert logic here
                db.FieldSet.Add(new Field() { Name = name, Size = (FieldSize)size });
                db.SaveChanges();
                return Json(new { state = "new field added" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: new field not added" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Field/Edit/5 - Tries to edit a Field, determined by the "id" parameter.
        // Edits a Fields name and FieldSize. Saves the changes to the database, if succeeded.
        // Returns a Json object with a state, indicating whether it succeeded editing the Field object or not.
        [HttpPost]
        public ActionResult Edit(int id, string name, int size)
        {
            try
            {
                Field f = db.FieldSet.Find(id);
                if (name != null)
                {
                    f.Name = name;
                }
                if ((FieldSize)size != f.Size)
                {
                    f.Size = (FieldSize)size;
                }

                db.Entry(f).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { state = "field edited" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: field not edited" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Field/Delete/5 - Tries to delete a Field object, determined by the "id".
        // Deletes the Field object from the FieldSet in the database, and saves the changes, if succeeded.
        // Returns a Json object, indicating whether it succeeded deleting the Field or not.
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Field f = db.FieldSet.Find(id);
                db.FieldSet.Remove(f);
                db.SaveChanges();

                return Json(new { state = "field deleted" });
            }
            catch
            {
                return Json(new { state = "ERROR: field not deleted" });
            }
        }
    }
}
