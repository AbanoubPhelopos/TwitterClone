using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twitter.Application.Models;

namespace Twitter.Application.Databases.DbConfiguration;

public class LikesConfig : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(L => new { L.PostId, L.LikerId });
        builder.HasOne(L => L.Liker).WithMany().HasForeignKey(L => L.LikerId).OnDelete(DeleteBehavior.Cascade);
    }
}