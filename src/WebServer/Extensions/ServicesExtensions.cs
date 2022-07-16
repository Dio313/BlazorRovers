using System.Text;
using Common.AuthViewModels;
using Core.Interfaces;
using Data.Helpers;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Service.Departments;
using Service.Employees;
using Service.Projects;
using Service.Roles;
using WebServer.Helpers;

namespace WebServer.Extensions;

public static class ServicesExtensions
{

    public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["JwtIssuer"],
                    ValidAudience = config["JwtAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSecurityKey"]))
                };
            });
    }


    public static void ConfigureRepositoryWrapper(this IServiceCollection services) 
    {
        services.AddSingleton<HttpContextAccessor>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IRoleService, RoleService>();

    }

    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(config => {
            config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
            config.AddPolicy(Policies.IsStaff, Policies.IsStaffPolicy());
            config.AddPolicy(Policies.IsManager, Policies.IsManagerPolicy());
        });
    }
}