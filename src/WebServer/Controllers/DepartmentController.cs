using Common.ViewModels;
using Common.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Service.Departments;

namespace WebServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async  Task<ActionResult<ApiResultResponse<List<DepartmentVm>>>> GetDepartments()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResultResponse<DepartmentVm>>> GetDepartment(string id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }


        [HttpGet("{id}/employees")]
        public async Task<ActionResult<ApiResultResponse<DepartmentEmployeeVm>>> GetDepartmentEmployees(string id)
        {
            return Ok( await _service.GetDepartmentEmployeeAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResultResponse<DepartmentVm>>> CreateDepartment(DepartmentVm model)
        {
            return Ok(await _service.CreateAsync(model));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResultResponse<DepartmentVm>>> UpdateDepartment(DepartmentVm model)
        {
            return Ok(await _service.UpdateAsync(model));
        }


        [HttpDelete("{id}")]
        public async Task DeleteDepartment(string id)
        {
            await _service.DeleteAsync(id);
            
        }

    }
}