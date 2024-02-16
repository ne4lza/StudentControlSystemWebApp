using EYOkulProjectWebUI.DAL;
using Microsoft.AspNetCore.Mvc;

namespace EYOkulProjectWebUI.Controllers
{
    public class TerminalController : Controller
    {
        public IActionResult Index()
        {
            using var context = new EYOkulDbContext();
            var model = context.TBL_TERMINALS.ToList();
            return View(model);
        }
    }
}
