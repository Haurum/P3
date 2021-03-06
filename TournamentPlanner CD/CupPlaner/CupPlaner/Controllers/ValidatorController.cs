﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CupPlaner.Helpers;

namespace CupPlaner.Controllers
{
    public class ValidatorController : Controller
    {
        Validator v = new Validator();

        // calls the IsScheduleReady function from the Validator class and return a json object to the frontend with the result
        public ActionResult IsScheduleReady(int tournamentID)
        {
            try
            {
                if (v.IsScheduleReady(tournamentID))
                {
                    return Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "error", message = "turnering ikke klar" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { status = "error", message = "fejl i programmet" }, JsonRequestBehavior.AllowGet);
            }
            

        }
    }
}