using EYOkulProjectWebUI.DAL;
using Microsoft.AspNetCore.Mvc;

namespace EYOkulProjectWebUI.Controllers
{
    public class SinifController : Controller
    {
        public IActionResult Index()
        {
            using var context = new EYOkulDbContext();
            var model = context.TBL_CLASS.ToList();
            return View(model);
        }
    }
}
