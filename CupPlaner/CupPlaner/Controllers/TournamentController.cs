using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

namespace CupPlaner.Controllers
{
    public class TournamentController : Controller
    {
        CupDBContainer db = new CupDBContainer();

        // GET: Tournament/Details/5
        // Define a list with divisions, fields and times.
        // Going through all the divisiosns, add some divisions with name and id.
        // Return a JSON object if there is a tournamet with a password, id, name, fields and divisions.
        // If there doesnt exist a tournament it will return a JSON object and get a error.
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
        // This function will create a password as a string.
        // Find a password to the tournament in the databse. 
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
        // This function will have name, password starttime and endtime.
        // 

        [HttpPost]
        public ActionResult Create(string name, string password, string startTimes, string endTimes)
        {
            try
            {
                if (!db.TournamentSet.Any(x => x.Password == password))
                {
                    Tournament t = new Tournament();
                    List<Team> teams2 = new List<Team>();
                    List<TimeInterval> tis = new List<TimeInterval>();

                    HttpPostedFileBase file = null;
                    int poolStart = 2;
                    
                    List<DateTime> startTimesList = startTimes.Split(',').Select(DateTime.Parse).ToList();
                    List<DateTime> endTimesList = endTimes.Split(',').Select(DateTime.Parse).ToList();
                    for (int i = 0; i < startTimesList.Count; i++)
                    {
                        tis.Add(new TimeInterval() { StartTime = startTimesList[i], EndTime = endTimesList[i] });
                        db.TimeIntervalSet.Add(new TimeInterval() { StartTime = startTimesList[i], EndTime = endTimesList[i] });
                    }

                    if (Request != null && Request.Files.Count > 0 && Request.Files[0] != null && Request.Files[0].ContentLength > 0)
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
                                        
                                        foreach (char c in charRange.ToList())
                                        {
                                            var teamsRange = sheet.get_Range(c + j.ToString(), Missing.Value);
                                            if (!string.IsNullOrEmpty(teamsRange.Value))
                                            {
                                                Team newTeam = new Team() { Name = teamsRange.Value, Pool = p, IsAuto = false,  };
                                                foreach (TimeInterval ti in tis)
                                                {
                                                    newTeam.TimeIntervals.Add(db.TimeIntervalSet.Add(new TimeInterval { StartTime = ti.StartTime, EndTime = ti.EndTime }));
                                                }
                                                p.Teams.Add(newTeam);
                                                newTeam = db.TeamSet.Add(newTeam);
                                            }
                                            else
                                                break;
                                        }
                                        p = db.PoolSet.Add(p);
                                    }
                                }

                                if (range2.Value == null)
                                    break;

                                d = new Division() { Tournament = t, Name = range.Value, FieldSize = FieldSize.ElevenMan, MatchDuration = 60 };
                                d = db.DivisionSet.Add(d);
                                poolStart = i;
                            }  
                        }
                    }

                    foreach (Team teamitem in teams2)
                    {
                        db.TeamSet.Add(teamitem);
                    }
                    t.Name = name;
                    t.Password = password;
                    t.TimeIntervals = tis;
                    t = db.TournamentSet.Add(t);
                    db.SaveChanges();
 
                    return Json(new { status = "success", message = "New tournament added", id = t.Id }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = "error", message = "Password already exists" }, JsonRequestBehavior.AllowGet);          
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "New tournament not added", details = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Tournament/Edit/5
        // Find a id to the tournament in the database.
        // Generate a list of all Timeinterval.
        // Going through all the divisions, pools and teams
        // Find a id to team in the database
        // Try try remove the first and the last element in timeinterval in the database, and going through all timeintervals.

        [HttpPost]
        public ActionResult Edit(int id, string name, string password, List<DateTime> startTimes, List<DateTime> endTimes)
        {
            try
            {
                Tournament t = db.TournamentSet.Find(id);
                db.TimeIntervalSet.RemoveRange(t.TimeIntervals);
                List<TimeInterval> tis = new List<TimeInterval>();
                TimeInterval timeinterval = new TimeInterval();
                for (int i = 0; i < startTimes.Count; i++)
                {
                    tis.Add(new TimeInterval() { StartTime = startTimes[i], EndTime = endTimes[i] });
                }
                t.TimeIntervals = tis;
                foreach (Division d in t.Divisions)
                {
                    foreach (Pool p in d.Pools)
                    {
                        foreach (Team tm in p.Teams)
                        {
                            Team team = db.TeamSet.Find(tm.Id);
                            db.TimeIntervalSet.RemoveRange(team.TimeIntervals);
                            foreach (TimeInterval ti in t.TimeIntervals)
                            {
                                timeinterval = new TimeInterval() { Team = team, StartTime = ti.StartTime, EndTime = ti.EndTime };
                                db.TimeIntervalSet.Add(timeinterval);
                                team.TimeIntervals.Add(timeinterval);
                            }
                        }
                    }
                }
                t.Name = name;
                t.Password = password;

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
        // Find a id to a tournament via the database
        // Going through all the division. 
        // Return a JSON object if the tournament is deleted with succes, and error when is not deleted.
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
