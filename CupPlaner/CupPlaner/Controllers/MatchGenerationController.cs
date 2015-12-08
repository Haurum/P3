using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CupPlaner.Controllers
{
    public class MatchGenerationController : Controller
    {
        // GET: MatchGeneration
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GenerateGroupStage(int tournamentID)
        {

        }
    }
}