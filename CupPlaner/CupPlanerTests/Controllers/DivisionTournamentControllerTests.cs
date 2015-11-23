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
    public class DivisionTournamentControllerTests
    {
        DivisionTournamentController controller = new DivisionTournamentController();
        CupDBContainer db = new CupDBContainer();

        [TestMethod()]
        public void Initialize()
        {
            Division d = db.DivisionSet.Find(ID.DivisionId);
            DivisionTournament dt = db.DivisionTournamentSet.Add(new DivisionTournament() { Division = d, TournamentStructure = TournamentStructure.RoundRobin });
            db.SaveChanges();
            ID.DivisionTournamentId = dt.Id;
        }

        [TestMethod()]
        public void DetailsTest()
        {
            //Find the creared division tournament
            dynamic jsonResult = ((JsonResult)controller.Details(ID.DivisionTournamentId)).Data;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual(ID.DivisionTournamentId, jsonResult.Id);
            Assert.AreEqual((TournamentStructure)0, jsonResult.TournamentStructure);

            //Find a division tournament that does not exist
            jsonResult = ((JsonResult)controller.Details(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            //Delete the created division
            dynamic jsonResult = ((JsonResult)controller.Delete(ID.DivisionTournamentId)).Data;
            Assert.AreEqual("success", jsonResult.status);

            //Delete a tournament that does not exist
            jsonResult = ((JsonResult)controller.Delete(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
        }
    }
}