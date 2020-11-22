using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DimensionData.Data;
using Dimension2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DimensionData.Controllers
{
    [Authorize(Roles = "Manager, Employee")]
    public class GraphsController : Controller
    {
        private readonly DimensionDataContext _context;

        public GraphsController(DimensionDataContext context)
        {
            _context = context;
        }

        public IActionResult TimeGraphs()
        {
            #region PieChart
            int female = _context.DataSet.Where(s => s.Gender == "female").Select(s => s).Count();
            int male = _context.DataSet.Where(s => s.Gender == "male").Select(s => s).Count();

            ViewBag.FEM = female;
            ViewBag.MA = male;
            #endregion PieChart

            

            #region PolarAreaChart
            (int, int, int) maritalStatTuple = (_context.DataSet.Where(m => m.MaritalStatus == "Single").Select(m => m).Count(),
                                                _context.DataSet.Where(m => m.MaritalStatus == "Divorced").Select(m => m).Count(),
                                                _context.DataSet.Where(m => m.MaritalStatus == "Married").Select(m => m).Count());

            ViewData["MARSTAT"] = maritalStatTuple;
            #endregion PolarAreaChart

            return View();
        }
    }
}