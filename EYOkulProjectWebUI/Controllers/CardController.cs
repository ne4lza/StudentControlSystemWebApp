using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EYOkulProjectWebUI.Controllers
{
    public class CardController : Controller
    {
        private readonly EYOkulDbContext _context;

        public CardController(EYOkulDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var school = new SqlParameter("@SchoolId", HttpContext.Session.GetInt32("SchoolId"));

            var query = _context.TBL_CARDS
                                      .FromSqlRaw("EXEC sp_Cards @SchoolId", school)
                                      .ToList();
            return View(query);
        }
    }
}
