using AutoMobile.Domain.Interface;
using AutoMobile.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoMobile.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailsRepository _orderDetailsRepository;

        public OrderDetailsController(IOrderDetailsRepository orderDetailsRepository)
        {
            _orderDetailsRepository = orderDetailsRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderDetails>> Get()
        {
            return Ok(_orderDetailsRepository.GetOrderDetails());
        }
    }
}
