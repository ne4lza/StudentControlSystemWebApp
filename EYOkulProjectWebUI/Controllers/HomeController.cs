using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace EYOkulProjectWebUI.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EYOkulDbContext _context;
        public HomeController(ILogger<HomeController> logger, EYOkulDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string UserUserName, string Password)
        {
            UserModel userModel = new UserModel();
            string hashPassword = userModel.HashPassword(Password);
            var model = _context.TBL_A_USERS.Where(x => x.UserUserName == UserUserName && x.Password == hashPassword).FirstOrDefault();
            if (model != null)
            {
                if (!model.IsActive)
                {
                    TempData["Alert"] = "Hesabınız Pasif Durumdadır.Lütfen Göervliye Başvurunuz";
                    return View();
                }
                else if (model.UserType != 4)
                {
                    TempData["Alert"] = "Bu Hizmet İçin Yetkiniz Bulunmamaktadır.";
                    return View();
                }

                HttpContext.Session.SetString("Kullanici",model.UserName);
                HttpContext.Session.SetInt32("SchoolId", model.SchoolId);
                HttpContext.Session.SetInt32("SysUserId", model.Id);
                
                string name = model.UserName + " " + model.UserSurName;
                HttpContext.Session.SetString("FullName",name);
                ViewBag.userName = model.UserName;
                var school = _context.TBL_SCOOLS.Where(x=>x.Id == model.SchoolId).FirstOrDefault();
                ViewBag.schoolName = school.ScoolName;
                TempData["Id"] = model.Id;
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,model.UserName),
                    new Claim("UserName",model.UserName),
                    new Claim("SchoolName",school.ScoolName),
                    new Claim("UserFullName",name),
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties authentication = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authentication);

                return RedirectToAction("Index","Dashboard");

            }
            else
            {
                TempData["Alert"] = "Kullanıcı Bulunamadı.";
                return View();
            }



        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Alert"] = "Başarıyla Çıkış Yaptınız.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        
    }
}