﻿using System;
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

        public ActionResult GetAllTournamentFields(int tournamentId)
        {
            try
            {
                Tournament t = db.TournamentSet.Find(tournamentId);
                List<object> fields = new List<object>();

                if(t.Fields != null)
                {
                    foreach(Field f in t.Fields)
                    {
                        fields.Add(new { Id = f.Id, Name = f.Name, fieldSize = f.Size });
                    }
                }
                object obj = new { status = "success", Fields = fields };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { status = "error", message = "could not find fields", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Field/Create - Tries to create a Field object, with the parameters "name" and "size".
        // Sets the FieldSize and Name to the parameters ("name" and "size").
        // Adds the Field object to the database FieldSet, and saves the changes in the database.
        // Returns a Json object with a state, indicating whether it succeeded creating the Field object or not.
        [HttpPost]
        public ActionResult Create(string name, int size, int tournamentId)
        {
            try
            {
                // TODO: Add insert logic here
                Tournament t = db.TournamentSet.Find(tournamentId);
                Field f = db.FieldSet.Add(new Field() { Name = name, Size = (FieldSize)size, Tournament = t });
                db.SaveChanges();
                return Json(new { status = "success", message = "New field added", id = f.Id, fieldName = f.Name }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "New field not added", details = ex.Message }, JsonRequestBehavior.AllowGet);
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

                return Json(new { status = "success", message = "Field edited" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Field not edited", details = ex.Message }, JsonRequestBehavior.AllowGet);
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

                return Json(new { status = "status", message = "Field deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Field not deleted", details = ex.Message });
            }
        }
    }
}
