using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using EYOkulProjectWebUI.ViewModels.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection;
namespace EYOkulProjectWebUI.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly EYOkulDbContext _context;

        public StudentController(EYOkulDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.classList = _context.TBL_CLASS.Where(x => x.ScoolId == HttpContext.Session.GetInt32("SchoolId")).ToList();
            var query = from student in _context.TBL_STUDENTS
                        join school in _context.TBL_SCOOLS on student.ScoolId equals school.Id
                        join classID in _context.TBL_CLASS on student.ClassId equals classID.Id
                        where (student.ScoolId == HttpContext.Session.GetInt32("SchoolId"))
                        select new StudentsModel
                        {
                            Id = student.Id,
                            StudentName = student.StudentName,
                            StudentSurName = student.StudentSurName,
                            StudentNumber = student.StudentNumber,
                            StudentTckn = student.StudentTckn,
                            IsActive = student.IsActive,
                            UpdatedDate = student.UpdatedDate,
                            SysUserId = student.SysUserId,
                            IsDeleted = student.IsDeleted,
                            InsertedDate = student.InsertedDate,
                            ScoolId = student.ScoolId,
                            ClassId = student.ClassId,
                            SchollName = school.ScoolName,
                            ClassName = classID.ClassName
                        };

            StudentViewModel studentViewModel = new StudentViewModel()
            {
                classList = _context.TBL_CLASS.Where(x => x.ScoolId == HttpContext.Session.GetInt32("SchoolId")).ToList(),
                studentList = query.ToList(),
                studentsModel = new StudentsModel()

            };
            return View(studentViewModel);
        }
        [HttpPost]
        public IActionResult Index(int selectClass)
        {
            ViewBag.classList = _context.TBL_CLASS.Where(x => x.ScoolId == HttpContext.Session.GetInt32("SchoolId")).ToList();
            if (selectClass == -1)
                return RedirectToAction("Index");

            var query = from student in _context.TBL_STUDENTS
                        join school in _context.TBL_SCOOLS on student.ScoolId equals school.Id
                        join classID in _context.TBL_CLASS on student.ClassId equals classID.Id
                        where (student.ScoolId == HttpContext.Session.GetInt32("SchoolId") && student.ClassId == selectClass)
                        select new StudentsModel
                        {
                            Id = student.Id,
                            StudentName = student.StudentName,
                            StudentSurName = student.StudentSurName,
                            StudentNumber = student.StudentNumber,
                            StudentTckn = student.StudentTckn,
                            IsActive = student.IsActive,
                            UpdatedDate = student.UpdatedDate,
                            SysUserId = student.SysUserId,
                            IsDeleted = student.IsDeleted,
                            InsertedDate = student.InsertedDate,
                            ScoolId = student.ScoolId,
                            ClassId = student.ClassId,
                            SchollName = school.ScoolName,
                            ClassName = classID.ClassName

                        };

            StudentViewModel studentViewModel = new StudentViewModel()
            {
                classList = _context.TBL_CLASS.Where(x => x.ScoolId == HttpContext.Session.GetInt32("SchoolId")).ToList(),
                studentList = query.ToList(),

            };
            return View(studentViewModel);
        }
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(StudentViewModel studentViewModel)
        {
            StudentsModel studentModel = new StudentsModel()
            {
                StudentName = studentViewModel.studentsModel.StudentName,
                StudentSurName = studentViewModel.studentsModel.StudentSurName,
                StudentNumber = studentViewModel.studentsModel.StudentNumber,
                StudentTckn = studentViewModel.studentsModel.StudentTckn,
                ClassId = studentViewModel.studentsModel.ClassId,
                ScoolId = (int)HttpContext.Session.GetInt32("SchoolId"),
                IsActive = true,
                IsDeleted = false,
                InsertedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                SysUserId = (int)HttpContext.Session.GetInt32("SysUserId")
            };

            var control = _context.TBL_STUDENTS.Where(x => x.StudentTckn == studentViewModel.studentsModel.StudentTckn || x.StudentNumber == studentModel.StudentNumber).FirstOrDefault();
            if (control != null)
            {
                TempData["Alert"] = "Aynı TCKN Veya Öğrenci Numarasına Sahip Bir Öğrenci Daha Önce Eklenmiş.";
                return RedirectToAction("Index", "Student"); ;
            }
            else
            {
                _context.TBL_STUDENTS.Add(studentModel);
                _context.SaveChanges();
                TempData["Alert"] = "Öğrenci Ekleme İşlemi Başarıyla Tamamlandı.";
                return RedirectToAction("Index", "Student");
            }
        }

        [HttpPost]
        public IActionResult UpdateStudent(StudentsModel updatedStudent)
        {
            var existingStudent = _context.TBL_STUDENTS.Find(updatedStudent.Id);

            if (existingStudent == null)
            {
                return NotFound();
            }

            List<string> updatedFields = new List<string>();

            PropertyInfo[] properties = typeof(StudentsModel).GetProperties();
            foreach (var property in properties)
            {
                var existingValue = property.GetValue(existingStudent);
                var updatedValue = property.GetValue(updatedStudent);

                if (!object.Equals(existingValue, updatedValue))
                {
                    updatedFields.Add(property.Name);
                    property.SetValue(existingStudent, updatedValue);
                }
            }

            existingStudent.UpdatedDate = DateTime.Now;

            _context.SaveChanges();

            LogModel log = new LogModel()
            {
                ActivityType = "UPDATE",
                SysUserId = (int)HttpContext.Session.GetInt32("SysUserId"),
                RecordId = updatedStudent.Id,
                RecordName = "STUDENT",
                UpdatedFields = string.Join(",", updatedFields),
                IsActive = updatedStudent.IsActive,
                IsDeleted = updatedStudent.IsDeleted,
                EventTime = DateTime.Now,
            };
            _context.TBL_LOGS.Add(log);
            _context.SaveChanges();

            TempData["Alert"] = "Öğrenci Güncelleme İşlemi Tamamlandı.";
            return RedirectToAction("Index", "Student");
        }


    }
}
