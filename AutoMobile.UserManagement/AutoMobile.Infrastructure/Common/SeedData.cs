using AutoMobile.Application.ApplicationConstants;
using AutoMobile.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                new IdentityRole { Name = "CUSTOMER",NormalizedName = "CUSTOMER"},
                new IdentityRole { Name = "PREMIUMCUSTOMER",NormalizedName = "PREMIUMCUSTOMER"},
                new IdentityRole { Name = "USER",NormalizedName = "USER"}
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

            if (!applicationDbContext.CustomClaimTypeValue.Any())
            {
                await applicationDbContext.AddRangeAsync(

                    new CustomClaimTypeValue(Guid.NewGuid(), CustomClaimType.ManagerType, CustomClaimValue.JuniorManager),
                    new CustomClaimTypeValue(Guid.NewGuid(), CustomClaimType.ManagerType, CustomClaimValue.SeniorManager),
                    new CustomClaimTypeValue(Guid.NewGuid(), CustomClaimType.ManagerType, CustomClaimValue.AssistantManager),
                    new CustomClaimTypeValue(Guid.NewGuid(), CustomClaimType.ManagerType, CustomClaimValue.AssociateProductManager)
                    );

                await applicationDbContext.SaveChangesAsync();
            }


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

                var superAdminResult = await userManager.CreateAsync(superAdmin, "Admin@123");

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

                var customerResult = await userManager.CreateAsync(customer, "Customer@123");

                if (customerResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer, CustomRole.Customer);

                }

                ApplicationUser userOne = new ApplicationUser
                {
                    Email = "userOne@gmail.com",
                    UserName = "userOne@gmail.com",
                    FirstName = "Jhon",
                    LastName = "Doe 1",
                    EmailConfirmed = true
                };

                var userOneResult = await userManager.CreateAsync(userOne, "User@123");

                if (userOneResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(userOne, CustomRole.User);

                    var claim = new Claim(CustomClaimType.ManagerType, CustomClaimValue.JuniorManager);

                    await userManager.AddClaimAsync(userOne,claim);
                }

                ApplicationUser userTwo = new ApplicationUser
                {
                    Email = "userTwo@gmail.com",
                    UserName = "userTwo@gmail.com",
                    FirstName = "Jhon",
                    LastName = "Doe 2",
                    EmailConfirmed = true
                };

                var userTwoResult = await userManager.CreateAsync(userTwo, "User@123");

                if (userTwoResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(userTwo, CustomRole.User);

                    var claim = new Claim(CustomClaimType.ManagerType, CustomClaimValue.JuniorManager);

                    await userManager.AddClaimAsync(userTwo, claim);

                    await applicationDbContext.SaveChangesAsync();
                }


            }
        }
    }
}
