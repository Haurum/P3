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
        CupDBContainer db = new CupDBContainer();
        // GET: Field
        public ActionResult Index()
        {
            return View();
        }

        // GET: Field/Details/5
        public ActionResult Details(int id)
        {
            Field f = db.FieldSet.Find(id);
            object obj = new { Id = f.Id, name = f.Name, size = f.Size };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        // POST: Field/Create
        [HttpPost]
        public ActionResult Create(string name, FieldSize size)
        {
            try
            {
                // TODO: Add insert logic here
                Field f = db.FieldSet.Add(new Field() { Name = name, Size = size });
                db.SaveChanges();
                return Json(new { state = "new field added" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { state = "ERROR: new field not added" }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Field/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Field f = db.FieldSet.Find(id);
                if (name != null)
                {
                    f.Name = name;
                }
                if (size != f.Size)
                {
                    f.Size = size;
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

        // POST: Field/Delete/5
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
