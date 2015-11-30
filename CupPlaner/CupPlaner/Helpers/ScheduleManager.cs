using System;
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
            List<TournamentStage> TournamentStages = db.TournamentStageSet.Where(x => x.DivisionTournament.Division.Tournament == t).ToList();

            while (!t.isScheduled)
            {
                foreach (TournamentStage ts in TournamentStages)
                {
                    if (ts.Matches.First().Teams.First().PrevPool == null)
                    {

                    }
                    else if (ts.Matches.First().Teams.First().PrevPool.TournamentStage.IsScheduled)
                    {
                        if (ts.startTime == t.TimeIntervals.First().StartTime)
                        {
                            ts.startTime = ts.Matches.First().Teams.First().PrevPool.TournamentStage.endTime;
                        }
                    }
                    else
                    {
                        continue;
                    }

                }
            }
                
        }

        public Tuple<DateTime, Field> scheduleMatch(Match m)
        {
            Tuple<DateTime, Field> results;
            List<Field> fields = m.TournamentStage.DivisionTournament.Division.Tournament.Fields.Where(x => x.Size == m.TournamentStage.DivisionTournament.Division.FieldSize).ToList();
            List<Field> fieldsNotChecked = new List<Field>();
            fieldsNotChecked.AddRange(fields);
            foreach (Field field in m.TournamentStage.Pool.FavoriteFields)
            {
                if (validator.areTeamsFree(m, field.nextFreeTime))
                {
                    results = new Tuple<DateTime, Field>(field.nextFreeTime, field);
                    return results;
                }
                fieldsNotChecked.Remove(field);
            }
            foreach (Field field in fieldsNotChecked)
            {
                if (validator.areTeamsFree(m, field.nextFreeTime))
                {
                    results = new Tuple<DateTime, Field>(field.nextFreeTime, field);
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