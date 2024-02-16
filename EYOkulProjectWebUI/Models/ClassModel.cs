namespace EYOkulProjectWebUI.Models
{
    public class ClassModel
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string Desciription { get; set; }
        public int ScoolId { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int SysUserId { get; set; }  
    }
}
