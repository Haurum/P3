using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CupPlaner.Controllers;
using System.Web.Mvc;

namespace CupPlaner.Helpers
{
    class MatchGeneration
    {
        MatchController mc = new MatchController();
        DivisionTournamentController dtc = new DivisionTournamentController();
        TournamentStageController tsc = new TournamentStageController();
        CupDBContainer db = new CupDBContainer();

        public bool Generate(int tournamentID)
        {
            Tournament t = db.TournamentSet.Find(tournamentID);
            foreach (Division d in t.Divisions)
            {
                dynamic jsonResult = ((JsonResult)dtc.Create(d.Id)).Data;
                if(jsonResult.status == "success")
                {
                    DivisionTournament dt = db.DivisionTournamentSet.Find(jsonResult.id);
                    if (!GenerateGroupStage(d, dt))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private bool GenerateGroupStage(Division division, DivisionTournament divTournament)
        {
            foreach (Pool p in division.Pools)
            {
                dynamic tsJsonResult = ((JsonResult)tsc.Create(divTournament.Id, p.Id)).Data;
                if (tsJsonResult.status == "success")
                {
                    TournamentStage ts = db.TournamentStageSet.Find(tsJsonResult.id);

                    if (!GenerateRoundRobin(p, ts))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private bool GenerateRoundRobin(Pool p, TournamentStage ts)
        {
            for (int i = 0; i < p.Teams.Count; i++)
            {
                for (int j = i + 1; j < p.Teams.Count; j++)
                {
                    dynamic matchJsonResult = ((JsonResult)mc.Create(p.Teams[i], p.Teams[j])).Data;
                    if (matchJsonResult.status == "success")
                    {
                        Match m = db.MatchSet.Find(matchJsonResult.id);
                        ts.Matches.Add(m);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
