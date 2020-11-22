using System;
using System.Collections.Generic;

namespace DimensionData.Models
{
    public partial class EmployeeEducation
    {
        public EmployeeEducation()
        {
            Employee = new HashSet<Employee>();
        }

        public int EduId { get; set; }
        public int? Education { get; set; }
        public string EducationField { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
