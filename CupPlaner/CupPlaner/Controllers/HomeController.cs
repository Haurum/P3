using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

namespace CupPlaner.Controllers
{
    public class HomeController : Controller
    {
        CupDBContainer db = new CupDBContainer();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Hejhej(string name)
        {
            db.TournamentSet.Add(new Tournament() { Name = name });
            db.SaveChanges();

            return Json(new { LOL = "hej" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHejs()
        {
            List<Tournament> ts = db.TournamentSet.ToList();

            return Json(new { Name = ts.First().Name }, JsonRequestBehavior.AllowGet);
        }

        /*[HttpPost]
        public ActionResult ExcelTest(HttpPostedFileBase file = null)
        {
            List<string> divisions = new List<string>();
            List<string> pools = new List<string>();
            List<string> teams = new List<string>();

            int poolStart = 2;
            if (file == null)
            {
                file = Request.Files[0];
            }
            if (file != null && file.ContentLength > 0) 
            {
                string charRange = "CDEFGHIJKLMNOPQRSTY";
                Tournament t = new Tournament();
                Division d = new Division();
                Pool p = new Pool();

                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/App_Data/Excel"), fileName);

                // UNCOMMENT TO SAVE FILE
                //file.SaveAs(path);
                var excel = new Excel.Application();
                excel.Workbooks.Open(path);
                Excel.Worksheet sheet = excel.Sheets["Cup"] as Excel.Worksheet;
                Excel.Range range = sheet.get_Range("A1", Missing.Value);

                t.Name = range.Value;
                t.Password = "testtest";

                for (int i = 2; i < 100; i++)
                {
                    range = sheet.get_Range("A" + i.ToString(), Missing.Value);
                    if (range.Value != null)
                    {
                        d = new Division() { Tournament = t, Name = range.Value, FieldSize = FieldSize.ElevenMan, MatchDuration = 60 };

                        db.DivisionSet.Add(d);

                        if (t.Divisions.Count > 1)
                        {
                            for (int j = poolStart; j < i; j++)
                            {
                                p = new Pool() { Division = d, Name = range.Value };

                                db.PoolSet.Add(p);

                                range = sheet.get_Range("B" + j.ToString(), Missing.Value);

                                foreach (char c in charRange)
                                {
                                    var teamsRange = sheet.get_Range(c + j.ToString(), Missing.Value);

                                    if (!string.IsNullOrEmpty(teamsRange.Value))
                                    {
                                        Team newTeam = new Team() { Name = teamsRange.Value, Pool = p };

                                        db.TeamSet.Add(newTeam);
                                    }
                                    else
                                        break;
                                }
                            }
                            poolStart = i;
                        }
                    }     
                }
            }

            db.SaveChanges();

            return Json(new { State = "Success" }, JsonRequestBehavior.AllowGet);
            
        }*/
    }
}