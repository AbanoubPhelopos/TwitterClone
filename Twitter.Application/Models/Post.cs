namespace Twitter.Application.Models;

public class Post : BaseEntity
{
    public Guid AuthorId { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;

    public string[]? Images { get; set; }
    public Guid? OriginalPostId { get; set; }
    public DateTime PostedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 
    public Post? OriginalPost { get; set; }
    public User Author { get; set; } = default!;
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}