using Common.ViewModels;
using Common.Wrapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Service.Departments;

public class DepartmentService : IDepartmentService
{
    private readonly IRepository<Department> _departmentRepo;
    
    public DepartmentService(IRepository<Department> departmentRepo)
    {
        _departmentRepo = departmentRepo;
    }


    public async Task<ApiResultResponse<List<DepartmentVm>>> GetAllAsync()
    {
        try
        {
            var departments = await _departmentRepo.TableNoTracking
                .Select(b => new DepartmentVm()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description
                }).OrderBy(p => p.Name).ToListAsync();

            return new ApiResultResponse<List<DepartmentVm>>()
            {
                Data = departments
            };
        }
        catch (Exception ex)
        {
            return new ApiResultResponse<List<DepartmentVm>>()
            {
                IsSuccess = false,
                Errors = new List<string> {ex.Message}
            };
        }
       
    }

    public async Task<ApiResultResponse<DepartmentVm>> GetByIdAsync(string id)
    {
        try
        {
            var dept = await (from p in _departmentRepo.TableNoTracking
                where p.Id == id
                select new DepartmentVm
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                }).FirstOrDefaultAsync();

            return new ApiResultResponse<DepartmentVm>()
            {
                Data = dept
            };
        }
        catch (Exception ex)
        {
            return new ApiResultResponse<DepartmentVm>()
            {
                IsSuccess = false,
                Errors = new List<string> {ex.Message}
            };
        }
       
    }


    public async Task<ApiResultResponse<DepartmentEmployeeVm>> GetDepartmentEmployeeAsync(string id)
    {
        try
        {
            var deptEmployee = await (from p in _departmentRepo.TableNoTracking
                where p.Id == id
                select new DepartmentEmployeeVm
                {
                    Id = p.Id,
                    Name = p.Name,
                    Employees =  (List<EmployeeListVm>) p.Employees.Select(b => new EmployeeListVm()
                    {
                        Id = b.Id,
                        LastName = b.LastName,
                        FirstName = b.FirstName,
                        EmployeeNumber = b.EmployeeNumber,
                    })
                }).FirstOrDefaultAsync();

            return new ApiResultResponse<DepartmentEmployeeVm>()
            {
                Data = deptEmployee
            };
        }
        catch (Exception ex)
        {
            return new ApiResultResponse<DepartmentEmployeeVm>()
            {
                IsSuccess = false,
                Errors = new List<string> {ex.Message}
            };
        }
    }


    public async Task<ApiResultResponse<DepartmentVm>> CreateAsync(DepartmentVm model)
    {
        try
        {
            if (_departmentRepo.Table.Any(d => d.Name == model.Name))
            {
                return new ApiResultResponse<DepartmentVm>()
                {
                    Message =  $"A Department with the name '{model.Name}' already exist!",
                    IsSuccess = false
                };
            }

            var department = new Department()
            {
                Name = model.Name,
                Description = model.Description
            };

            await _departmentRepo.InsertAsync(department);
            
            return new ApiResultResponse<DepartmentVm>() {Data = model };
        }
        catch (Exception ex)
        {
            return new ApiResultResponse<DepartmentVm>()
            {
                IsSuccess = false,
                Errors = new List<string> {ex.Message}
            };
        }
        
    }


    public async Task<ApiResultResponse<DepartmentVm>> UpdateAsync(DepartmentVm model)
    {
        try
        {
            var departmentToUpdate = await _departmentRepo.GetByIdAsync(model.Id);

            if (departmentToUpdate is null)
            {
                return new ApiResultResponse<DepartmentVm>()
                {
                    Errors = new List<string> {"Bad Request"}
                };
            }

           
            departmentToUpdate.Name = model.Name;
            departmentToUpdate.Description = model.Description;

       
            await _departmentRepo.UpdateAsync(departmentToUpdate);

            return new ApiResultResponse<DepartmentVm>() { Data = model };
        }
        catch (Exception ex)
        {
            return new ApiResultResponse<DepartmentVm>()
            {
                IsSuccess = false,
                Errors = new List<string> {ex.Message}
            };
        }
        
    }



    public async Task DeleteAsync(string id)
    {
        var department = await _departmentRepo.GetByIdAsync(id);

        if (department is null)
        {
            throw new Exception("Bad Request");
        }

        await _departmentRepo.DeleteAsync(department);
            
    }
}