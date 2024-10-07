using Microsoft.AspNetCore.Identity;

namespace Twitter.Application.Models;

public class User : IdentityUser<Guid>
{ 
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public string? Image { get; set; } = default!;
    
    public ICollection<Post> Posts = new List<Post>();

    public ICollection<User> Followers = [];
    public ICollection<Follow> FollowFollowers = [];
    public ICollection<User> Followees = [];
    public ICollection<Follow> FollowFollowees = [];

}