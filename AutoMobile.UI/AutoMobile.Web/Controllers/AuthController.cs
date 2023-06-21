using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.Common;
using AutoMobile.Domain.DTO.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using AutoMobile.Domain.ApplicationConstants;

namespace AutoMobile.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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

                identity.AddClaim(new Claim(ClaimTypes.Name, model.UserId));


                var principle = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                });

                HttpContext.Session.SetString(ApplicationConstant.SessionToken, model.Token);

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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            HttpContext.Session.SetString(ApplicationConstant.SessionToken, "");

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
