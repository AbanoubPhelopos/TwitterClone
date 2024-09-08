namespace Twitter.Application.Models;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Read { get; set; }
}