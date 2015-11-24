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
        }

        [TestMethod()]
        public void ScheduleTest()
        {
            //Schedule the created match
            dynamic jsonResult = ((JsonResult)controller.Schedule(ID.MatchId, "17-11-2015 11:20:00", ID.FieldId)).Data;
            Assert.AreEqual("success", jsonResult.status);

            //Check to see if the changes have been made successfully
            jsonResult = ((JsonResult)controller.Details(ID.MatchId)).Data;
            Assert.AreEqual(60, jsonResult.Duration);
            Assert.AreEqual(DateTime.Parse("2015-11-17 11:20:00"), jsonResult.StartTime);

            //Schedule a new match, but to a non-existing match ID
            jsonResult = ((JsonResult)controller.Schedule(999999, "17-11-2015 11:20:00", ID.FieldId)).Data;
            Assert.AreEqual("error", jsonResult.status);

            //Schedule a new match, but with an invalid start time parameter
            jsonResult = ((JsonResult)controller.Schedule(999999, "17-11-2015 11:20:00", ID.FieldId)).Data;
            Assert.AreEqual("error", jsonResult.status);

            //Schedule a new match, but to a non-existing field
            jsonResult = ((JsonResult)controller.Schedule(999999, "17-11-2015 11:20:00", ID.FieldId)).Data;
            Assert.AreEqual("error", jsonResult.status);
        }

        [TestMethod()]
        public void EditTest()
        {
            //Edit the created match
            dynamic jsonResult = ((JsonResult)controller.Edit(ID.MatchId, "18-11-2015 12:00:00", 40)).Data;
            Assert.AreEqual("success", jsonResult.status);

            //Check to see if edits have been saved
            jsonResult = ((JsonResult)controller.Details(ID.MatchId)).Data;
            Assert.AreEqual(40, jsonResult.Duration);
            Assert.AreEqual(DateTime.Parse("2015-11-18 12:00:00"), jsonResult.StartTime);

            //Edit a match that does not exist
            jsonResult = ((JsonResult)controller.Edit(999999, "18-11-2015 12:00:00", 40)).Data;
            Assert.AreEqual("error", jsonResult.status);

            //Edit a match with an unvalid date as parameter
            jsonResult = ((JsonResult)controller.Edit(ID.MatchId, "NotADateTime", 40)).Data;
            Assert.AreEqual("error", jsonResult.status);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            //Delete the created pool
            dynamic jsonResult = ((JsonResult)controller.Delete(ID.MatchId)).Data;
            Assert.AreEqual("success", jsonResult.status);

            //Delete a pool that does not exist
            jsonResult = ((JsonResult)controller.Delete(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
        }
    }
}