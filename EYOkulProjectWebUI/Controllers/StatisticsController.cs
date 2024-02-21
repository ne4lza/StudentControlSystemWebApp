using EYOkulProjectWebUI.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EYOkulProjectWebUI.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly EYOkulDbContext _context;

        public StatisticsController(EYOkulDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int schoolId = (int)HttpContext.Session.GetInt32("SchoolId");
            ViewBag.studentCount = _context.TBL_STUDENTS.Where(x=>x.ScoolId == schoolId).Count();
            ViewBag.classCount = _context.TBL_CLASS.Where(x => x.ScoolId == schoolId).Count();
            ViewBag.cardCount = _context.TBL_CARDS.Where(x=>x.ScoolId == schoolId).Count();
            return View();
        }
    }
}
