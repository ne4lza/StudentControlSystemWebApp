using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace EYOkulProjectWebUI.Controllers
{
    public class VerifyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
