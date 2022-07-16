using System.Net.Http.Json;
using Common.AuthViewModels;
using Common.ViewModels;
using Common.Wrapper;

namespace WebUI.HttpService;

public interface IRoleService
{
    Task<List<EmployeeListVm>> ListUsers();
    Task<List<RoleVm>> ListRoles();
    Task<EditRoleVm> AssignRole(EditRoleVm role);
    Task<EditRoleVm> RemoveRole(EditRoleVm role);
}

public class RoleService : IRoleService
{
    private readonly HttpClient _httpClient;
    private readonly string url = "/api/role";

    public RoleService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EmployeeListVm>> ListUsers()
    {
        var result = await _httpClient.GetAsync(url);
        var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<List<EmployeeListVm>>>();
        return content?.Data;
    }

    public async Task<List<RoleVm>> ListRoles()
    {
        var result = await _httpClient.GetAsync(url + "/roles");
        var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<List<RoleVm>>>();
        return content?.Data;
    }

    public async Task<EditRoleVm> AssignRole(EditRoleVm role)
    {
        var result = await _httpClient.PostAsJsonAsync(url + "/assignRole", role);
        var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<EditRoleVm>>();
        return content?.Data;
    }

    public async Task<EditRoleVm> RemoveRole(EditRoleVm role)
    {
        var result = await _httpClient.PostAsJsonAsync(url + "/removeRole", role);
        var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<EditRoleVm>>();
        return content?.Data;
    }
}