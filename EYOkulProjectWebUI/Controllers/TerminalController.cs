using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EYOkulProjectWebUI.Controllers
{
    [Authorize]
    public class TerminalController : Controller
    {
        private readonly EYOkulDbContext _context;

        public TerminalController(EYOkulDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = _context.TBL_TERMINALS.Where(x => x.SchoolId == HttpContext.Session.GetInt32("SchoolId")).ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult AddTerminal(TerminalModel terminalModel) 
        {
            TerminalModel terminal = new TerminalModel()
            {
                TerminalName = terminalModel.TerminalName,
                TerminalNum = terminalModel.TerminalNum,
                TerminalIp = terminalModel.TerminalIp,
                IsActive = true,
                IsDeleted = false,
                InsertedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                SchoolId = (int)HttpContext.Session.GetInt32("SchoolId"),
                SysUserId = (int)HttpContext.Session.GetInt32("SysUserId")
            };
            _context.TBL_TERMINALS.Add(terminal);
            _context.SaveChanges();
            TempData["Alert"] = "Terminal Ekleme İşlemi Tamamlandı.";
            return RedirectToAction("Index", "Terminal");
        }
        [HttpGet]
        public IActionResult UpdateTerminal(int id)
        {
            var terminal = _context.TBL_TERMINALS.Where(x => x.Id == id).FirstOrDefault();
            return PartialView("_UpdateTerminalModal", terminal);
        }
        [HttpPost]
        public IActionResult UpdateTerminal(TerminalModel terminal)
        {
            terminal.UpdatedDate = DateTime.Now;
            terminal.SchoolId = (int)HttpContext.Session.GetInt32("SchoolId");
            _context.TBL_TERMINALS.Update(terminal);
            _context.SaveChanges();
            TempData["Alert"] = "Terminal Güncelleme İşlemi Tamamlandı.";
            return RedirectToAction("Index", "Terminal");
        }
    }
}
