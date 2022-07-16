using Core.Entities;
using Data.Contexts;
using Microsoft.AspNetCore.Identity;

namespace WebServer;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var appContext = scope.ServiceProvider.GetRequiredService<BlazorRoversContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Employee>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        DbInitializer.Initialize(appContext, userManager, roleManager).Wait();
        return host;
    }
}