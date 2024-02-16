using EYOkulProjectWebUI.DAL;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace EYOkulProjectWebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IToastNotification _toastNotification;

        public LoginController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string UserName,string Password)
        {
            using var cotnext = new EYOkulDbContext();
            var model = cotnext.TBL_A_USERS.Where(x => x.UserUserName == UserName && x.Password == Password).FirstOrDefault();
            if(model != null)
            {
                if(!model.IsActive) 
                {
                    _toastNotification.AddErrorToastMessage("Hesabınız Pasif Durumdadır.Lütfen Göervliye Başvurunuz");
                    return View();
                }
                else if(model.SysUserId != 4)
                {
                    _toastNotification.AddErrorToastMessage("Bu Hizmet İçin Yetkiniz Bulunmamaktadır.");
                    return View();
                }
                
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Kullanıcı Bulunamadı.");
                return View();
            }
            return View();
           


        }
    }
}
