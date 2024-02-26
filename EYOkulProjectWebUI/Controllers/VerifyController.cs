using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Hubs;
using EYOkulProjectWebUI.Subscription.Concreate;
using Microsoft.AspNetCore.Mvc;

namespace EYOkulProjectWebUI.Controllers
{
    public class VerifyController : Controller
    {
        private readonly EYOkulDbContext _context;

        public VerifyController(EYOkulDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
