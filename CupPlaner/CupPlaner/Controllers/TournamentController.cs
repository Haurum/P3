using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace CupPlaner.Controllers
{
    public class TournamentController : Controller
    {
        CupDBContainer db = new CupDBContainer();
        // GET: Tournament
        public ActionResult Index()
        {
            return View();
        }

        // GET: Tournament/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                Tournament t = db.TournamentSet.Find(id);
                List<object> divs = new List<object>();
                List<object> fields = new List<object>();
                List<object> times = new List<object>();
                if (t.Divisions != null)
                {
                    foreach (Division d in t.Divisions)
                    {
                        divs.Add(new { Id = d.Id, Name = d.Name });
                    }
                }
                if (t.TimeIntervals != null)
                {
                    foreach (TimeInterval ti in t.TimeIntervals)
                    {
                        times.Add(new { Id = ti.Id, StartTime = ti.StartTime, EndTime = ti.EndTime });
                    }
                }

                if(t.Fields != null)
                {
                    foreach (Field f in t.Fields)
                    {
                        fields.Add(new { Id = f.Id, Name = f.Name, fieldSize = f.Size });
                    }
                }
                object obj = new { status = "success", Id = t.Id, Name = t.Name, Password = t.Password, Divisions = divs, TimeIntervals = times, Fields = fields };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { status = "error", message = "Could not find tournament", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult IdFromPass(string password)
        {
            Tournament t = db.TournamentSet.SingleOrDefault(x => x.Password == password);
            if (t == null)
            {
                return Json(new { Id = 0 });
            }
            return Json(new { Id = t.Id });
        }

        // POST: Tournament/Create
        [HttpPost]
        public ActionResult Create(string name, string password, List<DateTime> startTimes, List<DateTime> endTimes)
        {
            //try
            //{
                if (!db.TournamentSet.Any(x => x.Password == password))
                {
                    Tournament t = new Tournament();
                    
                    HttpPostedFileBase file = null;

                    int poolStart = 2;
                             
                    if (Request != null && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                    {
                        file = Request.Files[0];
                        string charRange = "CDEFGHIJKLMNOPQRSTY";
                        List<string> divisions = new List<string>();
                        List<string> pools = new List<string>();
                        List<string> teams = new List<string>();

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
                        Excel.Range range2 = sheet.get_Range("A2", Missing.Value);
                        Excel.Range poolsRange;

                        t.Name = range.Value;

                        for (int i = 2; i < 10000; i++)
                        {
                            range = sheet.get_Range("A" + i.ToString(), Missing.Value);
                            range2 = sheet.get_Range("B" + i.ToString(), Missing.Value);
                            if (range.Value != null || range2.Value == null)
                            {
                                if (t.Divisions.Count > 0)
                                {
                                    for (int j = poolStart; j < i; j++)
                                    {
                                        poolsRange = sheet.get_Range("B" + j.ToString(), Missing.Value);
                                        p = new Pool() { Division = d, Name = poolsRange.Value, IsAuto = false };
                                        db.PoolSet.Add(p);

                                        foreach (char c in charRange)
                                        {
                                            var teamsRange = sheet.get_Range(c + j.ToString(), Missing.Value);
                                            if (!string.IsNullOrEmpty(teamsRange.Value))
                                            {
                                                Team newTeam = new Team() { Name = teamsRange.Value, Pool = p, IsAuto = false };
                                                db.TeamSet.Add(newTeam);
                                            }
                                            else
                                                break;
                                        }
                                    }
                                }

                                if (range2.Value == null)
                                    break;

                                d = new Division() { Tournament = t, Name = range.Value, FieldSize = FieldSize.ElevenMan, MatchDuration = 60 };
                                db.DivisionSet.Add(d);
                                poolStart = i;
                            }
                            
                        }
                    }
                    
                    List<TimeInterval> tis = new List<TimeInterval>();
                    for (int i = 0; i < startTimes.Count; i++)
                    {
                        tis.Add(new TimeInterval() { StartTime = startTimes[i], EndTime = endTimes[i] });
                    }

                    t.Name = name;
                    t.Password = password;
                    t.TimeIntervals = tis;
                    t = db.TournamentSet.Add(t);
                    db.SaveChanges();

                    return Json(new { status = "success", message = "New tournament added", id = t.Id }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = "error", message = "Password already exists" }, JsonRequestBehavior.AllowGet);          
            /*}
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "New tournament not added", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }*/
        }

        // POST: Tournament/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string name, string password, List<DateTime> startTimes, List<DateTime> endTimes)
        {
            try
            {

                Tournament t = db.TournamentSet.Find(id);
                db.TimeIntervalSet.RemoveRange(t.TimeIntervals);
                List<TimeInterval> tis = new List<TimeInterval>();
                for (int i = 0; i < startTimes.Count; i++)
                {
                    tis.Add(new TimeInterval() { StartTime = startTimes[i], EndTime = endTimes[i] });
                }
                t.Name = name;
                t.Password = password;
                t.TimeIntervals = tis;

                db.Entry(t).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { status = "success", message = "Tournament edited" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Tournament not edited", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        // POST: Tournament/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Tournament t = db.TournamentSet.Find(id);
                DivisionController dc = new DivisionController();
                foreach (Division d in t.Divisions)
                {
                    dc.Delete(d.Id);
                }
                db.TimeIntervalSet.RemoveRange(t.TimeIntervals);
                db.TournamentSet.Remove(t);
                db.SaveChanges();
                return Json(new { status = "success", message = "Tournament deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Tournament not deleted", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
