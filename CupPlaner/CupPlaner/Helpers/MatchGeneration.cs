﻿using System;
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

                    foreach (Pool p in d.Pools)
                    {
                        if (!p.IsAuto)
                        {
                            if (p.Teams.Count >= fl.PoolPlacement)
                            {
                                db.TeamSet.Add(new Team() { Name = "Nr " + fl.PoolPlacement + " fra " + d.Name + " - " + p.Name, IsAuto = true, Pool = autoPool });
                            }                          
                        }
                    }                    
                }


                List<Pool> finalsPools = db.PoolSet.Where(x => x.IsAuto).ToList();
                if (d.TournamentStructure == TournamentStructure.RoundRobin)
                {
                    foreach (Pool finalPool in finalsPools)
                    {
                        TournamentStage ts = db.TournamentStageSet.Add(new TournamentStage() { Pool = finalPool, DivisionTournament = dt, TournamentStructure = dt.TournamentStructure });
                        List<Team> teams = finalPool.Teams.ToList();
                        for (int i = 0; i < teams.Count; i++)
                        {
                            for (int j = i + 1; j < teams.Count; j++)
                            {

                                db.MatchSet.Add(new Match() { Teams = new List<Team>() { teams[i], teams[j] }, TournamentStage = ts });
                            }
                        }
                    }
                }

            }
            db.SaveChanges();
        }

        /*private void GenerateGroupStage(Division division, DivisionTournament divTournament)
        {
            foreach (Pool p in division.Pools)
            {
                    TournamentStage ts = tsc.Create(divTournament.Id, p.Id);
                    GenerateRoundRobin(p, ts);
            }
        }

        private void GenerateRoundRobin(Pool p, TournamentStage ts)
        {
            List<Team> teams = p.Teams.ToList();
            for (int i = 0; i < p.Teams.Count; i++)
            {
                for (int j = i + 1; j < p.Teams.Count; j++)
                {
                        mc.Create(teams[i].Id, teams[j].Id, ts.Id);

                }
            }
        }*/

    }
}
