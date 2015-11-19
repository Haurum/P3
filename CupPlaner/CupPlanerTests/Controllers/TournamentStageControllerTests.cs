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
    public class TournamentStageControllerTests
    {
        TournamentStageController controller = new TournamentStageController();
        List<int> MatchIDs = new List<int>() { 0, 1, 2 };

        [TestMethod()]
        public void CreateTest()
        {
            //Create a new tournament stage
            TournamentStage ts = controller.Create(ID.DivisionTournamentId, ID.PoolId);
            ID.TournamentStageId = ts.Id;
            Assert.AreEqual(ID.DivisionTournamentId, ts.DivisionTournament.Id);
            Assert.AreEqual(ID.PoolId, ts.Pool.Id);

            //Create a new tournament stage, but to a non-existing pool
            try
            {
                ts = controller.Create(ID.DivisionTournamentId, 999999);
                Assert.Fail();
            }
            catch { }

            //Create a new tournament stage, but to a non-existing division tournament
            try
            {
                ts = controller.Create(999999, ID.PoolId);
                Assert.Fail();
            }
            catch { }
        }

        [TestMethod()]
        public void DetailsTest()
        {
            //Find the created tournament stage
            dynamic jsonResult = ((JsonResult)controller.Details(ID.TournamentStageId)).Data;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual(ID.TournamentStageId, jsonResult.Id);
            Assert.AreEqual(TournamentStructure.RoundRobin, jsonResult.TournamentStructure);

            //Find a tournament stage that does not exist
            jsonResult = ((JsonResult)controller.Details(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("Could not find tournament stage", jsonResult.message);
        }

        [TestMethod()]
        public void EditTest()
        {
            //Edit the created tournament stage
            dynamic jsonResult = ((JsonResult)controller.Edit(ID.TournamentStageId, MatchIDs)).Data;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual("Tournament stage edited", jsonResult.message);

            //Check to see if the edits have been saved
            jsonResult = ((JsonResult)controller.Details(ID.TournamentStageId)).Data;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual(MatchIDs, jsonResult.Matches);

            //Edit a tournament stage that does not exist
            jsonResult = ((JsonResult)controller.Edit(999999, MatchIDs)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("Tournament stage not edited", jsonResult.message);

            //Edit a tournament stage passing null as the second parameter
            jsonResult = ((JsonResult)controller.Edit(ID.TournamentStageId, null)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("Tournament stage not edited", jsonResult.message);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            //Delete the created tournament
            dynamic jsonResult = ((JsonResult)controller.Delete(ID.TournamentStageId)).Data;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual("Tournament stage deleted", jsonResult.message);

            //Delete a tournament that does not exist
            jsonResult = ((JsonResult)controller.Delete(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("Tournament stage not deleted", jsonResult.message);
        }
    }
}