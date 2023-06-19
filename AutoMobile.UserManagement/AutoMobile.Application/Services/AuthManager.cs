﻿using AutoMapper;
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

        public async Task<IEnumerable<IdentityError>> SignUp(RegisterInputModel registerInputModel)
        {
            user = _mapper.Map<ApplicationUser>(registerInputModel);
            user.UserName = registerInputModel.Email;

            //Register
            var result = await _userManager.CreateAsync(user, registerInputModel.Password);

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
                    RefreshToken = await CreateRefreshToken()
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


        public async Task<string> CreateRefreshToken()
        {
            //remove old token if exsits in database
            await _userManager.RemoveAuthenticationTokenAsync(user, _loginProvider, _refreshToken);

            //generate new refresh token
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, _loginProvider, _refreshToken);

            //set the new generated refresh token fot user 
            await _userManager.SetAuthenticationTokenAsync(user, _loginProvider, _refreshToken, newRefreshToken);

            return newRefreshToken;

        }


        public async Task<AuthResponseVM> VerfiyRefreshToken(AuthResponseInputModel request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            //getting the old token
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);

            //Decryption of old token
            var userEmail = tokenContent.Claims.ToList().FirstOrDefault(x => x.Type == SystemConstant.ClaimConstants.UserId).Value;

            user = await _userManager.FindByIdAsync(userEmail);

            if (user == null || user.Id != request.UserId)
            {
                return null;
            }

            //Checking Refresh token is valid or not
            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(user, _loginProvider, _refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseVM
                {
                    UserId = user.Id,
                    Token = token,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            //sign out the logged in user
            await _userManager.UpdateSecurityStampAsync(user);
            return null;
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
    }
}
