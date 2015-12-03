using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CupPlaner.Helpers
{
    public class ScheduleManager
    {
        // Database container, has functionalities to connect to the database classes. References to other neccessary helper classes
        CupDBContainer db = new CupDBContainer();
        MatchGeneration mg = new MatchGeneration();
        Validator validator = new Validator();

        // Schedules all the generated matches in a tournament and saves the changes directly to the database.
        public void scheduleAll(Tournament t)
        {
            List<TournamentStage> TournamentStages = db.TournamentStageSet.Where(x => x.DivisionTournament.Division.Tournament.Id == t.Id).ToList();

            int indicator = 1;
            while (!t.IsScheduled)
            {
                List<TournamentStage> unscheduledTournamentstages = TournamentStages.Where(x => !x.IsScheduled).ToList();
                if (unscheduledTournamentstages.Count == 0)
                {
                    t.IsScheduled = true;
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
                            if (ts.TimeInterval.StartTime == t.TimeIntervals.First().StartTime)
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
                        ts.TimeInterval.EndTime = lastMatchStart.AddMinutes(ts.DivisionTournament.Division.MatchDuration);
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

                    List<Field> fields = matchToSchedule.TournamentStage.DivisionTournament.Division.Tournament.Fields.Where(x => x.Size == matchToSchedule.TournamentStage.DivisionTournament.Division.FieldSize).ToList();
                    List<Field> fieldsNotChecked = new List<Field>();
                    for (int i = 0; i < fields.First().NextFreeTime.Count; i++)
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
                        if (matchToSchedule.IsScheduled)
                        {
                            break;
                        }
                        foreach (Field field in fieldsNotChecked)
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
                        }
                        if (matchToSchedule.IsScheduled)
                        {
                            break;
                        }
                    }
                    if (!matchToSchedule.IsScheduled && unscheduledTournamentstages.Count == 1)
                    {
                        bool done = false;
                        foreach (Field field in fields)
                        {
                            foreach (NextFreeTime nft in field.NextFreeTime)
                            {
                                if (nft.FreeTime.AddMinutes(matchToSchedule.Duration) < t.TimeIntervals.First(x => x.EndTime.Date == nft.FreeTime.Date).EndTime)
                                {
                                    nft.FreeTime = nft.FreeTime.AddMinutes(matchToSchedule.Duration);
                                    done = true;
                                    break;
                                }
                            }
                            if (done)
                            {
                                break;
                            }
                        }
                    }

                    db.Entry(matchToSchedule).State = System.Data.Entity.EntityState.Modified;
                    
                }

                indicator *= -1;
            }
            db.SaveChanges();
                
        }

        // Schedules a match and returns the time and field a match should be played
        public Tuple<DateTime, Field> scheduleMatch(Match m)
        {
            Tuple<DateTime, Field> results;
            List<Field> fields = m.TournamentStage.DivisionTournament.Division.Tournament.Fields.Where(x => x.Size == m.TournamentStage.DivisionTournament.Division.FieldSize).ToList();
            List<Field> fieldsNotChecked = new List<Field>();
            for (int i = 0; i < fields.First().NextFreeTime.Count ; i++)
            {
                fieldsNotChecked.AddRange(fields);
                foreach (Field field in m.TournamentStage.Pool.FavoriteFields)
                {
                    List<NextFreeTime> nextFreeTimes = field.NextFreeTime.ToList();
                    if (validator.areTeamsFree(m, nextFreeTimes[i].FreeTime))
                    {
                        results = new Tuple<DateTime, Field>(nextFreeTimes[i].FreeTime, field);
                        return results;
                    }
                    fieldsNotChecked.Remove(field);
                }
                foreach (Field field in fieldsNotChecked)
                {
                    List<NextFreeTime> nextFreeTimes = field.NextFreeTime.ToList();
                    if (validator.areTeamsFree(m, nextFreeTimes[i].FreeTime))
                    {
                        results = new Tuple<DateTime, Field>(nextFreeTimes[i].FreeTime, field);
                        return results;
                    }
                }
            }
            
            results = new Tuple<DateTime, Field>(DateTime.MinValue, null);
            return results;
        }

        // Deletes the whole schedule in a tournament
        public void DeleteSchedule(int tournamentID)
        {
            MatchGeneration mg = new MatchGeneration();
            Tournament t = db.TournamentSet.Find(tournamentID);
            foreach (Division d in t.Divisions.ToList())
            {
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
            db.SaveChanges();
        }

    }
}