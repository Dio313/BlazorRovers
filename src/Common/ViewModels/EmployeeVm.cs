namespace Common.ViewModels
{
    public class EmployeeVm 
    {
        public string Id { get; set; }
        public string EmployeeNumber { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public DateTime HireDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public DesignationVm Designation { get; set; }
        public GenderVm Gender { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        
        public string ImageBase64 { get; set; }

        
    }

    public class EmployeeListVm
    {
        public string Id { get; set; }
        public string EmployeeNumber { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Picture { get; set; }
        public string Phone { get; set; }
        public string DepartmentName { get; set; }
    }


    public enum GenderVm
    {
        Female,
        Male
    }

    public enum DesignationVm
    {
        Trainee,
        Junior,
        Senior,
        Expert,
        Manager
    }
   


}
