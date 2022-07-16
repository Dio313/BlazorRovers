using Common.AuthViewModels;
using Common.ViewModels;
using Common.Wrapper;

namespace Service.Roles;

public interface IRoleService
{
    Task<ApiResultResponse<List<RoleVm>>> GetAllRoles();
    Task<ApiResultResponse<List<EmployeeListVm>>> GetAllUsers();
    Task<ApiResultResponse<EditRoleVm>> AssignRole(EditRoleVm request);
    Task<ApiResultResponse<EditRoleVm>> RemoveRole(EditRoleVm request);
}