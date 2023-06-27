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
                var role = _jwtHelper.ExtractRoleFromToken(model.Token);

                identity.AddClaim(new Claim(ClaimTypes.Name, model.UserId));

                if (role != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.ToUpper()));
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
                return RedirectToAction(nameof(Login));
            }
            else
            {
                ModelState.AddModelError("CustomError", response.Errors.FirstOrDefault().Description);
                return View(registerationRequestDto);
            }
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
                    RolesList = roles.Order().Select(x=> new SelectListItem
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            HttpContext.Response.Cookies.Delete(ApplicationConstant.SessionToken);

            //HttpContext.Session.SetString(ApplicationConstant.SessionToken, "");

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
    }
}
