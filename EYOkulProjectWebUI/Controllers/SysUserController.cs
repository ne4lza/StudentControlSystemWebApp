using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using EYOkulProjectWebUI.Models.LogModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EYOkulProjectWebUI.Controllers
{
    [Authorize]
    public class SysUserController : Controller
    {
        private readonly EYOkulDbContext _context;

        public SysUserController(EYOkulDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.typeList = _context.TBL_TYPES.ToList();
            var model = from user in _context.TBL_A_USERS
                        join school in _context.TBL_SCOOLS on user.SchoolId equals school.Id
                        join userType in _context.TBL_TYPES on user.UserType equals userType.Id
                        where (user.SchoolId == HttpContext.Session.GetInt32("SchoolId"))
                        select new UserModel
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            UserSurName = user.UserSurName,
                            UserUserName = user.UserName,
                            IsActive = user.IsActive,
                            IsDeleted = user.IsDeleted,
                            InsertedDate = user.InsertedDate,
                            UpdateDate = user.UpdateDate,
                            SysUserId = user.SysUserId,

                            UserType = user.UserType,
                            UserTypeName = userType.Tip,

                            SchoolId = user.SchoolId,
                            SchoolName = school.ScoolName,

                        };
            return View(model.ToList());
        }
        [HttpPost]
        public IActionResult Index(int selectType)
        {
            ViewBag.typeList = _context.TBL_TYPES.ToList();
            if (selectType == -1)
                return RedirectToAction("Index");

            var model = from user in _context.TBL_A_USERS
                        join school in _context.TBL_SCOOLS on user.SchoolId equals school.Id
                        join userType in _context.TBL_TYPES on user.UserType equals userType.Id
                        where (user.SchoolId == HttpContext.Session.GetInt32("SchoolId") && user.UserType == selectType)
                        select new UserModel
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            UserSurName = user.UserSurName,
                            UserUserName = user.UserName,
                            IsActive = user.IsActive,
                            IsDeleted = user.IsDeleted,
                            InsertedDate = user.InsertedDate,
                            UpdateDate = user.UpdateDate,
                            SysUserId = user.SysUserId,

                            UserType = user.UserType,
                            UserTypeName = userType.Tip,

                            SchoolId = user.SchoolId,
                            SchoolName = school.ScoolName,

                        };
            return View(model.ToList());
        }

            [HttpPost]
        public IActionResult AddSysUser(UserModel userModel)
        {
            UserModel model = new UserModel()
            {
                UserName = userModel.UserName,
                UserSurName = userModel.UserSurName,
                UserUserName = userModel.UserName,
                Password = userModel.HashPassword(userModel.Password),
                IsActive = true,
                IsDeleted = false,
                InsertedDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                SysUserId = (int)HttpContext.Session.GetInt32("SysUserId"),
                UserType = userModel.UserType,
                SchoolId = (int)HttpContext.Session.GetInt32("SchoolId"),
            };
            string confirmPasswordHash = userModel.HashPassword(userModel.ConfirmPassword);
            if(ModelState.IsValid) 
            {
                if (userModel.Password != confirmPasswordHash)
                {
                    TempData["Alert"] = "Şifreler Eşleşmiyor.";
                    return RedirectToAction("Index");
                }
                else
                {
                    var existingUser = _context.TBL_A_USERS.Where(x => x.UserUserName == userModel.UserUserName).FirstOrDefault();
                    if (existingUser == null)
                    {
                        User_H_Model user_H_Model = new User_H_Model()
                        {
                            ProccessType = "INSERT",
                            UserName = userModel.UserName,
                            UserSurName = userModel.UserSurName,
                            UserUserName = userModel.UserName,
                            Password = userModel.HashPassword(userModel.Password),
                            IsActive = true,
                            IsDeleted = false,
                            InsertedDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            SysUserId = (int)HttpContext.Session.GetInt32("SysUserId"),
                            UserType = userModel.UserType,
                            SchoolId = (int)HttpContext.Session.GetInt32("SchoolId"),
                        };
                        _context.TBL_A_H_USERS.Add(user_H_Model);
                        _context.Add(model);
                        _context.SaveChanges();
                        TempData["Alert"] = "Kullanıcı Başarıyla Eklendi.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Alert"] = "Bu Kullanıcı Adı Sistemde Mevcut.";
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                TempData["Alert"] = "Lütfen Alanları Boş Bırakmadığınızdan Emin Olun.";
                return RedirectToAction("Index");
            }
            
            
        }
    }
}
