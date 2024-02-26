namespace EYOkulProjectWebUI.Models.LogModels
{
    public class Student_H_Model
    {
        public int Id { get; set; }
        public string ProccessType { get; set; }
        public String StudentName { get; set; }
        public String StudentSurName { get; set; }
        public String StudentNumber { get; set; }
        public decimal StudentTckn { get; set; }
        public int ScoolId { get; set; }
        public int ClassId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int SysUserId { get; set; }
    }
}
