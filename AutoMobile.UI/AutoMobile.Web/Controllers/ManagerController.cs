using Microsoft.AspNetCore.Mvc;

namespace AutoMobile.Web.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
