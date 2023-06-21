using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.ApplicationConstants;
using AutoMobile.Domain.Common;
using AutoMobile.Domain.DTO.Vehicle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoMobile.Web.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {

        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index()
        {
            List<VehicleDto> list = new List<VehicleDto>();

            var response = await _vehicleService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(ApplicationConstant.SessionToken));

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VehicleDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }
    }
}
