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
        public static int matchNumber;
        public bool GenerateGroupStage(int tournamentID)
        {
            CupDBContainer db = new CupDBContainer();
            Tournament t = db.TournamentSet.Find(tournamentID);
            matchNumber = 1;

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
            }
            db.SaveChanges();
            return true;
        }
        
        public bool GenerateFinalsTeams(int tournamentID)
        {
            CupDBContainer db = new CupDBContainer();
            Tournament t = db.TournamentSet.Find(tournamentID);
            foreach (Division d in t.Divisions)
            {
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
                                Team team = db.TeamSet.Add(new Team() { Name = "Nr " + fl.PoolPlacement + " fra " + d.Name + " - " + pool.Name, PoolPlacement = fl.PoolPlacement, PrevPool = pool, IsAuto = true, Pool = autoPool });
                                team.TimeIntervals = SameTimeInterval(team.PrevPool);
                            }
                        }
                    }
                }
            }
            db.SaveChanges();
            return true;
        }

        public bool GenerateFinalsMatches(int tournamentID)
        {
            CupDBContainer db = new CupDBContainer();
            Tournament t = db.TournamentSet.Find(tournamentID);
            foreach (Division d in t.Divisions)
            {
                List<Pool> finalsPools = d.Pools.Where(x => x.IsAuto).ToList();
                foreach (Pool finalPool in finalsPools)
                {
                    if (finalPool.Teams.Count <2)
                    {
                        throw new Exception("not enough teams");
                    }
                    List<Team> teams = new List<Team>();
                    teams.AddRange(finalPool.Teams);


                    //if finals are round robin
                    if (d.TournamentStructure == TournamentStructure.RoundRobin)
                    {
                        TournamentStage tStage = new TournamentStage();
                        tStage = db.TournamentStageSet.Add(new TournamentStage() { Pool = finalPool, DivisionTournament = d.DivisionTournament, TournamentStructure = d.TournamentStructure, TimeInterval = new TimeInterval() { StartTime = DateTime.MinValue, EndTime = t.TimeIntervals.Last().EndTime } });

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
                        Pool KOPool = new Pool();
                        TournamentStage tournyStage = new TournamentStage();
                        int pow = 0;
                        while (Math.Pow(2, pow) <= teams.Count)
                        {
                            pow++;
                        }
                        int powOfTwo = (int)Math.Pow(2, pow - 1);
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
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " semi finaler", Division = d, IsAuto = true });
                                    break;
                                case 4:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " kvart finaler", Division = d, IsAuto = true });
                                    break;
                                default:
                                    KOPool = db.PoolSet.Add(new Pool() { Name = finalPool.Name + " " + powOfTwo + ". dels finaler", Division = d, IsAuto = true });
                                    break;
                            }
                            tournyStage = db.TournamentStageSet.Add(new TournamentStage() { Pool = KOPool, DivisionTournament = d.DivisionTournament, TournamentStructure = d.TournamentStructure, TimeInterval = new TimeInterval() { StartTime = DateTime.MinValue, EndTime = t.TimeIntervals.Last().EndTime } });

                            for (int i = 0; i < extraTeams.Count; i++)
                            {

                                if (extraTeams[i].Matches.Count == 0)
                                {
                                    for (int j = extraTeams.Count - 1; j > i; j--)
                                    {
                                        if (extraTeams[i].PrevPool != extraTeams[j].PrevPool && extraTeams[j].Matches.Count == 0)
                                        {
                                            Team winnerTeam = new Team() { Name = "Vinder af kamp " + matchNumber, IsAuto = true, PrevPool = KOPool };
                                            teams.Add(winnerTeam);
                                            Match m = db.MatchSet.Add(new Match() { Teams = { extraTeams[i], extraTeams[j] }, Duration = d.MatchDuration, TournamentStage = tournyStage, Number = matchNumber++ });
                                            break;
                                        }
                                    }
                                    if (extraTeams[i].Matches.Count == 0)
                                    {
                                        for (int j = extraTeams.Count - 1; j > i; j--)
                                        {
                                            if (extraTeams[j].Matches.Count == 0)
                                            {
                                                Team winnerTeam = new Team() { Name = "Vinder af kamp " + matchNumber, IsAuto = true, PrevPool = KOPool };
                                                teams.Add(winnerTeam);
                                                Match m = db.MatchSet.Add(new Match() { Teams = { extraTeams[i], extraTeams[j] }, Duration = d.MatchDuration, TournamentStage = tournyStage, Number = matchNumber++ });
                                                break;
                                            }
                                        }
                                    }
                                }
                                extraTeams[i].Pool = KOPool;
                                db.TeamSet.Add(extraTeams[i]);
                                extraTeams[i].TimeIntervals = SameTimeInterval(extraTeams[i].PrevPool);
                            }
                        }
                        List<Team> teamsToAdd = new List<Team>();
                        while (teams.Count > 0)
                        {
                            teamsToAdd.AddRange(teams);
                            teams.Clear();
                            switch (teamsToAdd.Count)
                            {
                                case 1:
                                    throw new Exception("Not enough teams");
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
                            if (teams.Count != 1)
                            {
                                tournyStage = db.TournamentStageSet.Add(new TournamentStage() { Pool = KOPool, DivisionTournament = d.DivisionTournament, TournamentStructure = d.TournamentStructure, TimeInterval = new TimeInterval() { StartTime = DateTime.MinValue, EndTime = t.TimeIntervals.Last().EndTime } });
                                if (teamsToAdd.Count == 2)
                                {
                                    Match m = db.MatchSet.Add(new Match() { Teams = { teamsToAdd[0], teamsToAdd[1] }, Duration = d.MatchDuration, TournamentStage = tournyStage, Number = matchNumber++ });
                                    teamsToAdd[0].Pool = KOPool;
                                    teamsToAdd[1].Pool = KOPool;
                                    db.TeamSet.AddRange(teamsToAdd);
                                    teamsToAdd[0].TimeIntervals = SameTimeInterval(teamsToAdd[0].PrevPool);
                                    teamsToAdd[1].TimeIntervals = SameTimeInterval(teamsToAdd[1].PrevPool);
                                }
                                else
                                {
                                    for (int i = 0; i < teamsToAdd.Count; i++)
                                    {

                                        if (teamsToAdd[i].Matches.Count == 0)
                                        {
                                            for (int j = teamsToAdd.Count - 1; j > i; j--)
                                            {
                                                if (teamsToAdd[i].PrevPool != teamsToAdd[j].PrevPool && teamsToAdd[j].Matches.Count == 0)
                                                {
                                                    Team winnerTeam = new Team() { Name = "Vinder af kamp " + matchNumber, IsAuto = true, PrevPool = KOPool };
                                                    teams.Add(winnerTeam);
                                                    Match m = db.MatchSet.Add(new Match() { Teams = { teamsToAdd[i], teamsToAdd[j] }, Duration = d.MatchDuration, TournamentStage = tournyStage, Number = matchNumber++ });
                                                    break;
                                                }
                                            }
                                            if (teamsToAdd[i].Matches.Count == 0)
                                            {
                                                for (int j = teamsToAdd.Count - 1; j > i; j--)
                                                {
                                                    if (teamsToAdd[j].Matches.Count == 0)
                                                    {
                                                        Team winnerTeam = new Team() { Name = "Vinder af kamp " + matchNumber, IsAuto = true, PrevPool = KOPool };
                                                        teams.Add(winnerTeam);
                                                        Match m = db.MatchSet.Add(new Match() { Teams = { teamsToAdd[i], teamsToAdd[j] }, Duration = d.MatchDuration, TournamentStage = tournyStage, Number = matchNumber++ });
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        teamsToAdd[i].Pool = KOPool;
                                        db.TeamSet.Add(teamsToAdd[i]);
                                        teamsToAdd[i].TimeIntervals = SameTimeInterval(teamsToAdd[i].PrevPool);
                                    }
                                }
                            }
                            teamsToAdd.Clear();
                            db.SaveChanges();
                        }
                        List<Team> teamsToClearUp = db.TeamSet.Where(x => x.Pool.Id == finalPool.Id).ToList();
                        foreach (Team team in teamsToClearUp)
                        {
                            db.TimeIntervalSet.RemoveRange(team.TimeIntervals);
                        }
                        db.TeamSet.RemoveRange(teamsToClearUp);
                        db.PoolSet.Remove(finalPool);
                        db.SaveChanges();
                    }

                }
            }
            db.SaveChanges();
            return true;
        }

        

        // Returns a list of TimeInterval, where each DateTime in a TimeInterval is equal to the highest time of the day (for the start of a day)
        // or lowest time of the day (for the end of the day) in the teams in a pool.
        public List<TimeInterval> SameTimeInterval(Pool p)
        {
            List<TimeInterval> intervals = new List<TimeInterval>();
            for (int i = 0; i < p.Division.Tournament.TimeIntervals.Count; i++)
            {
                DateTime dtStart = p.Teams.Select(x => x.TimeIntervals.ToArray()[i].StartTime).OrderByDescending(x => x.TimeOfDay).First();
                DateTime dtEnd = p.Teams.Select(x => x.TimeIntervals.ToArray()[i].EndTime).OrderBy(x => x.TimeOfDay).First();
                intervals.Add(new TimeInterval() { StartTime = dtStart, EndTime = dtEnd });
            }
            return intervals;
        }

       
    }
}
