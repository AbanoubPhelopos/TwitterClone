using Microsoft.AspNetCore.Identity;

namespace Twitter.Application.Models;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    
    public ICollection<Post> Posts = new List<Post>();
}