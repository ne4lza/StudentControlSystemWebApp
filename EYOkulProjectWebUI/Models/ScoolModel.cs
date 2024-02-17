namespace EYOkulProjectWebUI.Models
{
    public class ScoolModel
    {
        public int Id { get; set; }
        public string ScoolName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime Updatedate { get; set; }
        public int SysUserId { get; set; }
    }
}
