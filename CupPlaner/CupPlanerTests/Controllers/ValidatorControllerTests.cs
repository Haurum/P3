using Microsoft.VisualStudio.TestTools.UnitTesting;
using CupPlaner.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;

namespace CupPlaner.Controllers.Tests
{
    [TestClass()]
    public class ValidatorControllerTests
    {
        ValidatorController controller = new ValidatorController();
        CupDBContainer db = new CupDBContainer();

        [TestMethod()]
        public void IsScheduleReadyTest()
        {
            // Test tournament is not scheduled
            dynamic jsonResult = ((JsonResult)controller.IsScheduleReady(ID.TournamentId)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("turnering ikke klar", jsonResult.message);

            //Find a tournament that does not exist
            jsonResult = ((JsonResult)controller.IsScheduleReady(999999)).Data;
            Assert.AreEqual("error", jsonResult.status);
            Assert.AreEqual("fejl i programmet", jsonResult.message);
        }
    }
}