using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Claims;

namespace EYOkulProjectWebUI.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string UserName, string Password)
        {
            using var cotnext = new EYOkulDbContext();
            var model = cotnext.TBL_A_USERS.Where(x => x.UserUserName == UserName && x.Password == Password).FirstOrDefault();
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
                TempData["SchoolId"] = model.SchoolId;
                TempData["UserName"] = model.UserName;
                var school = cotnext.TBL_SCOOLS.Where(x=>x.Id == model.SchoolId).FirstOrDefault();
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