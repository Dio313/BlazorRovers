using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.AuthViewModels;
using Common.ViewModels;
using Common.Wrapper;
using Core.Entities;
using Core.Enum;
using Data.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Service.Employees;

public class EmployeeService : IEmployeeService
{
    private readonly UserManager<Employee> _employeeRepo;
    private readonly IFileService _fileService;
    private readonly SignInManager<Employee> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _env;

    public EmployeeService(UserManager<Employee> employeeRepo, IFileService fileService,
        SignInManager<Employee> signInManager,
        IConfiguration configuration,
        IHostEnvironment env)
    {
        _employeeRepo = employeeRepo;
        _fileService = fileService;
        _signInManager = signInManager;
        _configuration = configuration;
        _env = env;
    }


    public async Task<LoginResponse> Login(LoginVm request)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);

            if (!result.Succeeded)
            {
                return new LoginResponse() { Successful = false, Error = "Username or Password are invalid." };
            }

            var user = await _signInManager.UserManager.FindByNameAsync(request.Username);
            var roles = await _signInManager.UserManager.GetRolesAsync(user);

            var claims = new List<Claim> {new Claim(ClaimTypes.Name, request.Username)};
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new LoginResponse() { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
        catch (Exception ex)
        {
            return new LoginResponse()
            {
                Successful =  false,
                Error = ex.Message
            };
        }
        
    }


    public async Task<ResetPasswordResponse> ManagePassword(ChangePasswordVm request)
    {
        try
        {
            var user = await _employeeRepo.FindByNameAsync(request.Username);
            if (user == null)
            {
                return new ResetPasswordResponse()
                {
                    IsSuccess = false, Errors = new List<string> {"User not found"}
                };
                    
            }
            else
            {
                var identityResult = await this._employeeRepo.ChangePasswordAsync(
                    user,
                    request.Password,
                    request.NewPassword);

                if (identityResult.Succeeded)
                {
                    return new ResetPasswordResponse()
                    {
                        IsSuccess = true,
                        Message = "Password Changed"
                    };
                }
                else
                {
                    return new ResetPasswordResponse()
                    {
                        Errors =  identityResult.Errors.Select(x => x.Description)
                    };
                }
            }
        }
        catch (Exception ex)
        {
            return new ResetPasswordResponse()
            {
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public Task<PagedResult<EmployeeListVm>> GetEmployees(string name, int page)
    {
        int pageSize = 10;

        if (name is not null)
        {
            return Task.FromResult( _employeeRepo.Users.AsNoTracking()
                .Where(p => p.LastName.Contains(name) || 
                            p.FirstName.Contains(name) ||
                            p.EmployeeNumber.Contains(name))
                .Select(b => new EmployeeListVm()
                {
                    Id = b.Id,
                    LastName = b.LastName,
                    FirstName = b.FirstName,
                    EmployeeNumber = b.EmployeeNumber,
                    Phone = b.PhoneNumber,
                    Picture = b.Picture,
                    Username = b.UserName,
                    DepartmentName = b.Department.Name,
                       
                }).OrderBy(e => e.LastName).GetPaged( page, pageSize));
        }

        return Task.FromResult( _employeeRepo.Users.AsNoTracking()
            .Select(b => new EmployeeListVm()
            {
                Id = b.Id,
                LastName = b.LastName,
                FirstName = b.FirstName,
                EmployeeNumber = b.EmployeeNumber,
                Phone = b.PhoneNumber,
                Picture = b.Picture,
                Username = b.UserName,
                DepartmentName = b.Department.Name,
                    
            }).OrderBy(e => e.LastName).GetPaged(page, pageSize));
    }


    public async Task<ApiResultResponse<List<EmployeeListVm>>> GetListAsync()
    {
        var employees = await _employeeRepo.Users.AsNoTracking()
            .Select(b => new  EmployeeListVm()
            {
                Id = b.Id,
                LastName = b.LastName,
                FirstName = b.FirstName,
                Username = b.UserName,
                EmployeeNumber = b.EmployeeNumber
                    
            }).OrderBy(e => e.LastName).ToListAsync();

        return new ApiResultResponse<List<EmployeeListVm>>()
        {
            Data = employees,
            IsSuccess = true
        };
    }


    public async Task<ApiResultResponse<EmployeeVm>> GetByIdAsync(string id)
    {
        var employee = await (from e in _employeeRepo.Users
                .Include(e => e.Department)
                    
            where e.Id == id
            select new EmployeeVm
            {
                EmployeeNumber = e.EmployeeNumber,
                FirstName = e.FirstName,
                Username = e.UserName,
                Id = e.Id,
                LastName = e.LastName,
                Phone = e.PhoneNumber,
                Picture = e.Picture,
                HireDate = e.HireDate,
                DepartmentName = e.Department.Name,
                Designation = (DesignationVm)e.Designation,
                Gender = (GenderVm)e.Gender,
                Email = e.Email
            }).FirstOrDefaultAsync();

        return new ApiResultResponse<EmployeeVm>()
        {
            Data = employee,
            IsSuccess = true
        };
    }


    public async Task<ApiResultResponse<EmployeeVm>> CreateAsync(EmployeeVm model)
    {
        if (_employeeRepo.Users.Any(d => d.EmployeeNumber == model.EmployeeNumber))
        {
            return new ApiResultResponse<EmployeeVm>()
            {
                Message =  $"A Employee With number '{model.EmployeeNumber}' already exist!",
                IsSuccess = false
            };
        }

        if (!string.IsNullOrWhiteSpace(model.Picture))
        {
            var imagePicture = Convert.FromBase64String(model.Picture);
            model.Picture = await _fileService.SaveFile(imagePicture, "jpg", "employees");
        }

        var employee = new Employee
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.Username,
            EmployeeNumber =model.EmployeeNumber,
            Picture = model.Picture,
            HireDate = model.HireDate,
            Email = model.Email,
            PhoneNumber = model.Phone,
            Designation = (Designation) model.Designation,
            Gender = (Gender) model.Gender,
            DepartmentId = model.DepartmentId,
        };

        await _employeeRepo.CreateAsync(employee, model.Password);

        await _employeeRepo.AddToRoleAsync(employee, "Staff");

        return new ApiResultResponse<EmployeeVm>() { Data = model };
    }


    public async Task<ApiResultResponse<EmployeeVm>> UpdateAsync(EmployeeVm model)
    {
        var employeeToUpdate = await _employeeRepo.FindByIdAsync(model.Id);

        if (!string.IsNullOrWhiteSpace(model.Picture))
        {
            var imagePicture = Convert.FromBase64String(model.Picture);
            employeeToUpdate.Picture = await _fileService.EditFile(imagePicture,
                "jpg", "employees", employeeToUpdate.Picture);
        }

        if (employeeToUpdate is null)
        {
            return new ApiResultResponse<EmployeeVm>()
            {
                Errors = new List<string> {"Bad Request"}
            };
        }

        employeeToUpdate.UserName = model.Username;
        employeeToUpdate.DepartmentId = model.DepartmentId;
        employeeToUpdate.Designation = (Designation) model.Designation;
        employeeToUpdate.Email = model.Email;
        employeeToUpdate.EmployeeNumber = model.EmployeeNumber;
        employeeToUpdate.FirstName = model.FirstName;
        employeeToUpdate.Gender = (Gender) model.Gender;
        employeeToUpdate.HireDate = model.HireDate;
        employeeToUpdate.LastName = model.LastName;
        employeeToUpdate.PhoneNumber = model.Phone;
        employeeToUpdate.Picture = model.Picture;
            
        await _employeeRepo.UpdateAsync(employeeToUpdate);

        return new ApiResultResponse<EmployeeVm>() { Data = model };

    }

    public async Task DeleteAsync(string id)
    {
        var employee = await _employeeRepo.FindByIdAsync(id);

        var imageFileName = Path.GetFileName(employee.Picture);
            
            
        if (!string.Equals(imageFileName, "default.png"))
        {
            var imagePhysicalPath = Path.Combine(_env.ContentRootPath, "wwwroot", "employees", imageFileName ?? string.Empty);
            File.Delete(imagePhysicalPath);
        }

        await _employeeRepo.DeleteAsync(employee);
            
    }
}