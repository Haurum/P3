using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CupPlaner.Helpers
{
    public class Validator
    {
        CupDBContainer db = new CupDBContainer();

        public bool IsScheduleReady()
        {
            bool isValid = true;
            foreach (Tournament t in db.TournamentSet)
            {
                if (t.Divisions.Count < 1)
                {
                    isValid = false;
                    return isValid;
                }
                foreach (Division d in db.DivisionSet)
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