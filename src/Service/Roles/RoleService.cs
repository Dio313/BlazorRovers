using System.Security.Claims;
using Common.AuthViewModels;
using Common.ViewModels;
using Common.Wrapper;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Service.Roles;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<Employee> _userManager;

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<Employee> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }


    public async Task<ApiResultResponse<List<RoleVm>>> GetAllRoles()
    {
        try
        {
            var roles = await _roleManager.Roles.AsNoTracking()
                .Select(b => new RoleVm()
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToListAsync();
            return new ApiResultResponse<List<RoleVm>>()
            {
                Data = roles
            };
        }
        catch (Exception ex)
        {
            return new ApiResultResponse<List<RoleVm>>()
            {
                IsSuccess = false,
                Errors = new List<string> {ex.Message}
            };
        }
        
    }


    public async Task<ApiResultResponse<List<EmployeeListVm>>> GetAllUsers()
    {
        try
        {
            var employees =  await _userManager.Users.AsNoTracking()
                .Select(b => new EmployeeListVm()
                {
                    Id = b.Id,
                    FirstName = b.FirstName,
                    LastName = b.LastName,
                    EmployeeNumber = b.EmployeeNumber
                }).OrderBy(p => p.LastName).ToListAsync();

            return new ApiResultResponse<List<EmployeeListVm>>()
            {
                Data = employees
            };
        }
        catch (Exception ex)
        {
            return new ApiResultResponse<List<EmployeeListVm>>()
            {
                IsSuccess = false,
                Errors = new List<string> {ex.Message}
            };
        }
        
    }


    public async Task<ApiResultResponse<EditRoleVm>> AssignRole(EditRoleVm request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, request.RoleName));

            return new ApiResultResponse<EditRoleVm>()
            {
                IsSuccess = true,
                Message = "Role Assigned"
            };
        }
        catch (Exception ex)
        {
            return new ApiResultResponse<EditRoleVm>()
            {
                IsSuccess = false,
                Errors = new List<string> {ex.Message}
            };
        }
    }


    public async Task<ApiResultResponse<EditRoleVm>> RemoveRole(EditRoleVm request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, request.RoleName));

            return new ApiResultResponse<EditRoleVm>()
            {
                IsSuccess = true,
                Message = "Role Removed"
            };
        }
        catch (Exception ex)
        {
            return new ApiResultResponse<EditRoleVm>()
            {
                IsSuccess = false,
                Errors = new List<string> {ex.Message}
            };
        }
    }


}