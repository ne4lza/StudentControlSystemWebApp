using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using EYOkulProjectWebUI.Models.LogModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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
            var model = _context.TBL_CLASS.Where(x => x.ScoolId == HttpContext.Session.GetInt32("SchoolId")).ToList();
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
            Class_H_Model class_H_Model = new Class_H_Model()
            {
                ProccessType = "INSERT",
                ClassName = cl.ClassName,
                Desciription = cl.Desciription,
                InsertedDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                UpdateDate = DateTime.Now,
                ScoolId = (int)HttpContext.Session.GetInt32("SchoolId"),
                SysUserId = (int)HttpContext.Session.GetInt32("SysUserId")
            };
            _context.TBL_H_CLASS.Add(class_H_Model);
            _context.TBL_CLASS.Add(model);
            _context.SaveChanges();
            TempData["Alert"] = "Sınıf Ekleme İşlemi Tamamlandı.";
            return RedirectToAction("Index", "Sinif");
        }
        [HttpGet]
        public IActionResult UpdateSinif(int id)
        {

            var model = _context.TBL_CLASS.Where(x => x.Id == id).FirstOrDefault();
            return PartialView("_UpdateSinifModal", model);
        }
        [HttpPost]
        public IActionResult UpdateSinif(ClassModel cl)
        {
            cl.UpdateDate = DateTime.Now;
            cl.SysUserId = (int)HttpContext.Session.GetInt32("SysUserId");
            _context.TBL_CLASS.Update(cl);
            Class_H_Model class_H_Model = new Class_H_Model()
            {
                ProccessType = "UPDATE",
                ClassName = cl.ClassName,
                Desciription = cl.Desciription,
                InsertedDate = cl.InsertedDate,
                IsActive = cl.IsActive,
                IsDeleted = cl.IsDeleted,
                UpdateDate = DateTime.Now,
                ScoolId = (int)HttpContext.Session.GetInt32("SchoolId"),
                SysUserId = (int)HttpContext.Session.GetInt32("SysUserId")
            };
            _context.TBL_H_CLASS.Add(class_H_Model);
            _context.SaveChanges();
            TempData["Alert"] = "Sınıf Güncelleme İşlemi Tamamlandı.";
            return RedirectToAction("Index", "Sinif");
        }

    }
}
