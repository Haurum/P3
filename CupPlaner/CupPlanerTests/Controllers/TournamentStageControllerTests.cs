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
            dynamic jsonResult = ((JsonResult)controller.Details(ID.TournamentStageId)).Data;
            Assert.AreEqual(ID.TournamentStageId, jsonResult.Id);
            Assert.AreEqual(0, jsonResult.TournamentStructure);

            jsonResult = ((JsonResult)controller.Details(ID.TournamentStageId)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("Could not find division tournament", jsonResult.message);

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