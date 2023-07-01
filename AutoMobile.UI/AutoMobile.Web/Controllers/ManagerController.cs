using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoMobile.Web.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult JuniorManager()
        {
            return View();
        }

        public IActionResult SeniorManager()
        {
            return View();
        }

        public IActionResult AssistantManager()
        {
            return View();
        }

        public IActionResult AssociateProductManager()
        {
            return View();
        }
    }
}
