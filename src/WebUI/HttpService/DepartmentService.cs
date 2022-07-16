using System.Net.Http.Json;
using Common.ViewModels;
using Common.Wrapper;

namespace WebUI.HttpService
{
    public interface IDepartmentService
    {
        Task<List<DepartmentVm>> GetDepartments();
        Task<DepartmentVm> GetDepartment(string id);
        Task<DepartmentEmployeeVm> GetDepartmentEmployee(string id);
        Task<DepartmentVm> CreateDepartment(DepartmentVm department);
        Task<DepartmentVm> UpdateDepartment(DepartmentVm department);
        Task<HttpResponseMessage> DeleteDepartment(string id);
    }

    public class DepartmentService : IDepartmentService
    {
        private readonly HttpClient _httpClient;
        private readonly string url = "/api/department";

        public DepartmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        
        public async Task<List<DepartmentVm>> GetDepartments()
        {
            var result = await _httpClient.GetAsync(url);
            var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<List<DepartmentVm>>>();
            return content?.Data;
        }


        public async Task<DepartmentVm> GetDepartment(string id)
        {
            var result = await _httpClient.GetAsync($"{url}/{id}");
            var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<DepartmentVm>>();
            return content?.Data;
        }

        public async Task<DepartmentEmployeeVm> GetDepartmentEmployee(string id)
        {
            var result = await _httpClient.GetAsync($"{url}/{id}/employees");
            var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<DepartmentEmployeeVm>>();
            return content?.Data;
        }

        
        public async Task<DepartmentVm> CreateDepartment(DepartmentVm department)
        {
            var result = await _httpClient.PostAsJsonAsync(url, department);
            var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<DepartmentVm>>();
            return content?.Data;
        }

        public async Task<DepartmentVm> UpdateDepartment(DepartmentVm department)
        {
            var result = await _httpClient.PutAsJsonAsync(url, department);
            var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<DepartmentVm>>();
            return content?.Data;
        }

        public async Task<HttpResponseMessage> DeleteDepartment(string id)
        {
            return await _httpClient.DeleteAsync($"{url}/{id}");
        }


    }
}