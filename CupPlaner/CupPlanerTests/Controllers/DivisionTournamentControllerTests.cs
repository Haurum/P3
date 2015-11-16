﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod()]
        public void CreateTest()
        {
            //Create a new division tournament
            dynamic jsonResult = ((JsonResult)controller.Create(0, ID.DivisionId)).Data;
            ID.DivisionTournamentId = jsonResult.id;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual("New division tournament added", jsonResult.message);

            //Create a new division tournament, but to a non-existing division
            jsonResult = ((JsonResult)controller.Create(0, 999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("New division tournament not added", jsonResult.message);
        }

        [TestMethod()]
        public void DetailsTest()
        {
            //Find the creared division tournament
            dynamic jsonResult = ((JsonResult)controller.Details(ID.DivisionTournamentId)).Data;
            Assert.AreEqual(ID.DivisionTournamentId, jsonResult.Id);
            Assert.AreEqual((TournamentStructure)0, jsonResult.TournamentStructure);

            //Find a division tournament that does not exist
            jsonResult = ((JsonResult)controller.Details(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("Could not find division tournament", jsonResult.message);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            //Delete the created division
            dynamic jsonResult = ((JsonResult)controller.Delete(ID.DivisionTournamentId)).Data;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual("Division tournament deleted", jsonResult.message);

            //Delete a tournament that does not exist
            jsonResult = ((JsonResult)controller.Delete(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("Division tournament not deleted", jsonResult.message);
        }
    }
}