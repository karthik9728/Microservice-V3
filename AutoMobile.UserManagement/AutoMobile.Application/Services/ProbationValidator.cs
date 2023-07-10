using AutoMobile.Application.ApplicationConstants;
using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services
{
    public class ProbationValidator : IProbationValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProbationValidator(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public async Task<bool> ValidateUserProbationAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return false;
            }

            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

            var probationEndDateClaim = claimsPrincipal.FindFirst(CustomClaimType.ProbationEndDate);

            if(probationEndDateClaim != null)
            {
                var probationEndDate = DateTime.Parse(probationEndDateClaim.Value);

                if(DateTime.UtcNow >= probationEndDate)
                {
                    return true;
                }
            }

            return false;

        }
    }
}
