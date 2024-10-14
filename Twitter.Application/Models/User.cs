public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Image { get; set; } = default!;
    
    public ICollection<Post> Posts { get; set; } = new List<Post>();

    public ICollection<Follow> Followers { get; set; } = new List<Follow>();
    public ICollection<Follow> Followees { get; set; } = new List<Follow>();
}