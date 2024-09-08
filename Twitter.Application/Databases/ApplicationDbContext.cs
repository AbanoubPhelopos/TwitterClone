using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Twitter.Application.Models;

namespace Twitter.Application.Databases;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext))!);
        base.OnModelCreating(builder);
    }
}