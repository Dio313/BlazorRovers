using System.Net.Http.Json;
using Common.ViewModels;
using Common.Wrapper;

namespace WebUI.HttpService;

public interface IEmployeeService
{
    Task<PagedResult<EmployeeListVm>> GetEmployees(string SearchTerm, string Page);
    Task<List<EmployeeListVm>> ListEmployees();
    Task<EmployeeVm> GetEmployeeById(string id);
    Task<EmployeeVm> CreateEmployee(EmployeeVm employee);
    Task<EmployeeVm> UpdateEmployee(EmployeeVm employee);
    Task<HttpResponseMessage> DeleteEmployee(string id);
}

public class EmployeeService : IEmployeeService
{
    private readonly HttpClient _httpClient;
    private readonly string url = "/api/employee";

    public EmployeeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResult<EmployeeListVm>> GetEmployees(string SearchTerm, string Page)
    {
        var _EmpUrl = url + "/?name=" + SearchTerm + "&page=" + Page;
        return await _httpClient.GetFromJsonAsync<PagedResult<EmployeeListVm>>(_EmpUrl);
    }

    public async Task<List<EmployeeListVm>> ListEmployees()
    {
        var result = await _httpClient.GetAsync(url + "/lists");
        var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<List<EmployeeListVm>>>();
        return content?.Data;
    }

    public async Task<EmployeeVm> GetEmployeeById(string id)
    {
        var result = await _httpClient.GetAsync($"{url}/{id}");
        var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<EmployeeVm>>();
        return content?.Data;
    }


    public async Task<EmployeeVm> CreateEmployee(EmployeeVm employee)
    {
        var result = await _httpClient.PostAsJsonAsync(url, employee);
        var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<EmployeeVm>>();
        return content?.Data;
    }

    public async Task<EmployeeVm> UpdateEmployee(EmployeeVm employee)
    {
        var result = await _httpClient.PutAsJsonAsync(url, employee);
        var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<EmployeeVm>>();
        return content?.Data;
    }

    public async Task<HttpResponseMessage> DeleteEmployee(string id)
    {
        return await _httpClient.DeleteAsync($"{url}/{id}");
    }

}