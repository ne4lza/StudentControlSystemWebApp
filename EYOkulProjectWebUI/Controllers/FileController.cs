using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
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
        public IActionResult ReadExcelFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Dosya seçilmedi.";
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

                        // Veritabanına öğrenciyi ekle
                        _context.TBL_STUDENTS.Add(student);
                    }

                    // Değişiklikleri veritabanına kaydet
                    _context.SaveChanges();
                }
            }
            ViewBag.Message = "İşlem tamamlandı.";
            return RedirectToAction("Index");
        }

    }
}
