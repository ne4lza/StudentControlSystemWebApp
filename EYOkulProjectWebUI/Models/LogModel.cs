namespace EYOkulProjectWebUI.Models
{
    public class LogModel
    {
        public int Id { get; set; }
        public string? ActivityType { get; set; }
        public int SysUserId { get; set; }
        public int RecordId { get; set; }
        public string? RecordName { get; set; }
        public string? UpdatedFields { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime EventTime { get; set; }
    }
}
