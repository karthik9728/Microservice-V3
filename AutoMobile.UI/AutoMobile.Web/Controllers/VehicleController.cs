using AutoMapper;
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
        private readonly IMapper _mapper;
        public VehicleController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<VehicleDto> list = new List<VehicleDto>();

            var token = Request.Cookies[ApplicationConstant.SessionToken];

            var response = await _vehicleService.GetAllAsync<ApiResponse>(token);

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VehicleDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var token = Request.Cookies[ApplicationConstant.SessionToken];

            var response = await _vehicleService.GetAsync<ApiResponse>(id, token);

            if (response != null && response.IsSuccess)
            {
                VehicleDetailsDto model = JsonConvert.DeserializeObject<VehicleDetailsDto>(Convert.ToString(response.Result));

                return View(model);
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            VehicleCreateDto vehicleCreateDto = new VehicleCreateDto();

            return View(vehicleCreateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var token = Request.Cookies[ApplicationConstant.SessionToken];

                var response = await _vehicleService.CreateAsync<ApiResponse>(dto, token);

                if (response != null && response.IsSuccess)
                {

                    TempData["success"] = response.DisplayMessage;

                    return RedirectToAction(nameof(Index));
                }

            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var token = Request.Cookies[ApplicationConstant.SessionToken];

            var response = await _vehicleService.GetAsync<ApiResponse>(id, token);

            if (response != null && response.IsSuccess)
            {
                VehicleDetailsDto model = JsonConvert.DeserializeObject<VehicleDetailsDto>(Convert.ToString(response.Result));

                return View(_mapper.Map<VehicleUpdateDto>(model));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VehicleUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                var token = Request.Cookies[ApplicationConstant.SessionToken];

                var response = await _vehicleService.UpdateAsync<ApiResponse>(dto, token);

                if (response != null && response.IsSuccess)
                {

                    TempData["success"] = response.DisplayMessage;

                    return RedirectToAction(nameof(Index));
                }

            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = Request.Cookies[ApplicationConstant.SessionToken];

            var response = await _vehicleService.GetAsync<ApiResponse>(id, token);

            if (response != null && response.IsSuccess)
            {
                VehicleDetailsDto model = JsonConvert.DeserializeObject<VehicleDetailsDto>(Convert.ToString(response.Result));

                return View(_mapper.Map<VehicleUpdateDto>(model));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(VehicleDetailsDto dto)
        {
            if (ModelState.IsValid)
            {
                var token = Request.Cookies[ApplicationConstant.SessionToken];

                var response = await _vehicleService.DeleteAsync<ApiResponse>(dto.Id, token);

                if (response != null && response.IsSuccess)
                {

                    TempData["success"] = response.DisplayMessage;

                    return RedirectToAction(nameof(Index));
                }

            }

            return View(dto);
        }

    }
}
