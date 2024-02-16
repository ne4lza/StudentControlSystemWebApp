namespace EYOkulProjectWebUI.Models
{
    public class TerminalModel
    {
        public int Id { get; set; }
        public string? TerminalName { get; set; }
        public string? TerminalNum { get; set; }
        public string? TerminalIp { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int SysUserId { get; set; }
    }
}
