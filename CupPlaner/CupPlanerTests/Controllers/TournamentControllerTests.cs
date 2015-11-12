using Microsoft.VisualStudio.TestTools.UnitTesting;
using CupPlaner.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CupPlaner.Controllers.Tests
{
    [TestClass()]
    public class TournamentControllerTests
    {
        TournamentController controller = new TournamentController();

        [TestMethod()]
        public void CreateTest()
        {
            
            dynamic result = controller.Create("TestName", "TestPassword", new List<DateTime>() { DateTime.Now }, new List<DateTime>() { DateTime.Now.AddMinutes(60) }) as JsonResult;
            Debug.Write("xxcc");
            //var status = JsonConvert.DeserializeObject(result.status);
            //Assert.AreEqual(status, "success");
        }

        [TestMethod()]
        public void DetailsTest()
        {
            Trace.Write("xxcc");
            Assert.IsTrue(true);
            //Assert.Fail();
        }

        [TestMethod()]
        public void IdFromPassTest()
        {
            Trace.Write("xxcc");
            //Assert.Fail();
        }

        [TestMethod()]
        public void EditTest()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            //Assert.Fail();
        }
    }
}