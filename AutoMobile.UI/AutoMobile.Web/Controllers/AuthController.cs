using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.Common;
using AutoMobile.Domain.DTO.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using AutoMobile.Domain.ApplicationConstants;
using AutoMobile.Domain.ViewModels.SelectListItemVM;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMobile.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;
using System.Web;
using AutoMobile.Domain.DTO.UserManager;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AutoMobile.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IJwtHelper _jwtHelper;

        public AuthController(IAuthService authService, IJwtHelper jwtHelper)
        {
            _authService = authService;
            _jwtHelper = jwtHelper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto();

            return View(loginRequestDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ApiResponse response = await _authService.LoginAsync<ApiResponse>(loginRequestDto);

            if (response != null && response.IsSuccess)
            {
                LoginResponseDto model = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                //Extract Role from Token
                var tokenData = _jwtHelper.ExtractTokenData(model.Token);

                identity.AddClaim(new Claim(ClaimTypes.Name, model.UserId));

                if (tokenData != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, tokenData.Role.ToUpper()));

                    if(tokenData.AdditionalClaims.Count > 0)
                    {
                        foreach (var claim in tokenData.AdditionalClaims)
                        {
                            foreach (var claimValue in claim.Value)
                            {
                                identity.AddClaim(new Claim(claim.Key, claimValue));
                            }
                        }
                    }
                   
                }

                var principle = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                });

                //HttpContext.Session.SetString(ApplicationConstant.SessionToken, model.Token);

                HttpContext.Response.Cookies.Append(ApplicationConstant.SessionToken, model.Token, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30)),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.Errors.FirstOrDefault().Description);
                return View(loginRequestDto);
            }
        }



        [HttpGet]
        public IActionResult Register()
        {
            RegisterationRequestDto registerationRequestDto = new RegisterationRequestDto();

            return View(registerationRequestDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDto registerationRequestDto)
        {
            ApiResponse response = await _authService.RegisterAsync<ApiResponse>(registerationRequestDto);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(RegisterVerification));
            }
            else
            {
                ModelState.AddModelError("CustomError", response.Errors.FirstOrDefault().Description);
                return View(registerationRequestDto);
            }
        }


        public IActionResult RegisterVerification()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AdminRegister()
        {

            var response = await _authService.GetRolesAsync<ApiResponse>();

            if (response.IsSuccess)
            {
                List<string> roles = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(response.Result));


                AdminRegisterVM adminRegisterVM = new AdminRegisterVM
                {
                    RegisterationRequestDto = new AdminRegisterationRequestDto(),
                    RolesList = roles.Order().Select(x => new SelectListItem
                    {
                        Text = x,
                        Value = x.ToString()
                    })
                };

                return View(adminRegisterVM);
            }

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminRegister(AdminRegisterationRequestDto registerationRequestDto)
        {
            ApiResponse response = await _authService.AdminRegisterAsync<ApiResponse>(registerationRequestDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "User Successfully Registered";

                return RedirectToAction(nameof(AdminRegister));
            }
            else
            {
                ModelState.AddModelError("CustomError", response.Errors.FirstOrDefault().Description);
                return View(registerationRequestDto);
            }
        }


        [HttpGet]
        public IActionResult EmailConfirmation()
        {
            return View();
        }

        [HttpPost, ActionName("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmationPost()
        {
            var queryString = HttpContext.Request.QueryString.ToString();

            //// Parse the query string
            //NameValueCollection queryParams = HttpUtility.ParseQueryString(queryString);

            //// Extract UserId and Token values
            //string userId = queryParams["UserId"];
            //string token = queryParams["Token"];

            var response = await _authService.EmailConfirmationAsync<ApiResponse>(queryString);

            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(AccountVerified));
            }

            return View();
        }

        public IActionResult AccountVerified()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string emailId)
        {
            var response = await _authService.ForgetPasswordAsync<ApiResponse>(emailId);

            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(PasswordReset));
            }

            return View();
        }

        public IActionResult PasswordReset()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string newPassword)
        {
            var queryString = HttpContext.Request.QueryString.ToString() + $"&&newPassword={newPassword}";

            var response = await _authService.ResetPasswordAsync<ApiResponse>(queryString);

            if(response.IsSuccess) 
            { 
                return RedirectToAction(nameof(ResetPasswordSuccess));
            }

            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            HttpContext.Response.Cookies.Delete(ApplicationConstant.SessionToken);

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult InternalServerError()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<ApplicationUserDto> applicationUserDtos = new List<ApplicationUserDto>();

            var response = await _authService.GetUsersAsync<ApiResponse>();

            if (response !=null && response.IsSuccess)
            {
                applicationUserDtos = JsonConvert.DeserializeObject<List<ApplicationUserDto>>(Convert.ToString(response.Result));

                return View(applicationUserDtos);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserRole(string id)
        {
            ApplicationUserDto userDto = new ApplicationUserDto();

            var response = await _authService.GetUserByIdAsync<ApiResponse>(id);

            if(response != null && response.IsSuccess)
            {
                userDto = JsonConvert.DeserializeObject<ApplicationUserDto>(Convert.ToString(response.Result));

                return View(userDto);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(ApplicationUserDto dto)
        {

            var response = await _authService.ChangeUserRoleAsync<ApiResponse>(dto.Id, dto.Role);

            if(response !=null && response.IsSuccess)
            {
                return RedirectToAction(nameof(GetUsers));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserClaim(string id)
        {
            ApplicationUserDto userDto = new ApplicationUserDto();

            List<CustomClaimListDto> systemClaims = new List<CustomClaimListDto>();

            List<ClaimInputModelDto> userClaims = new List<ClaimInputModelDto>();

            var response = await _authService.GetUserByIdAsync<ApiResponse>(id);

            var claimResponse = await _authService.GetClaimsAsync<ApiResponse>();

            var userClaimResponse = await _authService.GetUserClaimsAsync<ApiResponse>(id);

            if (response != null && response.IsSuccess && claimResponse != null && claimResponse.IsSuccess && userClaimResponse != null && userClaimResponse.IsSuccess)
            {
                userDto = JsonConvert.DeserializeObject<ApplicationUserDto>(Convert.ToString(response.Result));

                systemClaims = JsonConvert.DeserializeObject<List<CustomClaimListDto>>(Convert.ToString(claimResponse.Result));

                userClaims = JsonConvert.DeserializeObject<List<ClaimInputModelDto>>(Convert.ToString(userClaimResponse.Result));

                ApplicationUserClaimsDto userClaim = new ApplicationUserClaimsDto
                {
                    UserDto = userDto,
                    SystemClaims = systemClaims,
                    UserClaims = userClaims
                };

                return View(userClaim);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserClaim(ApplicationUserClaimsDto model, [FromForm] List<string> selectedClaims)
        {
            List<ClaimInputModelDto> claims = selectedClaims
               .Select(c =>
               {
                   string[] parts = c.Split(':');
                   return new ClaimInputModelDto
                   {
                       ClaimType = parts[0],
                       ClaimValue = parts[1]
                   };
               })
               .ToList();

            AddOrRemoveClaimDto dto = new AddOrRemoveClaimDto
            {
                UserId = model.UserDto.Id,
                Claims = claims
            };

             var  response =  await _authService.AddOrRemoveUserClaimAsync<ApiResponse>(dto);

            if(response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(GetUsers));
            }


           return  View();

        }
    }
}
