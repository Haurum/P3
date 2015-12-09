using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CupPlaner.Helpers
{
    // Takes care of the schedule functions: generate and clear
    public class ScheduleManager
    {

        public bool scheduleAll(int tournamentID, FieldSize fSize, int numberOfFields)
        {
            CupDBContainer db = new CupDBContainer();
            MatchGeneration mg = new MatchGeneration();
            Validator validator = new Validator();
            Tournament t = db.TournamentSet.Find(tournamentID);
            List<TournamentStage> TournamentStages = db.TournamentStageSet.Where(x => x.DivisionTournament.Division.Tournament.Id == t.Id && x.DivisionTournament.Division.FieldSize == fSize).ToList();
            List<Match> allMatches = db.MatchSet.Where(x => x.TournamentStage.DivisionTournament.Division.Tournament.Id == t.Id && x.TournamentStage.DivisionTournament.Division.FieldSize == fSize).ToList();

            int selector = 0;
            int dayCount = 1;
            int indicator = 1;
            int prevCount = 0;
            int prevPrevCount = 0;
            int startingPoint = 0;
            bool IsScheduled = false;
            while (!IsScheduled)
            {
                List<TournamentStage> unscheduledTournamentstages = TournamentStages.Where(x => !x.IsScheduled).ToList();

                if (unscheduledTournamentstages.Count == 0)
                {
                    IsScheduled = true;
                    continue;
                }
                if (selector == unscheduledTournamentstages.Count)
                {
                    selector = 0;
                }
                //TournamentStage ts = unscheduledTournamentstages.ElementAt(selector);
                foreach (TournamentStage ts in unscheduledTournamentstages)
                {
                    bool isReady = true;
                    foreach (Team team in ts.Pool.Teams)
                    {
                        if (team.PrevPool == null)
                        {
                            continue;
                        }
                        else if (team.PrevPool.TournamentStage.IsScheduled)
                        {
                            if (ts.TimeInterval.StartTime < team.PrevPool.TournamentStage.TimeInterval.EndTime)
                            {
                                ts.TimeInterval.StartTime = team.PrevPool.TournamentStage.TimeInterval.EndTime;
                            }
                        }
                        else
                        {
                            isReady = false;
                            break;
                        }
                    }
                    if (!isReady)
                    {
                        selector++;
                        continue;
                    }


                    List<Match> unscheduledMatches = ts.Matches.Where(x => !x.IsScheduled).ToList();
                    Match matchToSchedule;
                    if (unscheduledMatches.Count == 0)
                    {
                        DateTime lastMatchStart = ts.Matches.Max(x => x.StartTime);
                        ts.TimeInterval.EndTime = lastMatchStart.AddMinutes(ts.DivisionTournament.Division.MatchDuration * 2);
                        ts.IsScheduled = true;
                        continue;
                    }
                    else if (indicator > 0)
                    {
                        matchToSchedule = unscheduledMatches.First();

                    }
                    else
                    {
                        matchToSchedule = unscheduledMatches.Last();
                    }

                    List<Field> fields = matchToSchedule.TournamentStage.DivisionTournament.Division.Tournament.Fields.Where(x => x.Size == fSize).Take(numberOfFields).ToList();
                    List<Field> fieldsNotChecked = new List<Field>();
                    fieldsNotChecked.AddRange(fields);

                    for (int i = 0; i < dayCount; i++)
                    {
                        
                        foreach (Field field in matchToSchedule.TournamentStage.Pool.FavoriteFields)
                        {
                            if (validator.areTeamsFree(matchToSchedule, field.NextFreeTime.ElementAt(i).FreeTime))
                            {
                                matchToSchedule.StartTime = field.NextFreeTime.ElementAt(i).FreeTime;
                                matchToSchedule.Field = field;
                                field.NextFreeTime.ElementAt(i).FreeTime = field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(matchToSchedule.Duration);
                                matchToSchedule.IsScheduled = true;
                                db.Entry(field).State = System.Data.Entity.EntityState.Modified;
                                break;
                            }
                            fieldsNotChecked.Remove(field);

                        }

                        if (matchToSchedule.IsScheduled || fieldsNotChecked.Count == 0)
                        {
                            break;
                        }

                        fieldsNotChecked = fieldsNotChecked.OrderBy(x => x.Matches.Count(y => y.StartTime.Date == x.NextFreeTime.ElementAt(i).FreeTime.Date)).ToList();
                        foreach (Field field in fieldsNotChecked)
                        {
                            if (validator.areTeamsFree(matchToSchedule, field.NextFreeTime.ElementAt(i).FreeTime))
                            {
                                matchToSchedule.StartTime = field.NextFreeTime.ElementAt(i).FreeTime;
                                matchToSchedule.Field = field;
                                field.NextFreeTime.ElementAt(i).FreeTime = field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(matchToSchedule.Duration);
                                matchToSchedule.IsScheduled = true;
                                matchToSchedule.TournamentStage.Pool.FavoriteFields.Add(field);
                                db.Entry(field).State = System.Data.Entity.EntityState.Modified;
                                break;
                            }
                            
                        }

                        if (matchToSchedule.IsScheduled)
                        {
                            startingPoint++;
                            break;
                        }


                    }
                    if (matchToSchedule.IsScheduled)
                    {
                        selector = 0;
                        db.Entry(matchToSchedule).State = System.Data.Entity.EntityState.Modified;
                    }

                }
                List<Match> allUnscheduledMatches = allMatches.Where(x => !x.IsScheduled).ToList();
                if (allUnscheduledMatches.Count != 0 && allUnscheduledMatches.Count == prevCount && prevCount == prevPrevCount)
                {
                        int newDayCount = 1;
                        int k = 0;
                        bool done = false;
                        List<Field> fields = allUnscheduledMatches.First().TournamentStage.DivisionTournament.Division.Tournament.Fields.Where(x => x.Size == fSize).Take(numberOfFields).ToList();
                        while (!done)
                        {
                            k += 10;
                            if (fields.All(x => x.NextFreeTime.ElementAt(newDayCount-1).FreeTime.AddMinutes(k) > t.TimeIntervals.ElementAt(newDayCount-1).EndTime))
                            {
                                if (newDayCount < dayCount)
                                {
                                    newDayCount++;
                                    k = 0;
                                    continue;
                                }
                                else if (dayCount < t.TimeIntervals.Count())
                                {
                                    dayCount++;
                                    done = true;
                                    continue;
                                }
                                else
                                {
                                    /*done = true;
                                    IsScheduled = true;
                                    continue;*/
                                    return false;
                                }
                            }
                            foreach (Match match in allUnscheduledMatches.Where(x => x.TournamentStage.TimeInterval.StartTime != DateTime.MinValue))
                            {
                                
                                List<Field> fieldsNotChecked = new List<Field>();
                                fieldsNotChecked.AddRange(fields);
                                fieldsNotChecked = fieldsNotChecked.OrderBy(x => x.Matches.Count(y => y.StartTime.Date == x.NextFreeTime.ElementAt(newDayCount-1).FreeTime.Date)).ToList();
                                foreach (Field field in fieldsNotChecked)
                                {
                                    if (field.NextFreeTime.ElementAt(newDayCount-1).FreeTime.AddMinutes(k) >= t.TimeIntervals.ElementAt(newDayCount-1).EndTime)
                                    {
                                        break;
                                    }
                                    if (validator.areTeamsFree(match, field.NextFreeTime.ElementAt(newDayCount-1).FreeTime.AddMinutes(k)))
                                    {
                                        field.NextFreeTime.ElementAt(newDayCount-1).FreeTime = field.NextFreeTime.ElementAt(newDayCount-1).FreeTime.AddMinutes(k);
                                        match.StartTime = field.NextFreeTime.ElementAt(newDayCount-1).FreeTime;
                                        match.Field = field;
                                        field.NextFreeTime.ElementAt(newDayCount-1).FreeTime = field.NextFreeTime.ElementAt(newDayCount-1).FreeTime.AddMinutes(match.Duration);
                                        match.IsScheduled = true;
                                        db.Entry(field).State = System.Data.Entity.EntityState.Modified;
                                        break;
                                    }
                                }
                                if (match.IsScheduled)
                                {
                                    selector = 0;
                                    done = true;
                                    startingPoint++;
                                    break;
                                }
                            }                           
                        }
                    }                                                          
                
                else
                {
                    prevPrevCount = prevCount;
                    prevCount = allUnscheduledMatches.Count;
                }
                indicator *= -1;
            }
            db.SaveChanges();
            return true;
        }

        public int MinNumOfFields(int tournamentID, FieldSize fs)
        {
            CupDBContainer db = new CupDBContainer();
            Tournament t = db.TournamentSet.Find(tournamentID);
            int duration = 0;
            foreach (Division d in t.Divisions)
            {
                if (d.FieldSize == fs)
                {
                    foreach (TournamentStage ts in d.DivisionTournament.TournamentStage)
                    {
                        duration += (d.MatchDuration * ts.Matches.Count());
                    }
                }
                
            }
            double tDuration = 0;
            foreach (TimeInterval ti in t.TimeIntervals)
            {
                tDuration += (ti.EndTime - ti.StartTime).TotalMinutes;
            }
            return (int)Math.Ceiling((duration / tDuration));
        }

        // Deletes the whole schedule for a tournament
        public void DeleteSchedule(int tournamentID)
        {
            CupDBContainer db = new CupDBContainer();
            DeleteSchedule(tournamentID, db);
        }
        public void DeleteSchedule(int tournamentID, CupDBContainer db)
        {
            //CupDBContainer db = new CupDBContainer();
            MatchGeneration mg = new MatchGeneration();
            Tournament t = db.TournamentSet.Find(tournamentID);
            if(db.MatchSet.Any(x => x.TournamentStage.DivisionTournament.Division.Tournament.Id == tournamentID))
            {
                foreach (Division d in t.Divisions.ToList())
                {
                    // Remove all division tournaments and their dependencies
                    if (d.DivisionTournament != null)
                    {
                        foreach (TournamentStage ts in d.DivisionTournament.TournamentStage.ToList())
                        {
                            foreach (Match m in ts.Matches.ToList())
                            {
                                foreach (Team team in m.Teams.ToList())
                                {
                                    team.Matches.Remove(m);
                                }
                            }
                            db.MatchSet.RemoveRange(ts.Matches);
                            db.TimeIntervalSet.Remove(ts.TimeInterval);
                        }
                        db.TournamentStageSet.RemoveRange(d.DivisionTournament.TournamentStage);
                        db.DivisionTournamentSet.Remove(d.DivisionTournament);
                    }
                    // Remeove each pool that is generated automatically by the match generation class and their dependencies
                    foreach (Pool pool in d.Pools.ToList())
                    {
                        pool.FavoriteFields.Clear();
                        if (pool.IsAuto)
                        {
                            foreach (Team team in pool.Teams)
                            {
                                db.TimeIntervalSet.RemoveRange(team.TimeIntervals);
                            }
                            db.TeamSet.RemoveRange(pool.Teams);
                            
                            db.PoolSet.Remove(pool);
                        }
                    }
                }
                // Reset next free time of each field to default (tournament start time) for each day
                TimeInterval[] tournamentTi = t.TimeIntervals.ToArray();
                foreach (Field f in t.Fields)
                {
                    db.NextFreeTimeSet.RemoveRange(f.NextFreeTime);
                    for (int i = 0; i < tournamentTi.Count(); i++)
                    {
                        f.NextFreeTime.Add(new NextFreeTime() { FreeTime = tournamentTi[i].StartTime });
                    }
                }
                t.IsScheduled = false;
                db.Entry(t).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}