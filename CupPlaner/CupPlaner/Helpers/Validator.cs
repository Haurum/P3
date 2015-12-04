using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CupPlaner.Helpers
{
    public class Validator
    {
        CupDBContainer db = new CupDBContainer();

        public bool areTeamsFree(Match m, DateTime startTime)
        {
            if (m.TournamentStage.TimeInterval.StartTime > startTime)
            {
                return false;
            }
            foreach (Team team in m.Teams)
            {
                TimeInterval timesForDate = team.TimeIntervals.First(x => x.StartTime.Date == startTime.Date);
                if (startTime < timesForDate.StartTime || startTime.AddMinutes(m.Duration) > timesForDate.EndTime)
                {
                    return false;
                }
                foreach (Match match in team.Matches)
                {
                    if (match.IsScheduled && startTime.AddMinutes(m.Duration * 2) > match.StartTime)
                    {
                        DateTime teamBreakDone = match.StartTime.AddMinutes(match.Duration * 2);
                        if (startTime < teamBreakDone)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool IsScheduleReady(int tournamentId)
        {
            bool isValid = true;
            Tournament t = db.TournamentSet.Find(tournamentId);

            if(t.Divisions.Count < 1)
            {
                isValid = false;
                return isValid;
            }
            else
            {
                foreach (Division d in t.Divisions)
                {
                    if (d.Pools.Count < 1)
                    {
                        isValid = false;
                        return isValid;
                    }
                    foreach (Pool p in d.Pools)
                    {
                        if (p.Teams.Count < 2)
                        {
                            isValid = false;
                            return isValid;
                        }
                    }
                }
            }
            return isValid;
        }
    }
}