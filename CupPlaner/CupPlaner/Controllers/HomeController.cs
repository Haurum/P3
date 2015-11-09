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

        [HttpPost]
        public ActionResult ExcelTest(HttpPostedFileBase file)
        {
            string tournamentName = "";

            List<string> divisions = new List<string>();
            List<string> pools = new List<string>();
            List<string> teams = new List<string>();

            int poolStart = 2;

            if (file != null && file.ContentLength > 0) 
            {
                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/App_Data/Excel"), fileName);
                file.SaveAs(path);
                var excel = new Excel.Application();
                excel.Workbooks.Open(path);
                Excel.Worksheet sheet = excel.Sheets["Cup"] as Excel.Worksheet;
                Excel.Range range = sheet.get_Range("A1", Missing.Value);

                tournamentName = range.Value;
                for (int i = 2; i < 100; i++)
                {
                    range = sheet.get_Range("A" + i.ToString(), Missing.Value);
                    if (range.Value != null)
                    {
                        divisions.Add(range.Value);
                        if (divisions.Count > 1)
                        {
                            for (int j = poolStart; j <= i; j++)
                            {
                                range = sheet.get_Range("B" + j.ToString(), Missing.Value);
                                pools.Add(range.Value);
                            }
                            poolStart = i;
                        }
                    }     
                }
            }

            return Json(new { TournamentName = tournamentName, Divisions = divisions, Pools = pools }, JsonRequestBehavior.AllowGet);
            
        }
    }
}