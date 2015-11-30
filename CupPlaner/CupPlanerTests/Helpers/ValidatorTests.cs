using Microsoft.VisualStudio.TestTools.UnitTesting;
using CupPlaner.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CupPlaner.Helpers.Tests
{
    [TestClass()]
    public class ValidatorTests
    {
        Validator v = new Validator();
        CupDBContainer db = new CupDBContainer();
        [TestMethod()]
        public void areTeamsFreeTest()
        {
            Tournament t = db.TournamentSet.Find(2);

            Assert.AreEqual(true, v.areTeamsFree(db.MatchSet.First(), t.TimeIntervals.First().StartTime));
        }
    }
}