using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CupPlaner.Controllers
{
    public class FinalsLinkController : Controller
    {
        CupDBContainer db = new CupDBContainer();
        // GET: FinalsLink
        public ActionResult Index()
        {
            return View();
        }

        // GET: FinalsLink/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                FinalsLink fl = db.FinalsLinkSet.Find(id);
                object obj = new { status = "success", id = fl.Id, Finalstage = fl.Finalstage, PoolPlacement = fl.PoolPlacement, Division_Id = fl.Division };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Could not find finals link", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: FinalsLink/Create
        [HttpPost]
        public ActionResult Create(int finalStage, int poolPlacement, int divisionID)
        {
            try
            {
                Division d = db.DivisionSet.Find(divisionID);
                db.FinalsLinkSet.Add(new FinalsLink { Finalstage = finalStage, PoolPlacement = poolPlacement, Division = d });
                db.SaveChanges();

                return Json(new { status = "success", message = "New finals link added", id = d.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "New finals link not added", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: FinalsLink/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, int finalStage, int poolPlacement, int divisionID)
        {
            try
            {
                FinalsLink fl = db.FinalsLinkSet.Find(id);
                Division d = db.DivisionSet.Find(divisionID);
                fl.Finalstage = finalStage;
                fl.PoolPlacement = poolPlacement;
                fl.Division = d;

                db.Entry(fl).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { status = "success", message = "Finals link edited" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Finals link not edited", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: FinalsLink/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                FinalsLink fl = db.FinalsLinkSet.Find(id);
                db.FinalsLinkSet.Remove(fl);
                db.SaveChanges();

                return Json(new { status = "success", message = "Finals link deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Finals link not deleted", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
