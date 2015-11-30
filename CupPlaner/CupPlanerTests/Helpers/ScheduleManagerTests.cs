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
    public class ScheduleManagerTests
    {
        CupDBContainer db = new CupDBContainer();
        ScheduleManager sm = new ScheduleManager();

        [TestMethod()]
        public void scheduleMatchTest()
        {

            Tournament t = db.TournamentSet.Find(2);


            Assert.AreEqual(new Tuple<DateTime, Field>(t.TimeIntervals.First().StartTime, db.FieldSet.First()), sm.scheduleMatch(db.MatchSet.First()));
        }

        [TestMethod()]
        public void scheduleAllTest()
        {
            Tournament t = db.TournamentSet.Find(2);
            sm.scheduleAll(t);
        }
    }
}