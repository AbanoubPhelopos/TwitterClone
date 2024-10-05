namespace Twitter.Application.Models
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Read { get; set; }
        public string Message { get; set; }  // Notification content
        public string Type { get; set; }  // Notification type, e.g., "NewComment"
        public Guid? RelatedEntityId { get; set; }  // Related entity (e.g., post or comment)
        public string RelatedEntityType { get; set; }  // Entity type (e.g., "Post", "Comment")
        public bool IsActionable { get; set; }  // If there's an action associated with the notification
    }
}