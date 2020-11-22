using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dimension2.Data;
using DimensionData.Models;
using Microsoft.AspNetCore.Authorization;
using DimensionData.Areas;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace DimensionData.Controllers
{
    public class DataSetsController : Controller
    {
        private readonly DimensionDataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DataSetsController(DimensionDataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: DataSets
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> Index(string sortOrder,string currentFilter,string searchString,int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["AgeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "age_desc" : "";
            ViewData["AttritionSortParm"] = sortOrder == "Attrition" ? "attrition_desc" : "Attrition";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var dataSets = from s in _context.DataSet
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                dataSets = dataSets.Where(s => s.Age.Contains(searchString)
                                       || s.Attrition.Contains(searchString)
                                       || s.JobRole.Contains(searchString)
                                       || s.Gender.Contains(searchString)
                                       || s.EmployeeNumber.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "age_desc":
                    dataSets = dataSets.OrderByDescending(s => s.Age);
                    break;
                case "Attrition":
                    dataSets = dataSets.OrderBy(s => s.Attrition);
                    break;
                case "attrition_desc":
                    dataSets = dataSets.OrderByDescending(s => s.Attrition);
                    break;
                default:
                    dataSets = dataSets.OrderBy(s => s.Age);
                    break;
            }

            int pageSize = 10;
            var pagedList = await PaginatedList<DataSet>.CreateAsync(dataSets.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(pagedList);
        }

        // GET: DataSets/Details/5
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSet = await _context.DataSet
                .FirstOrDefaultAsync(m => m.EmployeeNumber == id);
            if (dataSet == null)
            {
                return NotFound();
            }

            return View(dataSet);
        }

        // GET: DataSets/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataSets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("Age,Attrition,BusinessTravel,DailyRate,Department,DistanceFromHome,Education,EducationField,EmployeeCount,EmployeeNumber,EnvironmentSatisfaction,Gender,HourlyRate,JobInvolvement,JobLevel,JobRole,JobSatisfaction,MaritalStatus,MonthlyIncome,MonthlyRate,NumCompaniesWorked,Over18,OverTime,PercentSalaryHike,PerformanceRating,RelationshipSatisfaction,StandardHours,StockOptionLevel,TotalWorkingYears,TrainingTimesLastYear,WorkLifeBalance,YearsAtCompany,YearsInCurrentRole,YearsSinceLastPromotion,YearsWithCurrManager")] DataSet dataSet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dataSet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataSet);
        }

        // GET: DataSets/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSet = await _context.DataSet.FindAsync(id);
            if (dataSet == null)
            {
                return NotFound();
            }
            return View(dataSet);
        }

        // POST: DataSets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Age,Attrition,BusinessTravel,DailyRate,Department,DistanceFromHome,Education,EducationField,EmployeeCount,EmployeeNumber,EnvironmentSatisfaction,Gender,HourlyRate,JobInvolvement,JobLevel,JobRole,JobSatisfaction,MaritalStatus,MonthlyIncome,MonthlyRate,NumCompaniesWorked,Over18,OverTime,PercentSalaryHike,PerformanceRating,RelationshipSatisfaction,StandardHours,StockOptionLevel,TotalWorkingYears,TrainingTimesLastYear,WorkLifeBalance,YearsAtCompany,YearsInCurrentRole,YearsSinceLastPromotion,YearsWithCurrManager")] DataSet dataSet)
        {
            if (id != dataSet.EmployeeNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataSet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataSetExists(dataSet.EmployeeNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dataSet);
        }

        // GET: DataSets/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSet = await _context.DataSet
                .FirstOrDefaultAsync(m => m.EmployeeNumber == id);
            if (dataSet == null)
            {
                return NotFound();
            }

            return View(dataSet);
        }

        // POST: DataSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dataSet = await _context.DataSet.FindAsync(id);
            _context.DataSet.Remove(dataSet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataSetExists(string id)
        {
            return _context.DataSet.Any(e => e.EmployeeNumber == id);
        }
    }
}
