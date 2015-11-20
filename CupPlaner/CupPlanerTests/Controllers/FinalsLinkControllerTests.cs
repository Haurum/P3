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
    public class FinalsLinkControllerTests
    {
        FinalsLinkController controller = new FinalsLinkController();

        [TestMethod()]
        public void CreateTest()
        {
            //Create a new finals link
            dynamic jsonResult = ((JsonResult)controller.Create(1, 1, ID.DivisionId)).Data;
            Assert.AreEqual("success", jsonResult.status);
            ID.FinalsLinkId = jsonResult.id;

            //Create a new finals link, but to an invalid division
            jsonResult = ((JsonResult)controller.Create(1, 1, 999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
        }

        [TestMethod()]
        public void DetailsTest()
        {
            dynamic jsonResult = ((JsonResult)controller.Details(ID.FinalsLinkId)).Data;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual(ID.FinalsLinkId, jsonResult.id);
            Assert.AreEqual(1, jsonResult.Finalstage);
            Assert.AreEqual(1, jsonResult.PoolPlacement);
            Assert.AreEqual(ID.DivisionId, jsonResult.Division_Id.Id);

            //Find a field that does not exist
            jsonResult = ((JsonResult)controller.Details(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("Could not find finals link", jsonResult.message);
        }

        [TestMethod()]
        public void EditTest()
        {
            //Edit the created finals link
            dynamic jsonResult = ((JsonResult)controller.Edit(ID.FinalsLinkId, 2,2,ID.DivisionId)).Data;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual("Finals link edited", jsonResult.message);

            //Check to see if edits have been saved
            jsonResult = ((JsonResult)controller.Details(ID.FinalsLinkId)).Data;
            Assert.AreEqual(2, jsonResult.Finalstage);
            Assert.AreEqual(2, jsonResult.PoolPlacement);
            Assert.AreEqual(ID.DivisionId, jsonResult.Division_Id.Id);

            //Edit a finals link that does not exist
            jsonResult = ((JsonResult)controller.Edit(999999, 2, 2, ID.DivisionId)).Data;
            Assert.AreEqual("error", jsonResult.status);

            //Edit a finals link but assigned to a non-existing division
            jsonResult = ((JsonResult)controller.Edit(ID.FinalsLinkId, 2, 2, 999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            //Delete the created finals link
            dynamic jsonResult = ((JsonResult)controller.Delete(ID.FinalsLinkId)).Data;
            Assert.AreEqual("success", jsonResult.status);
            Assert.AreEqual("Finals link deleted", jsonResult.message);

            //Delete a finals link that does not exist
            jsonResult = ((JsonResult)controller.Delete(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("Finals link not deleted", jsonResult.message);
        }
    }
}