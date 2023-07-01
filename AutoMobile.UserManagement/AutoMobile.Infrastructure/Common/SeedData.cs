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
                new IdentityRole { Name = "USER",NormalizedName = "USER"},
                new IdentityRole { Name = "PREMIUMUSER",NormalizedName = "PREMIUMUSER"}
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

                ApplicationUser admin = new ApplicationUser
                {
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                    FirstName = "Jhon",
                    LastName = "Doe",
                    EmailConfirmed = true
                };

                var adminResult = await userManager.CreateAsync(admin, "Admin@123");

                if (adminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, CustomRole.Admin);

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

                    var claims = new List<Claim>()
                    {
                         new Claim(CustomClaimType.ManagerType, CustomClaimValue.JuniorManager),
                         new Claim(CustomClaimType.ManagerType, CustomClaimValue.SeniorManager)
                    };

                    await userManager.AddClaimsAsync(userTwo, claims);

                    await applicationDbContext.SaveChangesAsync();
                }

                ApplicationUser userThree = new ApplicationUser
                {
                    Email = "userThree@gmail.com",
                    UserName = "userThree@gmail.com",
                    FirstName = "Jhon",
                    LastName = "Doe 3",
                    EmailConfirmed = true
                };

                var userThreeResult = await userManager.CreateAsync(userThree, "User@123");

                if (userThreeResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(userThree, CustomRole.User);

                    var claims = new List<Claim>()
                    {
                         new Claim(CustomClaimType.ManagerType, CustomClaimValue.JuniorManager),
                         new Claim(CustomClaimType.ManagerType, CustomClaimValue.SeniorManager),
                         new Claim(CustomClaimType.ManagerType, CustomClaimValue.AssistantManager)
                    };

                    await userManager.AddClaimsAsync(userThree, claims);

                    await applicationDbContext.SaveChangesAsync();
                }

                ApplicationUser userFour = new ApplicationUser
                {
                    Email = "userFour@gmail.com",
                    UserName = "userFour@gmail.com",
                    FirstName = "Jhon",
                    LastName = "Doe 4",
                    EmailConfirmed = true
                };

                var userFourResult = await userManager.CreateAsync(userFour, "User@123");

                if (userFourResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(userFour, CustomRole.User);

                    var claims = new List<Claim>()
                    {
                         new Claim(CustomClaimType.ManagerType, CustomClaimValue.JuniorManager),
                         new Claim(CustomClaimType.ManagerType, CustomClaimValue.SeniorManager),
                         new Claim(CustomClaimType.ManagerType, CustomClaimValue.AssistantManager),
                         new Claim(CustomClaimType.ManagerType, CustomClaimValue.AssociateProductManager)
                    };

                    await userManager.AddClaimsAsync(userFour, claims);

                    await applicationDbContext.SaveChangesAsync();
                }

            }
        }
    }
}
