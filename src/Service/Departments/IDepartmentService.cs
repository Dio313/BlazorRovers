using Common.ViewModels;
using Common.Wrapper;

namespace Service.Departments;

public interface IDepartmentService
{
    Task<ApiResultResponse<List<DepartmentVm>>> GetAllAsync();
    Task<ApiResultResponse<DepartmentVm>> GetByIdAsync(string id);
    Task<ApiResultResponse<DepartmentEmployeeVm>> GetDepartmentEmployeeAsync(string id);
    Task<ApiResultResponse<DepartmentVm>> CreateAsync(DepartmentVm model);
    Task<ApiResultResponse<DepartmentVm>> UpdateAsync(DepartmentVm model);
    Task DeleteAsync(string id);
}