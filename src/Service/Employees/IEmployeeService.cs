using Common.AuthViewModels;
using Common.ViewModels;
using Common.Wrapper;

namespace Service.Employees;

public interface IEmployeeService
{
    Task<LoginResponse> Login(LoginVm request);
    Task<ResetPasswordResponse> ManagePassword(ChangePasswordVm request);
    Task<PagedResult<EmployeeListVm>> GetEmployees(string name, int page);
    Task<ApiResultResponse<List<EmployeeListVm>>> GetListAsync();
    Task<ApiResultResponse<EmployeeVm>> GetByIdAsync(string id);
    Task<ApiResultResponse<EmployeeVm>> CreateAsync(EmployeeVm model);
    Task<ApiResultResponse<EmployeeVm>> UpdateAsync(EmployeeVm model);
    Task DeleteAsync(string id);
}