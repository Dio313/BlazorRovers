using Core.Enum;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

    public class Employee : IdentityUser
    {

        public string EmployeeNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime HireDate { get; set; }
        public string Picture { get; set; }
        public Designation Designation { get; set; }
        public Gender Gender { get; set; }
        public string DepartmentId { get; set; }
        public Department Department { get; set; }
        public IList<Project> Projects { get; set; } = new List<Project>();
    }



