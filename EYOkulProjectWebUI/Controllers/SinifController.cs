﻿using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            _context.TBL_CLASS.Add(model);
            _context.SaveChanges();
            TempData["Alert"] = "Sınıf Ekleme İşlemi Tamamlandı.";
            return RedirectToAction("Index", "Sinif");
        }
        [HttpPost]
        public IActionResult UpdateSinif(ClassModel cl)
        {
            // Güncelleme tarihini ve kullanıcı kimliğini ayarla
            cl.UpdateDate = DateTime.Now;
            cl.SysUserId = (int)HttpContext.Session.GetInt32("SysUserId"); 

            // Varlık örneğini doğrudan güncelle
            _context.TBL_CLASS.Update(cl);
            _context.SaveChanges();
            _context.Entry(cl).State = EntityState.Detached;

            TempData["Alert"] = "Sınıf Güncelleme İşlemi Tamamlandı.";
            return RedirectToAction("Index", "Sinif");
        }


    }
}
