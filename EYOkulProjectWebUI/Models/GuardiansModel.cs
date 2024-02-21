using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace EYOkulProjectWebUI.Models
{
        public class GuardiansModel 
        {
            public int Id { get; set; }

            public string GuardianName { get; set; }
            public string GuardianSurName { get; set; }
            public decimal GuardianPhoneNumber { get; set; }
            public string GuardianMail { get; set; }
            public decimal GuardianTckn { get; set; }
            public int StudentId { get; set; }
            public int ClassId { get; set; }
            public int ScoolId { get; set; }
            public string CardNum { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime? InsertedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public int SysUserId { get; set; }
            public int UserId { get; set; }
            public int Type { get; set; }

        ////
        
    }
    }
