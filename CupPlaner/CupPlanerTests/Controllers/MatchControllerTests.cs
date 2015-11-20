using Microsoft.VisualStudio.TestTools.UnitTesting;
using CupPlaner.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CupPlaner.Controllers.Tests
{
    [TestClass()]
    public class MatchControllerTests
    {
        MatchController controller = new MatchController();

        [TestMethod()]
        public void CreateTest()
        {
            //Create a new match controller
            Match m = controller.Create(ID.TeamId, ID.TeamId + 1, ID.TournamentStageId);
            ID.MatchId = m.Id;
            Assert.AreEqual(ID.TeamId, m.Teams.ToArray()[0].Id);
            Assert.AreEqual(ID.TeamId + 1, m.Teams.ToArray()[1].Id);

            //Create a new match, but to one or two non-existing teams
            try
            {
                m = controller.Create(999999, 999999, ID.TournamentStageId);
                Assert.Fail();
            }
            catch { }

            //Create a new match, but to a non-existing tournament stage
            try
            {
                m = controller.Create(ID.TeamId, ID.TeamId + 1, 999999);
                Assert.Fail();
            }
            catch { }

        }

        [TestMethod()]
        public void DetailsTest()
        {
            //Find the created match
            dynamic jsonResult = ((JsonResult)controller.Details(ID.MatchId)).Data;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual(ID.MatchId, jsonResult.Id);

            //Find a match that does not exist
            jsonResult = ((JsonResult)controller.Details(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("Could not find match", jsonResult.message);
        }

        [TestMethod()]
        public void ScheduleTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }
    }
}