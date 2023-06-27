using AutoMapper;
using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.Common;
using AutoMobile.Domain.InputModel;
using AutoMobile.Domain.InputModel.UserManager;
using AutoMobile.Domain.InputModel.Users;
using AutoMobile.Domain.Models;
using AutoMobile.Domain.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static AutoMobile.Application.ApplicationConstants.ApplicationConstant;

namespace AutoMobile.Application.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private ApplicationUser user;

        private const string _loginProvider = "AutoMobileProvider";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<IdentityError>> AdminSignUp(AdminRegisterInputModel registerInputModel)
        {
            user = _mapper.Map<ApplicationUser>(registerInputModel);
            user.UserName = registerInputModel.Email;

            //Register
            var result = await _userManager.CreateAsync(user, registerInputModel.Password);

            if (result.Succeeded)
            {
                //Adding Roles to user
                await _userManager.AddToRoleAsync(user,registerInputModel.Role);
            }

            return result.Errors;
        }

        public async Task<IEnumerable<IdentityError>> SignUp(RegisterInputModel registerInputModel)
        {
            user = _mapper.Map<ApplicationUser>(registerInputModel);
            user.UserName = registerInputModel.Email;

            //Register
            var result = await _userManager.CreateAsync(user, registerInputModel.Password);

            if (result.Succeeded)
            {
                //Adding Roles to user
                await _userManager.AddToRoleAsync(user, CustomRole.Customer);
            }

            return result.Errors;
        }

     

        public async Task<object> SignIn(LoginInputModel loginInputModel)
        {
            user = await _userManager.FindByEmailAsync(loginInputModel.Email);


            var result = await _signInManager.PasswordSignInAsync(user, loginInputModel.Password, isPersistent: true, lockoutOnFailure: true);

            var isValidCredential = await _userManager.CheckPasswordAsync(user, loginInputModel.Password);

            if (result.Succeeded)
            {
               
                var token = await GenerateToken();

                return new AuthResponseVM
                {
                    UserId = user.Id,
                    Token = token,
                };
            }
            else
            {
                if (result.IsLockedOut)
                {
                    return "You are Account is Locked,Contact System ";
                }
                if (result.IsNotAllowed)
                {
                    return "Please Verfiy Emaill Address";
                }
                if (isValidCredential == false)
                {
                    return "Invalid Password";
                }
                else
                {
                    return "Login Failed";
                }
            }
        }


        public async Task<string> GenerateToken()
        {
            //getting securtiy key from appsettings.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            //digital signature for security key with SecurityAlgorithms
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Getting Roles of User for Database if any roles is there
            var roles = await _userManager.GetRolesAsync(user);

            //Coverting normal string type roles to claim type
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            //Getting Calims of User for Database if any Claims is there
            var userClaims = await _userManager.GetClaimsAsync(user);

            //Listing out Claims need to add with JWT
            List<Claim> claims = new List<Claim>()
            {
                new Claim(SystemConstant.ClaimConstants.UserId, user.Id.ToString()),
            }.Union(userClaims).Union(roleClaims).ToList();

            // Modify role claims
            var httpRoleClaims = claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").ToList();
            claims.RemoveAll(c => httpRoleClaims.Contains(c));
            var roleClaimsChange = httpRoleClaims.Select(c => new Claim("role", c.Value));
            claims.AddRange(roleClaimsChange);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"]))
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> IsUserExists(string emailId)
        {
            var user = await _userManager.FindByEmailAsync(emailId);
            if (user != null)
            {
                return true;
            }
            return false;
        }


        public Task<bool> EmailConfirmation(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IdentityError>> ChangePassword(ChangePasswordInputModel changePasswordInputModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForgetPassword(string emailId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ResetPassword(string userId, string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserExistsByUserId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
