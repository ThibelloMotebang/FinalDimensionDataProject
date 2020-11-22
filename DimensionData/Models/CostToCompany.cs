using System;
using System.Collections.Generic;

namespace DimensionData.Models
{
    public partial class CostToCompany
    {
        public CostToCompany()
        {
            Employee = new HashSet<Employee>();
        }

        public int MonthlyIncomeId { get; set; }
        public double? HourlyRate { get; set; }
        public double? MonthlyRate { get; set; }
        public double? MonthlyIncome { get; set; }
        public double? DailyRate { get; set; }
        public bool OverTime { get; set; }
        public double? PercentSalaryHike { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
