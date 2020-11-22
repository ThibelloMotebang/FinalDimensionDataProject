using System;
using System.Collections.Generic;

namespace DimensionData.Models
{
    public partial class EmployeeDetails
    {
        public EmployeeDetails()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
            Employee = new HashSet<Employee>();
        }

        public int EmpId { get; set; }
        public int? Age { get; set; }
        public bool Attrition { get; set; }
        public int? DistanceFromHome { get; set; }
        public bool Over18 { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Email { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
    }
}
