using Common.AuthViewModels;
using Common.ViewModels;
using Common.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Service.Employees;

namespace WebServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;


    public EmployeeController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public Task<PagedResult<EmployeeListVm>>  GetAllEmployees(string name, int page)
    {
        return _service.GetEmployees(name, page);
        
    }

    [HttpGet("lists")]
    public async Task<ActionResult<ApiResultResponse<List<EmployeeListVm>>>> GetListEmployees()
    {
        return Ok(await _service.GetListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResultResponse<EmployeeVm>>> GetEmployee(string id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResultResponse<EmployeeVm>>> CreateEmployee(EmployeeVm model)
    {
        return Ok(await _service.CreateAsync(model));
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginVm model)
    {
        return Ok(await _service.Login(model));
    }

   
    [HttpPut("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ChangePasswordVm model)
    {
        return Ok(await _service.ManagePassword(model));
    }

    [HttpPut]
    public async Task<ActionResult<ApiResultResponse<EmployeeVm>>> UpdateEmployee(EmployeeVm model)
    {
        return Ok(await _service.UpdateAsync(model));
    }

    [HttpDelete("{id}")]
    public async Task DeleteEmployee(string id)
    {
        await _service.DeleteAsync(id);

    }
}