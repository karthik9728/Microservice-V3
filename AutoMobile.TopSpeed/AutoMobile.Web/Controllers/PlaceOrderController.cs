using AutoMobile.Application.Services;
using AutoMobile.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoMobile.Web.Controllers
{
    [Route("api/topspeed/[controller]")]
    [ApiController]
    public class PlaceOrderController : ControllerBase
    {
        private readonly ITopSpeedService _topSpeedService;

        public PlaceOrderController(ITopSpeedService topSpeedService)
        {
            _topSpeedService = topSpeedService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] PlaceOrder placeOrder)
        {
            _topSpeedService.PlaceOrder(placeOrder);

            return Ok(placeOrder);
        }
    }
}
