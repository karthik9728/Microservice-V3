using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AutoMobile.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IAuthService authService, ILogger<HomeController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> EmployeeBenefits()
        {

            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;

            var response = await _authService.ValidateUserAsync<ApiResponse>(userId.ToString());

            if(response != null)
            {
                if (response.IsSuccess && response.Errors.Count == 0)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("ProbationPeriod","Auth");
                }
            }

            return View();
        }
    }
}