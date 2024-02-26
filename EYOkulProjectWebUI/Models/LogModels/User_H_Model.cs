namespace EYOkulProjectWebUI.Models.LogModels
{
    public class User_H_Model
    {
        public int Id { get; set; }
        public string? ProccessType { get; set; }
        public string? UserName { get; set; }
        public string? UserSurName { get; set; }
        public string? UserUserName { get; set; }
        public string? Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int SysUserId { get; set; }
        public int UserType { get; set; }
        public int SchoolId { get; set; }
    }
}
