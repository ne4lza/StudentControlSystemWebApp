using Microsoft.AspNetCore.Mvc;

namespace EYOkulProjectWebUI.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
