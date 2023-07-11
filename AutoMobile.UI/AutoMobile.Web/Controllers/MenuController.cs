using Microsoft.AspNetCore.Mvc;

namespace AutoMobile.Web.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult LoadPage(string pageName)
        {
            // Handle the logic to determine which page to load based on the 'pageName' parameter
            // Render the corresponding partial view and return it
            return PartialView($"_{pageName}"); // Assuming partial views are named with an underscore prefix (e.g., "_Home.cshtml")
        }
    }
}
