using EYOkulProjectWebUI.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EYOkulProjectWebUI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly EYOkulDbContext _context;

        public DashboardController(EYOkulDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.userFullname = HttpContext.Session.GetString("FullName");
            int schoolId = (int)HttpContext.Session.GetInt32("SchoolId");
            ViewBag.studentCount = _context.TBL_STUDENTS.Where(x => x.ScoolId == schoolId).Count();
            ViewBag.classCount = _context.TBL_CLASS.Where(x => x.ScoolId == schoolId).Count();
            ViewBag.cardCount = _context.TBL_CARDS.Where(x => x.ScoolId == schoolId).Count();
            return View();
        }
    }
}
