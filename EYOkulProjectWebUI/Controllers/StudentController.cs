using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using EYOkulProjectWebUI.ViewModels.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                        where(student.ScoolId == HttpContext.Session.GetInt32("SchoolId"))
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
                StudentSurName= studentViewModel.studentsModel.StudentSurName,
                StudentNumber = studentViewModel.studentsModel.StudentNumber,
                StudentTckn= studentViewModel.studentsModel.StudentTckn,
                ClassId= studentViewModel.studentsModel.ClassId,
                ScoolId = (int) HttpContext.Session.GetInt32("SchoolId"),
                IsActive = true,
                IsDeleted = false,
                InsertedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                SysUserId =(int) HttpContext.Session.GetInt32("SysUserId")
            };

            var control = _context.TBL_STUDENTS.Where(x => x.StudentTckn == studentViewModel.studentsModel.StudentTckn || x.StudentNumber == studentModel.StudentNumber).FirstOrDefault();
            if(control != null)
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
        [HttpGet]
        public IActionResult UpdateStudent(int id)
        {
            ViewBag.classList = _context.TBL_CLASS.Where(x => x.ScoolId == HttpContext.Session.GetInt32("SchoolId")).ToList();
            var student = _context.TBL_STUDENTS.Where(x=>x.Id == id).FirstOrDefault();
            return PartialView("_UpdateStudentModal",student);
        }
        [HttpPost]
        public IActionResult UpdateStudent(StudentsModel student)
        {
            student.UpdatedDate = DateTime.Now;
            _context.TBL_STUDENTS.Update(student);
            _context.SaveChanges();
            TempData["Alert"] = "Öğrenci Güncelleme İşlemi Tamamlandı.";
            return RedirectToAction("Index", "Student");
        }
    }
}
