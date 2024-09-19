namespace Twitter.Application.Models;

public class Comment :BaseEntity
{
    public Guid WriterId { get; set; }
    public Guid PostId { get; set; }
    
    public Post Post { get; set; }
    public User Writer { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}