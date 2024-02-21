using System.ComponentModel.DataAnnotations.Schema;

namespace EYOkulProjectWebUI.Models
{
    public class CardModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public decimal PhoneNumber { get; set; }
        public string Mail { get; set; }
        public decimal Tckn { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int ScoolId { get; set; }
        public string CardNum { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int SysUserId { get; set; }
        public int UserType { get; set; }
        public int UserId { get; set; }


        ////////////////////////////////////
        public string? ScoolName { get; set; }
        public string StudentName { get; set; }
        public string StudentSurName { get; set; }
        public string UserTypeName { get; set; }
        public string ClassName { get; set; }
    }
}
