using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using EYOkulProjectWebUI.Models.LogModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace EYOkulProjectWebUI.Controllers
{
    public class FileController : Controller
    {
        private readonly EYOkulDbContext _context;

        public FileController(EYOkulDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(IFormFile file)
        {
            List<string> errorMessages = new List<string>();
            if (file == null || file.Length == 0)
            {
                TempData["Alert"] = "Dosya seçilmedi.";
                return View();
            }
            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xlsx" && fileExtension != ".xls")
            {
                TempData["Alert"] = "Geçersiz dosya formatı. Lütfen bir Excel dosyası (.xlsx veya .xls) yükleyin.";
                return View();
            }
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Excel dosyasındaki ilk sayfa

                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Başlık satırını atlayarak işlem yapın
                    {
                        // Örneğin, Excel'de sütunların sırasını varsayalım: 
                        // 1. Sütun: Öğrenci Adı, 2. Sütun: Öğrenci Soyadı, 3. Sütun: Öğrenci Numarası,
                        // 4. Sütun: Öğrenci TCKN, 5. Sütun: Sınıf Adı
                        string studentName = worksheet.Cells[row, 1].Value?.ToString();
                        string studentSurName = worksheet.Cells[row, 2].Value?.ToString();
                        string studentNumber = worksheet.Cells[row, 3].Value?.ToString();
                        decimal studentTckn = Convert.ToDecimal(worksheet.Cells[row, 4].Value);
                        string className = worksheet.Cells[row, 5].Value?.ToString();
                        int? classId = _context.TBL_CLASS
                        .Where(x => x.ClassName == className)
                        .Select(x => (int?)x.Id)
                    .   FirstOrDefault();

                        var existingTckn = _context.TBL_STUDENTS.FirstOrDefault(s => s.StudentTckn == studentTckn);
                        if (existingTckn != null)
                        {
                            // Öğrenci zaten var, kullanıcıya göster
                            errorMessages.Add($"!! {studentTckn} TCKN'li öğrenci daha önce kayıt edilmiş.");
                            continue; // Bir sonraki öğrenciye geç
                        }
                        var existingStudentNumber = _context.TBL_STUDENTS.FirstOrDefault(s => s.StudentTckn == studentTckn);
                        if (existingStudentNumber != null)
                        {
                            // Öğrenci zaten var, kullanıcıya göster
                            errorMessages.Add($"!! {studentNumber} okul numaralı öğrenci öğrenci daha önce kayıt edilmiş.");
                            continue; // Bir sonraki öğrenciye geç
                        }
                        errorMessages.Add($"✓ {studentTckn} TCKN'li öğrenci sisteme dahil edildi.");
                        // Veritabanına kaydetmek için bir öğrenci nesnesi oluşturun ve değerleri atayın
                        StudentsModel student = new StudentsModel()
                        {
                            StudentName = studentName,
                            StudentSurName = studentSurName,
                            StudentNumber = studentNumber,
                            StudentTckn = studentTckn,
                            ScoolId = (int)HttpContext.Session.GetInt32("SchoolId"),
                            ClassId = Convert.ToInt32(classId),
                            IsActive = true,
                            IsDeleted = false,
                            InsertedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            SysUserId = (int)HttpContext.Session.GetInt32("SysUserId"),
                        };
                        _context.TBL_STUDENTS.Add(student);
                        Student_H_Model student_H_Model = new Student_H_Model()
                        {
                            ProccessType = "INSERT",
                            StudentName = studentName,
                            StudentSurName = studentSurName,
                            StudentNumber = studentNumber,
                            StudentTckn = studentTckn,
                            ScoolId = (int)HttpContext.Session.GetInt32("SchoolId"),
                            ClassId = Convert.ToInt32(classId),
                            IsActive = true,
                            IsDeleted = false,
                            InsertedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            SysUserId = (int)HttpContext.Session.GetInt32("SysUserId"),
                        };
                        _context.TBL_H_STUDENTS.Add(student_H_Model);
                    }
                    _context.SaveChanges();
                    // Değişiklikleri veritabanına kaydet

                }
            }
            if (errorMessages.Any())
            {
                return View(errorMessages);
            }
            TempData["Alert"] = "İşlem tamamlandı.";
            return View();
        }

        [HttpPost]
        public IActionResult TopluSinifGuncelle(IFormFile file)
        {
            List<string> errorMessages = new List<string>();
            if (file == null || file.Length == 0)
            {
                TempData["Alert"] = "Dosya seçilmedi.";
                return View("Index");
            }
            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xlsx" && fileExtension != ".xls")
            {
                TempData["Alert"] = "Geçersiz dosya formatı. Lütfen bir Excel dosyası (.xlsx veya .xls) yükleyin.";
                return View("Index");
            }
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Excel dosyasındaki ilk sayfa

                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Başlık satırını atlayarak işlem yapın
                    {
                        decimal studentTckn = Convert.ToDecimal(worksheet.Cells[row, 1].Value);
                        string className = worksheet.Cells[row, 2].Value?.ToString();
                        int? classId = _context.TBL_CLASS
                        .Where(x => x.ClassName == className)
                        .Select(x => (int?)x.Id)
                    .FirstOrDefault();
                        var missingTckn = _context.TBL_STUDENTS.FirstOrDefault(s => s.StudentTckn == studentTckn);
                        if (missingTckn == null)
                        {
                            // Öğrenci zaten var, kullanıcıya göster
                            errorMessages.Add($"{studentTckn} TCKN'li öğrenci sistemde mevcut değil.");
                            continue; // Bir sonraki öğrenciye geç
                        }
                        // Veritabanına kaydetmek için bir öğrenci nesnesi oluşturun ve değerleri atayın
                        var student = _context.TBL_STUDENTS.Where(x=>x.StudentTckn == studentTckn).FirstOrDefault();
                        student.ClassId = (int)classId;

                        _context.TBL_STUDENTS.Update(student);
                        Student_H_Model student_H_Model = new Student_H_Model()
                        {
                            ProccessType = "UPDATE",
                            StudentTckn = studentTckn,
                            ScoolId = (int)HttpContext.Session.GetInt32("SchoolId"),
                            ClassId = Convert.ToInt32(classId),
                            UpdatedDate = DateTime.UtcNow,
                            SysUserId = (int)HttpContext.Session.GetInt32("SysUserId"),
                        };
                        _context.TBL_H_STUDENTS.Add(student_H_Model);
                    }

                    _context.SaveChanges();
                }
            }
            if (errorMessages.Any())
            {
                return View("Index", errorMessages);
            }
            TempData["Alert"] = "İşlem tamamlandı.";
            return View("Index");
        }

    }
}
