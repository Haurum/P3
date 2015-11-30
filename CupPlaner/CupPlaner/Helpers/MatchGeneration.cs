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
            int matchNumber = 1;

            foreach (Division d in t.Divisions)
            {
                //gruopstage matches generation
                DivisionTournament dt = db.DivisionTournamentSet.Add(new DivisionTournament() { TournamentStructure = d.TournamentStructure, Division = d });
                foreach (Pool p in d.Pools)
                {
                    TournamentStage ts = db.TournamentStageSet.Add(new TournamentStage() { Pool = p, DivisionTournament = dt, TournamentStructure = TournamentStructure.RoundRobin, TimeInterval = new TimeInterval() { StartTime = t.TimeIntervals.First().StartTime, EndTime = t.TimeIntervals.Last().EndTime } });
                    List<Team> teams = p.Teams.ToList();
                    for (int i = 0; i < teams.Count; i++)
                    {
                        for (int j = i + 1; j < teams.Count; j++)
                        {
                            
                            db.MatchSet.Add(new Match() { Teams = { teams[i], teams[j] }, TournamentStage = ts, Duration = d.MatchDuration, Number = matchNumber++ });
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
                                Team team = db.TeamSet.Add(new Team() { Name = "Nr " + fl.PoolPlacement + " fra " + d.Name + " - " + pool.Name, PoolPlacement = fl.PoolPlacement, PrevPool = pool, IsAuto = true, Pool = autoPool});
                                foreach (TimeInterval ti in t.TimeIntervals)
                                {
                                    TimeInterval timeInterval = db.TimeIntervalSet.Add(new TimeInterval() { Team = team, StartTime = ti.StartTime, EndTime = ti.EndTime });
                                    team.TimeIntervals.Add(timeInterval);
                                }
                            }                          
                        }
                    }                    
                }
                
                //finals matches generation
                List<Pool> finalsPools = d.Pools.Where(x => x.IsAuto).ToList();
                foreach (Pool finalPool in finalsPools)
                {
                    List<Team> teams = new List<Team>();
                    teams.AddRange(finalPool.Teams);
                    

                    //if finals are round robin
                    if (d.TournamentStructure == TournamentStructure.RoundRobin)
                    {
                        TournamentStage tStage = new TournamentStage();
                        tStage = db.TournamentStageSet.Add(new TournamentStage() { Pool = finalPool, DivisionTournament = dt, TournamentStructure = dt.TournamentStructure, TimeInterval = new TimeInterval() { StartTime = t.TimeIntervals.First().StartTime, EndTime = t.TimeIntervals.Last().EndTime } });

                        for (int k = 0; k < teams.Count; k++)
                        {
                            for (int l = k + 1; l < teams.Count; l++)
                            {
                                db.MatchSet.Add(new Match() { Teams = { teams[k], teams[l] }, TournamentStage = tStage, Duration = d.MatchDuration, Number = matchNumber++ });
                            }
                        }
                    }
                    //if finals are knockout
                    else
                    {
                        //db.TeamSet.RemoveRange(teams);
                        //db.PoolSet.Remove(finalPool);
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
                            teams = teams.OrderByDescending(x => x.PoolPlacement).ToList();
                            List<Team> extraTeams = new List<Team>();
                            extraTeams.AddRange(teams.Take(numOfExtraTeams));
                            teams = teams.Skip(numOfExtraTeams).ToList();
                            switch (powOfTwo)
                            {
                                case 2:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " semi finaler", Division = d, IsAuto = true});
                                    break;
                                case 4:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " kvart finaler", Division = d, IsAuto = true });
                                    break;
                                default:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " " + powOfTwo + ". dels finaler", Division = d, IsAuto = true });
                                    break;
                            }
                            tournyStage = db.TournamentStageSet.Add(new TournamentStage() { Pool = KOPool, DivisionTournament = dt, TournamentStructure = dt.TournamentStructure, TimeInterval = new TimeInterval() { StartTime = t.TimeIntervals.First().StartTime, EndTime = t.TimeIntervals.Last().EndTime } });
                            
                            for (int i = 0; i< extraTeams.Count; i++)
                            {
                                
                                if (extraTeams[i].Matches.Count == 0)
                                {
                                    for (int j = extraTeams.Count-1; j > i; j--)
                                    {
                                        if (extraTeams[i].PrevPool != extraTeams[j].PrevPool && extraTeams[j].Matches.Count == 0)
                                        {
                                            Team winnerTeam = new Team() { Name = "vinder af kamp " + matchNumber, IsAuto = true, PrevPool = KOPool, Pool = KOPool };
                                            teams.Add(winnerTeam);
                                            Match m = db.MatchSet.Add(new Match() { Teams = { extraTeams[i], extraTeams[j] }, Duration = d.MatchDuration, TournamentStage = tournyStage, Number = matchNumber++ });
                                            break;
                                        }
                                    }
                                    if (extraTeams[i].Matches.Count == 0)
                                    {
                                        for (int j = extraTeams.Count-1; j > i; j--)
                                        {
                                            if (extraTeams[j].Matches.Count == 0)
                                            {
                                                Team winnerTeam = new Team() { Name = "vinder af kamp " + matchNumber, IsAuto = true, PrevPool = KOPool, Pool = KOPool};
                                                teams.Add(winnerTeam);
                                                Match m = db.MatchSet.Add(new Match() { Teams = { extraTeams[i], extraTeams[j] }, Duration = d.MatchDuration, TournamentStage = tournyStage, Number = matchNumber++ });
                                                break;
                                            }
                                        }
                                    }
                                }
                                extraTeams[i].Pool = KOPool;
                                db.TeamSet.Add(extraTeams[i]);
                                foreach (TimeInterval ti in t.TimeIntervals)
                                {
                                    TimeInterval timeInterval = db.TimeIntervalSet.Add(new TimeInterval() { Team = extraTeams[i], StartTime = ti.StartTime, EndTime = ti.EndTime });
                                    extraTeams[i].TimeIntervals.Add(timeInterval);
                                }
                            }
                        }
                        List<Team> teamsToAdd = new List<Team>();
                        while (teams.Count > 1)
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
                                    break;
                                case 8:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " kvart finaler", Division = d, IsAuto = true });
                                    break;
                                default:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " " + teamsToAdd.Count / 2 + ". dels finaler", Division = d, IsAuto = true });
                                    break;
                            }
                            tournyStage = db.TournamentStageSet.Add(new TournamentStage() { Pool = KOPool, DivisionTournament = dt, TournamentStructure = dt.TournamentStructure, TimeInterval = new TimeInterval() { StartTime = t.TimeIntervals.First().StartTime, EndTime = t.TimeIntervals.Last().EndTime } });
                            if (teamsToAdd.Count == 2)
                            {
                                Match m = db.MatchSet.Add(new Match() { Teams = { teamsToAdd[0], teamsToAdd[1] }, Duration = d.MatchDuration, TournamentStage = tournyStage, Number = matchNumber++ });
                                teamsToAdd[0].Pool = KOPool;
                                teamsToAdd[1].Pool = KOPool;
                                db.TeamSet.AddRange(teamsToAdd);
                                foreach (TimeInterval ti in t.TimeIntervals)
                                {
                                    TimeInterval timeInterval = db.TimeIntervalSet.Add(new TimeInterval() { Team = teamsToAdd[0], StartTime = ti.StartTime, EndTime = ti.EndTime });
                                    teamsToAdd[0].TimeIntervals.Add(timeInterval);
                                    timeInterval = db.TimeIntervalSet.Add(new TimeInterval() { Team = teamsToAdd[1], StartTime = ti.StartTime, EndTime = ti.EndTime });
                                    teamsToAdd[1].TimeIntervals.Add(timeInterval);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < teamsToAdd.Count; i++)
                                {

                                    if (teamsToAdd[i].Matches.Count == 0)
                                    {
                                        for (int j = teamsToAdd.Count-1; j > i; j--)
                                        {
                                            if (teamsToAdd[i].PrevPool != teamsToAdd[j].PrevPool && teamsToAdd[j].Matches.Count == 0)
                                            {
                                                Team winnerTeam = new Team() { Name = "vinder af kamp " + matchNumber, IsAuto = true, PrevPool = KOPool, Pool = KOPool };
                                                teams.Add(winnerTeam);
                                                Match m = db.MatchSet.Add(new Match() { Teams = { teamsToAdd[i], teamsToAdd[j] }, Duration = d.MatchDuration, TournamentStage = tournyStage, Number = matchNumber++ });
                                                break;
                                            }
                                        }
                                        if (teamsToAdd[i].Matches.Count == 0)
                                        {
                                            for (int j = teamsToAdd.Count-1; j > i; j--)
                                            {
                                                if (teamsToAdd[j].Matches.Count == 0)
                                                {
                                                    Team winnerTeam = new Team() { Name = "vinder af kamp " + matchNumber, IsAuto = true, PrevPool = KOPool, Pool = KOPool };
                                                    teams.Add(winnerTeam);
                                                    Match m = db.MatchSet.Add(new Match() { Teams = { teamsToAdd[i], teamsToAdd[j] }, Duration = d.MatchDuration, TournamentStage = tournyStage, Number = matchNumber++ });
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    teamsToAdd[i].Pool = KOPool;
                                    db.TeamSet.Add(teamsToAdd[i]);
                                    foreach (TimeInterval ti in t.TimeIntervals)
                                    {
                                        TimeInterval timeInterval = db.TimeIntervalSet.Add(new TimeInterval() { Team = teamsToAdd[i], StartTime = ti.StartTime, EndTime = ti.EndTime });
                                        teamsToAdd[i].TimeIntervals.Add(timeInterval);
                                    }
                                }
                            }                            
                            teamsToAdd.Clear();
                        }
                        db.PoolSet.Remove(finalPool);
                        //db.SaveChanges();
                    }
                    
                }               
            }
            db.SaveChanges();
        }
    }
}
