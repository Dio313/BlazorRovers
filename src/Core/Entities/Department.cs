namespace Core.Entities;

    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Employee> Employees { get; set; } = new List<Employee>();

    }

