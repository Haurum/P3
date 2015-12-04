using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CupPlaner.Controllers;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace CupPlaner.Helpers
{
    public class ExcelExport
    {
        CupDBContainer db = new CupDBContainer();

        Excel.Application xlApp = new Excel.Application();
        Excel.Workbook xlWorkbook = new Excel.Workbook();

        Excel.Worksheet xlCup = new Excel.Worksheet();
        Excel.Worksheet xlSpillesteder = new Excel.Worksheet();
        Excel.Worksheet xlRækker = new Excel.Worksheet();
        Excel.Worksheet xlPuljer = new Excel.Worksheet();
        Excel.Worksheet xlHold = new Excel.Worksheet();
        Excel.Worksheet xlKampe = new Excel.Worksheet();

    }
}