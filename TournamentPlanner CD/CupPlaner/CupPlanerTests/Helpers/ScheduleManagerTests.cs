﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void deleteScheduleTest()
        {
            sm.DeleteSchedule(17);
        }

        [TestMethod()]
        public void scheduleAllTest()
        {
            Tournament t = db.TournamentSet.Find(17);
            //bool succes = sm.scheduleAll(t, FieldSize.ElevenMan, 2);
            //if (!succes)
            //{
            sm.scheduleAll(t.Id, FieldSize.ElevenMan, sm.MinNumOfFields(t.Id, FieldSize.ElevenMan));
            //}
        }
    }
}