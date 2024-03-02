using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

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
            ViewBag.type = _context.TBL_TYPES.ToList();
            var school = new SqlParameter("@SchoolId", HttpContext.Session.GetInt32("SchoolId"));

            var query = _context.TBL_CARDS
                                      .FromSqlRaw("EXEC sp_Cards @SchoolId", school)
                                      .ToList();
            return View(query);
        }

        [HttpPost]
        public IActionResult AddCard(CardModel cardModel)
        {
            var userFirstName = new SqlParameter("@UserName", cardModel.UserName);
            var userLastName = new SqlParameter("@UserLastName", cardModel.UserLastName);
            var phoneNumber = new SqlParameter("@PhoneNumber", cardModel.PhoneNumber);
            var mail = new SqlParameter("@Mail", cardModel.Mail);
            var tckn = new SqlParameter("@Tckn", cardModel.Tckn);
            var cardNum = new SqlParameter("@CardNum", cardModel.CardNum);
            var userType = new SqlParameter("@UserType", cardModel.UserType);
            var schoolId = new SqlParameter("@ScoolId", HttpContext.Session.GetInt32("SchoolId"));

            var student = _context.TBL_STUDENTS.Where(x => x.StudentTckn == cardModel.StudentTckn).FirstOrDefault();
            if(student == null)
            {
                TempData["Alert"] = "Girmiş Olduğunuz TCKN'ye Ait Bir Öğrenci Bulunamadı.";
                return RedirectToAction("Index");
            }
            var studentId = new SqlParameter("@StudentId", Convert.ToInt32(student.Id));
            var classId = new SqlParameter("@ClassId", Convert.ToInt32(student.ClassId));
            var sysUser = _context.TBL_A_USERS.Where(x => x.UserUserName == cardModel.UserUserName).FirstOrDefault();
            if(sysUser == null) 
            {
                TempData["Alert"] = "Girmiş Olduğunuz Kullanıcı Adına Ait Bir Kullanıcı Bulunamadı.";
                return RedirectToAction("Index");
            }
            var userId = new SqlParameter("@UserId", Convert.ToInt32(sysUser.Id));
            var sysUserId = new SqlParameter("@SysUserId", HttpContext.Session.GetInt32("SysUserId"));
            _context.Database.ExecuteSqlRaw("EXEC sp_InsertCard @UserName, @UserLastName, @PhoneNumber, @Mail, @Tckn, @CardNum, @UserType, @ScoolId, @SysUserId, @StudentId, @ClassId, @UserId",
                                      userFirstName, userLastName, phoneNumber, mail, tckn, cardNum, userType, schoolId, sysUserId, studentId, classId, userId);
            TempData["Alert"] = "Kart Başarıyla Eklendi.";
            return RedirectToAction("Index");
        }

    }
}