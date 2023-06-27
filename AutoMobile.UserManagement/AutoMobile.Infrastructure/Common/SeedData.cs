using AutoMobile.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoMobile.Application.ApplicationConstants.ApplicationConstant;

namespace AutoMobile.Infrastructure.Common
{
    public class SeedData
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new List<IdentityRole>()
            {
                new IdentityRole { Name = "SUPERADMIN",NormalizedName = "SUPERADMIN" },
                new IdentityRole { Name = "MASTERADMIN",NormalizedName = "MASTERADMIN" },
                new IdentityRole { Name = "ADMIN",NormalizedName = "ADMIN" },
                new IdentityRole { Name = "CUSTOMER",NormalizedName = "CUSTOMER"}
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }

        public static async Task SeedDataAsync(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
        {
            if (!applicationDbContext.ApplicationUser.Any())
            {
                using var scope = serviceProvider.CreateScope();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                ApplicationUser superAdmin = new ApplicationUser
                {
                    Email = "superadmin@gmail.com",
                    UserName = "superadmin@gmail.com",
                    FirstName = "Super",
                    LastName = "Admin",
                    EmailConfirmed = true
                };

                var superAdminResult = await userManager.CreateAsync(superAdmin,"Admin@123");

                if (superAdminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, CustomRole.SuperAdmin);
                }

                ApplicationUser masterAdmin = new ApplicationUser
                {
                    Email = "masteradmin@gmail.com",
                    UserName = "masteradmin@gmail.com",
                    FirstName = "Master",
                    LastName = "Admin",
                    EmailConfirmed = true
                };

                var masterAdminResult = await userManager.CreateAsync(masterAdmin, "Admin@123");

                if (masterAdminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(masterAdmin, CustomRole.MasterAdmin);
                }

                ApplicationUser customer = new ApplicationUser
                {
                    Email = "customer@gmail.com",
                    UserName = "customer@gmail.com",
                    FirstName = "Jhon",
                    LastName = "Doe",
                    EmailConfirmed = true
                };

                var customerResult = await userManager.CreateAsync(customer, "Cutomer@123");

                if (customerResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer, CustomRole.Customer);
                }

            }
        }
    }
}
