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