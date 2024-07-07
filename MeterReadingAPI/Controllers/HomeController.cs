using Microsoft.AspNetCore.Mvc;

namespace MeterReadingAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
