using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CupPlaner.Helpers;

namespace CupPlaner.Controllers
{
    public class ScheduleManagerController : Controller
    {

        ScheduleManager sm = new ScheduleManager();
        CupDBContainer db = new CupDBContainer();

        public ActionResult DeleteSchedule (int tournamentID)
        {
            //try
            //{
                sm.DeleteSchedule(tournamentID);
                return Json(new { status = "success"}, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception)
            //{
            //    return Json(new { status = "error" }, JsonRequestBehavior.AllowGet);
            //}
        }

        public ActionResult MinNumOfFields (int tournamentID, FieldSize fs)
        {
            try
            {
                Tournament t = db.TournamentSet.Find(tournamentID);
                int numOfAllFields = t.Fields.Count(x => x.Size == fs);
                int numOfMinFields = sm.MinNumOfFields(tournamentID, fs);
                return Json(new { status = "success", FieldsCount = numOfMinFields, AllFieldsCount = numOfAllFields }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return Json(new { status = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Schedule(int tournamentID, FieldSize fs)
        {
            //try
            //{
                Tournament t = db.TournamentSet.Find(tournamentID);
                int numOfFields = t.Fields.Count(x => x.Size == fs);
                if (numOfFields > 0 && t.Divisions.Any(x => x.FieldSize == fs))
                {
                    int minNumOfFields = sm.MinNumOfFields(tournamentID, fs);
                    for (int i = minNumOfFields; i <= numOfFields; i++)
                    {
                        if (sm.scheduleAll(tournamentID, fs, i))
                        {
                            return Json(new { status = "success"}, JsonRequestBehavior.AllowGet);
                        }
                    }
                }else
                {
                    return Json(new { status = "success"}, JsonRequestBehavior.AllowGet);
                }                
                return Json(new { status = "error", errorCode = 1, message = "algoritmen fandt ingen løsning" }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception)
            //{
            //    return Json(new { status = "error", errorCode = 2, message = "fejl i programmet" }, JsonRequestBehavior.AllowGet);
            //}
        }
    }
}