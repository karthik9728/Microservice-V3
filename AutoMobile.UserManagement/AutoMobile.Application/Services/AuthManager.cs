using AutoMapper;
using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.Common;
using AutoMobile.Domain.InputModel;
using AutoMobile.Domain.InputModel.UserManager;
using AutoMobile.Domain.InputModel.Users;
using AutoMobile.Domain.Models;
using AutoMobile.Domain.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private ApplicationUser user;

        private const string _loginProvider = "AutoMobileProvider";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;

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
                var isRoleExists = await _roleManager.RoleExistsAsync(registerInputModel.Role);

                if (isRoleExists)
                {
                    await _userManager.AddToRoleAsync(user, registerInputModel.Role);
                }
                else
                {
                    await _userManager.DeleteAsync(user);
                    // Custom error message
                    var customError = new IdentityError
                    {
                        Code = "CustomError",
                        Description = "Invalid Role"
                    };

                    // Add the custom error to the result errors collection
                    var errors = new List<IdentityError>(result.Errors);
                    errors.Add(customError);
                    result = IdentityResult.Failed(errors.ToArray());
                }
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
                var isRoleExists = await _roleManager.RoleExistsAsync(CustomRole.Customer);

                if (isRoleExists)
                {
                    await _userManager.AddToRoleAsync(user, CustomRole.Customer);

                    var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    string codeHtmlVersion = HttpUtility.UrlEncode(confirmationToken);

                    //var link = $"https://localhost:7000/api/usermanagement/User/EmailConfirmation?UserId={user.Id}&Token={codeHtmlVersion}";

                    var link = $"https://localhost:7143/Auth/EmailConfirmation?UserId={user.Id}&Token={codeHtmlVersion}";

                    await _emailService.EmailVerification(user.Email, link);
                }
                else
                {
                    await _userManager.DeleteAsync(user);

                    var customError = new IdentityError
                    {
                        Code = "CustomError",
                        Description = "Invalid Role"
                    };

                    // Add the custom error to the result errors collection
                    var errors = new List<IdentityError>(result.Errors);
                    errors.Add(customError);
                    result = IdentityResult.Failed(errors.ToArray());
                }

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


        public async Task<List<string>> GetRoles()
        {
            var identityRoles = await _roleManager.Roles.ToListAsync();

            List<string> roles = new List<string>();

            foreach (var role in identityRoles)
            {
                roles.Add(role.Name);
            }

            return roles;
        }

        public async Task<bool> EmailConfirmation(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public Task<IEnumerable<IdentityError>> ChangePassword(ChangePasswordInputModel changePasswordInputModel)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ForgetPassword(string emailId)
        {
            var user = await _userManager.FindByEmailAsync(emailId);
            if (user != null)
            {
                var confirmationToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                string codeHtmlVersion = HttpUtility.UrlEncode(confirmationToken);

                var link = $"https://localhost:7143/Auth/ResetPassword?UserId={user.Id}&Token={codeHtmlVersion}";

                await _emailService.ForgetPassword(user.Email, link);
                return true;
            }
            return false;
        }

        public async Task<bool> ResetPassword(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> IsUserExistsByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public async Task<List<ApplicationUserVM>> GetUsers()
        {
            List<ApplicationUserVM> customers = new List<ApplicationUserVM>();

            string[] roleNames = { CustomRole.PremiumCustomer, CustomRole.Customer };

            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Intersect(roleNames).Any())
                {
                    var userVM = _mapper.Map<ApplicationUserVM>(user);
                    userVM.Role = userRoles.Intersect(roleNames).FirstOrDefault();
                    customers.Add(userVM);
                }
            }

            return customers;
        }

        public async Task<bool> ChangeUserRole(string id,string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user != null)
            {
                var userRole = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, userRole);

                await _userManager.AddToRoleAsync(user, role);

                return true;
            }

            return false;
           
        }
    }
}
