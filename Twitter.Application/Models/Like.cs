namespace Twitter.Application.Models;

public class Like 
{
    public Guid PostId { get; set; }
    public Guid LikerId { get; set; }
    
    public User Liker { get; set; }
    public Post Post { get; set; }
}