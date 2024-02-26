namespace EYOkulProjectWebUI.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string StudentNameSurName { get; set; }
        public string StudentClass { get; set; }
        public string GuardianNameSurName { get; set; }
        public string Image { get; set; }
        public DateTime? InsertedDate { get; set; }
    }
}
