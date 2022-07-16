namespace Common.ViewModels;

public class DepartmentEmployeeVm
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<EmployeeListVm> Employees { get; set; } = new List<EmployeeListVm>();
}