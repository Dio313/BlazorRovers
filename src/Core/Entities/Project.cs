namespace Core.Entities;

    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }

    }

