using Microsoft.VisualStudio.TestTools.UnitTesting;
using CupPlaner.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CupPlaner.Controllers;

namespace CupPlaner.Helpers.Tests
{
    [TestClass()]
    public class MatchGenerationTests
    {
        CupDBContainer db = new CupDBContainer();
        DivisionTournamentController dtc = new DivisionTournamentController();

        [TestMethod()]
        public void GenerateTest()
        {
            MatchGeneration mg = new MatchGeneration();
            Tournament t = db.TournamentSet.Find(1);
            foreach (Division d in t.Divisions.ToList())
            {
                if (d.DivisionTournament != null)
                {
                    foreach (TournamentStage ts in d.DivisionTournament.TournamentStage.ToList())
                    {
                        foreach (Match m in ts.Matches.ToList())
                        {
                            foreach (Team team in m.Teams.ToList())
                            {
                                team.Matches.Remove(m);
                            }
                            db.MatchSet.Remove(m);
                        }
                        db.TournamentStageSet.Remove(ts);
                    }
                    db.DivisionTournamentSet.Remove(d.DivisionTournament);
                    //dtc.Delete(d.DivisionTournament.Id);
                }

                int maxNumOfTeams = 0;

                foreach (Pool p in d.Pools)
                {
                    if (maxNumOfTeams < p.Teams.Count)
                    {
                        maxNumOfTeams = p.Teams.Count;
                    }
                }

                for (int i = 1; i <= maxNumOfTeams; i++)
                {
                    db.FinalsLinkSet.Add(new FinalsLink() { Division = d, PoolPlacement = i, Finalstage = (i / 2) + 1 });
                }
            }
            db.SaveChanges();
            
           mg.Generate(1);
        }
    }
}