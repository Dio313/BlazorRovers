using Common.AuthViewModels;
using Common.ViewModels;
using Common.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Service.Roles;

namespace WebServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _service;
  
    public RoleController(IRoleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResultResponse<List<EmployeeListVm>>>> GetUsers()
    {
        return Ok(await _service.GetAllUsers());
    }

    [HttpGet("roles")]
    public async Task<ActionResult<ApiResultResponse<List<RoleVm>>>> GetRoles()
    {
        return Ok(await _service.GetAllRoles());
    }



    [HttpPost("assignRole")]
    public async Task<ActionResult<ApiResultResponse<EditRoleVm>>> AssignRole(EditRoleVm request)
    {
        return Ok(await _service.AssignRole(request));
    }



    [HttpPost("removeRole")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResultResponse<EditRoleVm>>> RemoveRole(EditRoleVm request)
    {
        return Ok(await _service.RemoveRole(request));
    }
}