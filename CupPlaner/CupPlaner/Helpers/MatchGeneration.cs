using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CupPlaner.Controllers;
using System.Web.Mvc;

namespace CupPlaner.Helpers
{
    public class MatchGeneration
    {
        CupDBContainer db = new CupDBContainer();

        public void Generate(int tournamentID)
        {
            Tournament t = db.TournamentSet.Find(tournamentID);
            

            foreach (Division d in t.Divisions)
            {
                DivisionTournament dt = db.DivisionTournamentSet.Add(new DivisionTournament() { TournamentStructure = d.TournamentStructure, Division = d });
                foreach (Pool p in d.Pools)
                {
                    TournamentStage ts = db.TournamentStageSet.Add(new TournamentStage() { Pool = p, DivisionTournament = dt, TournamentStructure = dt.TournamentStructure });
                    List<Team> teams = p.Teams.ToList();
                    for (int i = 0; i < teams.Count; i++)
                    {
                        for (int j = i + 1; j < teams.Count; j++)
                        {
                            
                            db.MatchSet.Add(new Match() { Teams = new List<Team>() { teams[i], teams[j] }, TournamentStage = ts });
                        }
                    }
                }

                int finalsIndex = 0;
                Pool autoPool = new Pool();
                foreach (FinalsLink fl in d.FinalsLinks)
                {
                    
                    if (finalsIndex < fl.Finalstage)
                    {
                        finalsIndex = fl.Finalstage;
                        autoPool = db.PoolSet.Add(new Pool() { Name = d.Name + " " + (char)(64 + finalsIndex) + " slutspil", Division = d, IsAuto = true });
                    }

                    foreach (Pool pool in d.Pools)
                    {
                        if (!pool.IsAuto)
                        {
                            if (pool.Teams.Count >= fl.PoolPlacement)
                            {
                                db.TeamSet.Add(new Team() { Name = "Nr " + fl.PoolPlacement + " fra " + d.Name + " - " + pool.Name, IsAuto = true, Pool = autoPool });
                            }                          
                        }
                    }                    
                }
                db.SaveChanges();

                List<Pool> finalsPools = db.PoolSet.Where(x => x.IsAuto).ToList();
                foreach (Pool finalPool in finalsPools)
                {
                    TournamentStage tstage = db.TournamentStageSet.Add(new TournamentStage() { Pool = finalPool, DivisionTournament = dt, TournamentStructure = dt.TournamentStructure });
                    List<Team> teams = finalPool.Teams.ToList();
                    for (int k = 0; k < teams.Count; k++)
                    {
                        for (int l = k + 1; l < teams.Count; l++)
                        {

                            db.MatchSet.Add(new Match() { Teams = new List<Team>() { teams[k], teams[l] }, TournamentStage = tstage });
                        }
                    }
                }

            }
            db.SaveChanges();
        }

    }
}
