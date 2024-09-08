using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twitter.Application.Models;

namespace Twitter.Application.Databases.DbConfiguration;

public class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasOne(c => c.Writer).WithMany().HasForeignKey(c => c.WriterId).OnDelete(DeleteBehavior.Cascade);
    }
}