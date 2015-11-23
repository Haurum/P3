using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CupPlaner.Controllers;
using System.Web.Mvc;
using System.Data.Entity;

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
                //gruopstage matches generation
                DivisionTournament dt = db.DivisionTournamentSet.Add(new DivisionTournament() { TournamentStructure = d.TournamentStructure, Division = d });
                foreach (Pool p in d.Pools)
                {
                    TournamentStage ts = db.TournamentStageSet.Add(new TournamentStage() { Pool = p, DivisionTournament = dt, TournamentStructure = TournamentStructure.RoundRobin });
                    List<Team> teams = p.Teams.ToList();
                    for (int i = 0; i < teams.Count; i++)
                    {
                        for (int j = i + 1; j < teams.Count; j++)
                        {
                            
                            db.MatchSet.Add(new Match() { Teams = new List<Team>() { teams[i], teams[j] }, TournamentStage = ts, Duration = d.MatchDuration });
                        }
                    }
                }

                //finals pools and team generation
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
                
                //finals matches generation
                List<Pool> finalsPools = d.Pools.Where(x => x.IsAuto).ToList();
                foreach (Pool finalPool in finalsPools)
                {
                    List<Team> teams = finalPool.Teams.ToList();

                    //if finals are round robin
                    if (d.TournamentStructure == TournamentStructure.RoundRobin)
                    {
                        TournamentStage tstage = db.TournamentStageSet.Add(new TournamentStage() { Pool = finalPool, DivisionTournament = dt, TournamentStructure = dt.TournamentStructure });

                        for (int k = 0; k < teams.Count; k++)
                        {
                            for (int l = k + 1; l < teams.Count; l++)
                            {
                                db.MatchSet.Add(new Match() { Teams = new List<Team>() { teams[k], teams[l] }, TournamentStage = tstage, Duration = d.MatchDuration });
                            }
                        }
                    }
                    //if finals are knockout
                    else
                    {
                        db.TeamSet.RemoveRange(teams);
                        db.PoolSet.Remove(finalPool);
                        Pool KOPool = new Pool();
                        TournamentStage tournyStage = new TournamentStage();
                        int pow = 0;
                        while (Math.Pow(2, pow) <= teams.Count)
                        {
                            pow++;
                        }
                        int powOfTwo = (int)Math.Pow(2, pow-1);
                        if (powOfTwo < teams.Count)
                        {
                            int numOfExtraTeams = (teams.Count - powOfTwo) * 2;
                            teams = teams.OrderByDescending(x => x.Name).ToList();
                            List<Team> extraTeams = teams.Take(numOfExtraTeams).ToList();
                            teams = teams.Skip(numOfExtraTeams).ToList();
                            switch (powOfTwo)
                            {
                                case 2:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " semi finaler", Division = d, IsAuto = true});
                                    teams.Add(new Team() { Name = "Vinder af " + finalPool.Name + " semi finale 1", IsAuto = true, Pool= KOPool });
                                    break;
                                case 4:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " kvart finaler", Division = d, IsAuto = true });
                                    for (int i = 1; i <= numOfExtraTeams/2; i++)
                                    {
                                        teams.Add(new Team() { Name = "Vinder af " + finalPool.Name + " kvart finale " + i, IsAuto = true, Pool = KOPool });
                                    }
                                    break;
                                default:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " " + powOfTwo + ". dels finaler", Division = d, IsAuto = true });
                                    for (int i = 0; i < numOfExtraTeams/2; i++)
                                    {
                                        teams.Add(new Team() { Name = "Vinder af " + finalPool.Name + " " + powOfTwo + ". dels finale " + i, IsAuto = true, Pool = KOPool });
                                    }
                                    break;
                            }
                            tournyStage = db.TournamentStageSet.Add(new TournamentStage() { Pool = KOPool, DivisionTournament = dt, TournamentStructure = dt.TournamentStructure });
                            
                            for (int i = 0; i< extraTeams.Count; i++)
                            {
                                
                                if (extraTeams[i].Matches.Count == 0)
                                {
                                    for (int j = i+1; j < extraTeams.Count; j++)
                                    {
                                        if (extraTeams[i].Pool != extraTeams[j].Pool)
                                        {
                                            db.MatchSet.Add(new Match() { Teams = { extraTeams[i], extraTeams[j] }, Duration = d.MatchDuration, TournamentStage = tournyStage });
                                            break;
                                        }
                                    }
                                    db.MatchSet.Add(new Match() { Teams = { extraTeams[i], extraTeams[i+1] }, Duration = d.MatchDuration, TournamentStage = tournyStage });
                                }
                                extraTeams[i].Pool = KOPool;
                                db.TeamSet.Add(extraTeams[i]);
                            }
                        }
                        List<Team> teamsToAdd = new List<Team>();
                        while (teams.Count != 0)
                        {
                            teamsToAdd.AddRange(teams);
                            teams.Clear();
                            switch (teamsToAdd.Count)
                            {
                                case 2:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " finale", Division = d, IsAuto = true });
                                    break;
                                case 4:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " semi finaler", Division = d, IsAuto = true });
                                    teams.Add(new Team() { Name = "vinder af " + finalPool.Name + " semi finale 1", Pool = KOPool, IsAuto = true });
                                    teams.Add(new Team() { Name = "vinder af " + finalPool.Name + " semi finale 2", Pool = KOPool, IsAuto = true });
                                    break;
                                case 8:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " kvart finaler", Division = d, IsAuto = true });
                                    for (int i = 1; i <= 4; i++)
                                    {
                                        teams.Add(new Team() { Name = "vinder af " + finalPool.Name + " kvart finale " + i });
                                    }
                                    break;
                                default:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " kvart finaler", Division = d, IsAuto = true });
                                    for (int i = 1; i <= teams.Count; i++)
                                    {
                                        teams.Add(new Team() { Name = "vinder af " + finalPool.Name + teams.Count + ". dels finale " + i, Pool = KOPool, IsAuto = true });
                                    }
                                    break;
                            }
                            tournyStage = db.TournamentStageSet.Add(new TournamentStage() { Pool = KOPool, DivisionTournament = dt, TournamentStructure = dt.TournamentStructure });
                            for (int i = 0; i < teamsToAdd.Count; i++)
                            {

                                if (teamsToAdd[i].Matches.Count == 0)
                                {
                                    for (int j = i + 1; j < teamsToAdd.Count; j++)
                                    {
                                        if (teamsToAdd[i].Pool != teamsToAdd[j].Pool)
                                        {
                                            db.MatchSet.Add(new Match() { Teams = { teamsToAdd[i], teamsToAdd[j] }, Duration = d.MatchDuration, TournamentStage = tournyStage });
                                            break;
                                        }
                                    }
                                }
                                teamsToAdd[i].Pool = KOPool;
                                db.TeamSet.Add(teamsToAdd[i]);
                            }
                            teamsToAdd.Clear();
                        }

                    }                
                }               
            }
            db.SaveChanges();
        }

    }
}
