using System.Net.Http.Json;
using Common.ViewModels;
using Common.Wrapper;

namespace WebUI.HttpService
{
    public interface IProjectService
    {
        Task<PagedResult<ProjectVm>> GetProjects(string SearchTerm, string Page);
        Task<List<ProjectVm>> ListProjects();
        Task<ProjectVm> GetProject(string id);
        Task<ProjectVm> CreateProject(ProjectVm project);
        Task<ProjectVm> UpdateProject(ProjectVm project);
        Task<HttpResponseMessage> DeleteProject(string id);
    }

    public class ProjectService : IProjectService
    {
        private readonly HttpClient _httpClient;

        private readonly string url = "/api/project";

        public ProjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedResult<ProjectVm>> GetProjects(string SearchTerm, string Page)
        {
            var _proUrl = url + "/?name=" + SearchTerm + "&page=" + Page;
            return await _httpClient.GetFromJsonAsync<PagedResult<ProjectVm>>(_proUrl);
        }

        public async Task<List<ProjectVm>> ListProjects()
        {
            var result = await _httpClient.GetAsync(url + "/lists");
            var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<List<ProjectVm>>>();
            return content?.Data;
        }
        public async Task<ProjectVm> GetProject(string id)
        {
            var result = await _httpClient.GetAsync($"{url}/{id}");
            var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<ProjectVm>>();
            return content?.Data;
        }

        public async Task<ProjectVm> CreateProject(ProjectVm project)
        {
            var result = await _httpClient.PostAsJsonAsync(url, project);
            var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<ProjectVm>>();
            return content?.Data;
        }

        public async Task<ProjectVm> UpdateProject(ProjectVm project)
        {
            var result = await _httpClient.PutAsJsonAsync(url, project);
            var content = await result.Content.ReadFromJsonAsync<ApiResultResponse<ProjectVm>>();
            return content?.Data;
        }

        public async Task<HttpResponseMessage> DeleteProject(string id)
        {
            return await _httpClient.DeleteAsync($"{url}/{id}");
        }
    }
}
