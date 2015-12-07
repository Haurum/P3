using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CupPlaner.Helpers
{
    // Takes care of the schedule functions: generate and clear
    public class ScheduleManager
    {
        CupDBContainer db = new CupDBContainer();
        MatchGeneration mg = new MatchGeneration();
        Validator validator = new Validator();

        public void scheduleAll(Tournament t, FieldSize fSize)
        {
            List<TournamentStage> TournamentStages = db.TournamentStageSet.Where(x => x.DivisionTournament.Division.Tournament.Id == t.Id && x.DivisionTournament.Division.FieldSize == fSize).ToList();
            List<Match> allMatches = db.MatchSet.Where(x => x.TournamentStage.DivisionTournament.Division.Tournament.Id == t.Id && x.TournamentStage.DivisionTournament.Division.FieldSize == fSize).ToList();

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

                    List<Field> fields = matchToSchedule.TournamentStage.DivisionTournament.Division.Tournament.Fields.Where(x => x.Size == fSize).ToList();
                    List<Field> fieldsNotChecked = new List<Field>();

                    for (int i = 0; i < dayCount; i++)
                    {
                        fieldsNotChecked.AddRange(fields);
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
                        if (startingPoint >= fieldsNotChecked.Count)
                        {
                            startingPoint = 0;
                        }
                        int endingPoint = startingPoint == 0 ? fieldsNotChecked.Count - 1 : startingPoint - 1;
                        for (int j = startingPoint; j != endingPoint; j++)
                        {
                            if (validator.areTeamsFree(matchToSchedule, fieldsNotChecked[j].NextFreeTime.ElementAt(i).FreeTime))
                            {
                                matchToSchedule.StartTime = fieldsNotChecked[j].NextFreeTime.ElementAt(i).FreeTime;
                                matchToSchedule.Field = fieldsNotChecked[j];
                                fieldsNotChecked[j].NextFreeTime.ElementAt(i).FreeTime = fieldsNotChecked[j].NextFreeTime.ElementAt(i).FreeTime.AddMinutes(matchToSchedule.Duration);
                                matchToSchedule.IsScheduled = true;
                                matchToSchedule.TournamentStage.Pool.FavoriteFields.Add(fieldsNotChecked[j]);
                                db.Entry(fieldsNotChecked[j]).State = System.Data.Entity.EntityState.Modified;
                                break;
                            }
                            if (j == fieldsNotChecked.Count-1)
                            {
                                j = -1;
                            }
                        }
                        
                        if(matchToSchedule.IsScheduled)
                        {
                            startingPoint++;
                            break;
                        }
                        
                        
                    }
                    if (matchToSchedule.IsScheduled)
                    {
                        db.Entry(matchToSchedule).State = System.Data.Entity.EntityState.Modified;
                    }

                }
                List<Match> allUnscheduledMatches = allMatches.Where(x => !x.IsScheduled).ToList();
                if (allUnscheduledMatches.Count != 0 && allUnscheduledMatches.Count == prevCount && prevCount == prevPrevCount)
                {
                    if (dayCount < t.TimeIntervals.Count)
                    {
                        dayCount++;
                    }
                    else
                    {
                        Match firstMatch = allUnscheduledMatches.OrderBy(x => x.TournamentStage.TimeInterval.StartTime).First(x => x.TournamentStage.TimeInterval.StartTime != DateTime.MinValue);
                        List<Field> fields = firstMatch.TournamentStage.DivisionTournament.Division.Tournament.Fields.Where(x => x.Size == fSize).ToList();
                        List<Field> fieldsNotChecked = new List<Field>();

                        
                        for (int i = 0; i < dayCount; i++)
                        {
                            fieldsNotChecked.AddRange(fields);
                            //foreach (Field field in firstMatch.TournamentStage.Pool.FavoriteFields)
                            //{
                            //    int k = 10;
                            //    while (!validator.areTeamsFree(firstMatch, field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(k)))
                            //    {
                            //        if (field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(k) >= t.TimeIntervals.ElementAt(i).EndTime)
                            //        {
                            //            break;
                            //        }
                            //        k += 10;
                            //    }
                            //    if (validator.areTeamsFree(firstMatch, field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(k)))
                            //    {
                            //        field.NextFreeTime.ElementAt(i).FreeTime = field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(k);
                            //        firstMatch.StartTime = field.NextFreeTime.ElementAt(i).FreeTime;
                            //        firstMatch.Field = field;
                            //        field.NextFreeTime.ElementAt(i).FreeTime = field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(firstMatch.Duration);
                            //        firstMatch.IsScheduled = true;
                            //        db.Entry(field).State = System.Data.Entity.EntityState.Modified;
                            //        break;
                            //    }
                            //    fieldsNotChecked.Remove(field);
                            //}
                            //if (firstMatch.IsScheduled)
                            //{
                            //    break;
                            //}
                            
                            foreach (Field field in fieldsNotChecked)
                            {
                                int k = 10;
                                while (!validator.areTeamsFree(firstMatch, field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(k)))
                                {
                                    if (field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(k) >= t.TimeIntervals.ElementAt(i).EndTime)
                                    {
                                        break;
                                    }
                                    k += 10;
                                }
                                if (validator.areTeamsFree(firstMatch, field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(k)))
                                {
                                    field.NextFreeTime.ElementAt(i).FreeTime = field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(k);
                                    firstMatch.StartTime = field.NextFreeTime.ElementAt(i).FreeTime;
                                    firstMatch.Field = field;
                                    field.NextFreeTime.ElementAt(i).FreeTime = field.NextFreeTime.ElementAt(i).FreeTime.AddMinutes(firstMatch.Duration);
                                    firstMatch.IsScheduled = true;
                                    db.Entry(field).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                }
                            }
                            if (firstMatch.IsScheduled)
                            {
                                break;
                            }
                        }
                        if (!firstMatch.IsScheduled)
                        {
                            t.IsScheduled = true;
                        }
                        else
                        {
                            db.Entry(firstMatch).State = System.Data.Entity.EntityState.Modified;
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
                
        }


        // Deletes the whole schedule for a tournament
        public void DeleteSchedule(int tournamentID)
        {
            MatchGeneration mg = new MatchGeneration();
            Tournament t = db.TournamentSet.Find(tournamentID);

            foreach (Division d in t.Divisions.ToList())
            {
                // Remove all division tournaments and their dependencies
                if (d.DivisionTournament != null)
                {
                    foreach (TournamentStage ts in d.DivisionTournament.TournamentStage)
                    {
                        foreach (Match m in ts.Matches)
                        {
                            foreach (Team team in m.Teams)
                            {
                                team.Matches.Remove(m);
                            }                           
                        }
                        db.MatchSet.RemoveRange(ts.Matches);
                    }
                    db.TournamentStageSet.RemoveRange(d.DivisionTournament.TournamentStage);
                    db.DivisionTournamentSet.Remove(d.DivisionTournament);
                }
                // Remeove each pool that is generated automatically by the match generation class and their dependencies
                foreach (Pool pool in d.Pools.ToList())
                {
                    if (pool.IsAuto)
                    {
                        foreach (Team team in pool.Teams)
                        {
                            db.TimeIntervalSet.RemoveRange(team.TimeIntervals);
                        }
                        db.TeamSet.RemoveRange(pool.Teams);
                        pool.FavoriteFields.Clear();
                        db.PoolSet.Remove(pool);
                    }
                }
            }
            foreach (TimeInterval ti in t.TimeIntervals)
            {
                foreach (Field f in t.Fields)
                {
                    db.NextFreeTimeSet.RemoveRange(f.NextFreeTime);
                }
            }


            db.SaveChanges();
        }

    }
}