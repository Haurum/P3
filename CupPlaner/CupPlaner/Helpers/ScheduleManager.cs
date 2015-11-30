﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CupPlaner.Helpers
{
    public class ScheduleManager
    {
        CupDBContainer db = new CupDBContainer();
        MatchGeneration mg = new MatchGeneration();
        Validator validator = new Validator();

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
                foreach (TournamentStage ts in TournamentStages)
                {
                    if (ts.IsScheduled)
                    {
                        continue;
                    }
                    else if (ts.Matches.First().Teams.First().PrevPool == null) { }
                    else if (ts.Matches.First().Teams.First().PrevPool.TournamentStage.IsScheduled && ts.TimeInterval.StartTime == t.TimeIntervals.First().StartTime)
                    {
                        ts.TimeInterval.StartTime = ts.Matches.First().Teams.First().PrevPool.TournamentStage.TimeInterval.EndTime;
                    }
                    else
                    {
                        continue;
                    }

                    List<Match> unscheduledMatches = ts.Matches.Where(x => !x.IsScheduled).ToList();
                    Tuple<DateTime, Field> result = new Tuple<DateTime, Field>(DateTime.MinValue, null);
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

                    result = scheduleMatch(matchToSchedule);
                    matchToSchedule.StartTime = result.Item1;
                    matchToSchedule.Field = result.Item2;
                    matchToSchedule.IsScheduled = true;
                    matchToSchedule.Field.NextFreeTime = matchToSchedule.Field.NextFreeTime.AddMinutes(matchToSchedule.Duration);
                    db.Entry(matchToSchedule).State = System.Data.Entity.EntityState.Modified;
                    db.Entry(result.Item2).State = System.Data.Entity.EntityState.Modified;
                }

                indicator *= -1;
            }
            db.SaveChanges();
                
        }

        public Tuple<DateTime, Field> scheduleMatch(Match m)
        {
            Tuple<DateTime, Field> results;
            List<Field> fields = m.TournamentStage.DivisionTournament.Division.Tournament.Fields.Where(x => x.Size == m.TournamentStage.DivisionTournament.Division.FieldSize).ToList();
            List<Field> fieldsNotChecked = new List<Field>();
            fieldsNotChecked.AddRange(fields);
            foreach (Field field in m.TournamentStage.Pool.FavoriteFields)
            {
                if (validator.areTeamsFree(m, field.NextFreeTime))
                {
                    results = new Tuple<DateTime, Field>(field.NextFreeTime, field);
                    return results;
                }
                fieldsNotChecked.Remove(field);
            }
            foreach (Field field in fieldsNotChecked)
            {
                if (validator.areTeamsFree(m, field.NextFreeTime))
                {
                    results = new Tuple<DateTime, Field>(field.NextFreeTime, field);
                    return results;
                }
            }
            results = new Tuple<DateTime, Field>(DateTime.MinValue, null);
            return results;
        }

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