using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CupPlaner.Helpers
{
    public class Validator
    {
        CupDBContainer db = new CupDBContainer();

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