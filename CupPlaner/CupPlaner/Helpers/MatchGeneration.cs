using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CupPlaner.Controllers;

namespace CupPlaner.Helpers
{
    class MatchGeneration
    {
        TournamentController tournamentCon = new TournamentController();
        CupDBContainer db = new CupDBContainer();

        public bool Generate(int tournamentID)
        {
            Tournament t = db.TournamentSet.Find(tournamentID);
            foreach (Division d in t.Divisions)
            {
                
            }
            return false;
        }

    }
}
