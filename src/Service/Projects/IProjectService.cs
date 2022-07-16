using Common.ViewModels;
using Common.Wrapper;

namespace Service.Projects;

public interface IProjectService
{
    Task<PagedResult<ProjectVm>> GetProjects(string name, int page);
    Task<ApiResultResponse<ProjectVm>> GetByIdAsync(string id);
    Task< ApiResultResponse<ProjectVm>> CreateAsync(ProjectVm model);
    Task<ApiResultResponse<ProjectVm>> UpdateAsync(ProjectVm model);
    Task DeleteAsync(string id);
}