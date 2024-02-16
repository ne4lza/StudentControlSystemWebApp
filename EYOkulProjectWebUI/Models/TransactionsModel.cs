using System.ComponentModel.DataAnnotations.Schema;

namespace EYOkulProjectWebUI.Models
{
    public class TransactionsModel
    {
        public int Id { get; set; }
        public int ScoolId { get; set; }
        public int ClassId { get; set; }
        public int CardId { get; set; }
        public int StudentId { get; set; }

        [NotMapped]
        public int? GuardianId { get; set; }
        [NotMapped]
        public int ImageId { get; set; }
        [NotMapped]
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string? InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [NotMapped]
        public int Type { get; set; }

        ///////////////////////////////
        public string SchoolName { get; set; }
        public string ClassName { get; set; }
        public string StudentName { get; set; }
        public string StudentSurName { get; set; }
        public string CardNumber { get; set; }
        public string TerminalName { get; set; }
        public TimeSpan? InsertedTime { get; set; }
    }
}
