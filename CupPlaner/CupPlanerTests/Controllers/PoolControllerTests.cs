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
    public class PoolControllerTests
    {
        PoolController controller = new PoolController();

        [TestMethod()]
        public void CreateTest()
        {
            //Create a new pool
            dynamic jsonResult = ((JsonResult)controller.Create("Pulje Test", ID.DivisionId)).Data;
            ID.PoolId = jsonResult.id;
            Assert.AreEqual("success", jsonResult.status);

            //Create a new pool, but to a non-existing division
            jsonResult = ((JsonResult)controller.Create("Pulje Test", 999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
        }

        [TestMethod()]
        public void DetailsTest()
        {
            //Find the created division
            dynamic jsonResult = ((JsonResult)controller.Details(ID.PoolId)).Data;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual(ID.PoolId, jsonResult.Id);
            Assert.AreEqual("Pulje Test", jsonResult.Name);
            
            //Find a pool that does not exist
            jsonResult = ((JsonResult)controller.Details(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
        }

        [TestMethod()]
        public void EditTest()
        {
            //Edit the created pool
            dynamic jsonResult = ((JsonResult)controller.Edit(ID.PoolId, "Pulje Test2", ID.DivisionId, new List<int>())).Data;
            Assert.AreEqual("success", jsonResult.status);

            //Check to see if edits have been saved
            jsonResult = ((JsonResult)controller.Details(ID.PoolId)).Data;
            Assert.AreEqual(ID.PoolId, jsonResult.Id);
            Assert.AreEqual("Pulje Test2", jsonResult.Name);

            //Edit a pool that does not exist
            jsonResult = ((JsonResult)controller.Edit(999999, "Pulje Test2", ID.DivisionId, new List<int>())).Data;
            Assert.AreEqual("error", jsonResult.status);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            //Delete the created pool
            dynamic jsonResult = ((JsonResult)controller.Delete(ID.PoolId)).Data;
            Assert.AreEqual("success", jsonResult.status);

            //Delete a pool that does not exist
            jsonResult = ((JsonResult)controller.Delete(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
        }
    }
}