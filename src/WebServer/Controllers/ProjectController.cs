using Common.ViewModels;
using Common.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Service.Projects;

namespace WebServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectController(IProjectService service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<PagedResult<ProjectVm>> GetAllProjects(string name, int page)
    {
        return _service.GetProjects(name, page);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResultResponse<ProjectVm>>> GetProject(string id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }


    [HttpPost]
    public async Task<ActionResult<ApiResultResponse<ProjectVm>>> CreateProject(ProjectVm model)
    {
        return Ok(await _service.CreateAsync(model));
    }

    [HttpPut]
    public async Task<ActionResult<ApiResultResponse<ProjectVm>>> UpdateProject(ProjectVm model)
    {
        return Ok(await _service.UpdateAsync(model));
    }

    [HttpDelete("{id}")]
    public async Task DeleteProject(string id)
    {
        await _service.DeleteAsync(id);

    }
}