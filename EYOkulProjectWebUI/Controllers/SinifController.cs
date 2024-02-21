using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EYOkulProjectWebUI.Controllers
{
    [Authorize]
    public class SinifController : Controller
    {
        EYOkulDbContext _context;

        public SinifController(EYOkulDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = _context.TBL_CLASS.Where(x=>x.ScoolId ==HttpContext.Session.GetInt32("SchoolId")).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddSinif(ClassModel cl) 
        {
            
            ClassModel model = new ClassModel()
            {
                ClassName = cl.ClassName,
                Desciription = cl.Desciription,
                InsertedDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                UpdateDate = DateTime.Now,
                ScoolId = (int)HttpContext.Session.GetInt32("SchoolId"),
                SysUserId = (int)HttpContext.Session.GetInt32("SysUserId")

            };
            _context.TBL_CLASS.Add(model);
            _context.SaveChanges();
            LogModel log = new LogModel()
            {
                ActivityType = "INSERT",
                SysUserId = (int) HttpContext.Session.GetInt32("SysUserId"),
                RecordId = model.Id,
                RecordName = "CLASS",
                IsActive = model.IsActive,
                IsDeleted = model.IsDeleted,
                EventTime = DateTime.Now,
            };
            _context.TBL_LOGS.Add(log);
            _context.SaveChanges();
            TempData["Alert"] = "Sınıf Ekleme İşlemi Tamamlandı.";
            return RedirectToAction("Index","Sinif");
        }
        [HttpGet]
        public IActionResult UpdateSinif(int id)
        {
            
            var model= _context.TBL_CLASS.Where(x=>x.Id == id).FirstOrDefault();
            return PartialView("_UpdateSinifModal", model);
        }
        [HttpPost]
        public IActionResult UpdateSinif(ClassModel cl)
        {
            cl.UpdateDate = DateTime.Now;
            _context.TBL_CLASS.Update(cl);
            _context.SaveChanges();
            LogModel log = new LogModel()
            {
                ActivityType = "UPDATE",
                SysUserId = (int)HttpContext.Session.GetInt32("SysUserId"),
                RecordId = cl.Id,
                RecordName = "CLASS",
                IsActive = cl.IsActive,
                IsDeleted = cl.IsDeleted,
                EventTime = DateTime.Now,
            };
            _context.TBL_LOGS.Add(log);
            _context.SaveChanges();
            TempData["Alert"] = "Sınıf Güncelleme İşlemi Tamamlandı.";
            return RedirectToAction("Index","Sinif");
        }

    }
}
