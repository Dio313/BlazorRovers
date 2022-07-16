using System.Security.Claims;
using Core.Entities;
using Core.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

    public static class DbInitializer
    {
        public static async  Task Initialize(BlazorRoversContext context, UserManager<Employee> userManager,
            RoleManager<IdentityRole> roleManager)
        {
           
            try
            {
                if ( (await context.Database.GetPendingMigrationsAsync()).Any())
                {
                   await context.Database.MigrateAsync();
                }
                else
                {
                   await context.Database.EnsureCreatedAsync();
                   
                }
            }
            catch (Exception)
            {
                // ignored
            }

            
            //Check if AdminRole Exists
            var adminRole = new IdentityRole("Admin");
            var adminRoleInDb = await roleManager.FindByNameAsync("Admin");
            if (adminRoleInDb is null)
            {
                await roleManager.CreateAsync(adminRole);
            }


            //Check if StaffRole Exists
            var staffRole = new IdentityRole("Staff");
            var staffRoleInDb = await roleManager.FindByNameAsync("Staff");
            if (staffRoleInDb is null)
            {
                await roleManager.CreateAsync(staffRole);
            }


            //Check if ManagerRole Exists
            var managerRole = new IdentityRole("Manager");
            var managerRoleInDb = await roleManager.FindByNameAsync("Manager");
            if (managerRoleInDb is null)
            {
                await roleManager.CreateAsync(managerRole);
            }

            // create a list to track the newly added users
            var addedUserList = new List<Employee>();

            //Add Admin User

            var userAdmin = new Employee
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                EmployeeNumber = "MGT01",
                UserName ="admin313",
                Email = "admin@BlazorRovers.com",
                FirstName = "Admin",
                LastName = "SystemAdmin",
                PhoneNumber = "08081234567",
                Designation = Designation.Manager,
                Gender = Gender.Male,
                HireDate = new DateTime(1999, 02, 27),
                
                // confirm the e-mail and remove lockout
                LockoutEnabled = false,
                PhoneNumberConfirmed = true
            };
            var adminUserInDb = await userManager.FindByEmailAsync(userAdmin.Email);
            if (adminUserInDb == null)
            {
                await userManager.CreateAsync(userAdmin, "Admin@3130");
                var result = await userManager.AddToRoleAsync(userAdmin, "Admin");
                if (result.Succeeded)
                {
                    await roleManager.AddClaimAsync(adminRole, new Claim(ClaimTypes.Role,  "Admin"));
                }
            }
            // add the admin user to the added users list
            addedUserList.Add(userAdmin);

            // if we added at least one user, persist the changes into the DB
            if (addedUserList.Count > 0)
                await context.SaveChangesAsync();


            if (!context.Departments.Any())
            {
                await context.Departments.AddRangeAsync(new[]
                {
                    new Department() { Id = Guid.NewGuid().ToString(), Name = "Development", Description = "Development Dept" },
                    new Department() { Id = Guid.NewGuid().ToString(), Name = "Marketing", Description = "Marketing Dept" },
                    new Department() { Id = Guid.NewGuid().ToString(), Name = "Consulting", Description = "Consulting" }
                });
                await context.SaveChangesAsync();
            }

        }

    }
