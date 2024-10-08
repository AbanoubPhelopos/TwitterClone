﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twitter.Application.Models;

namespace Twitter.Application.Databases.DbConfiguration;

public class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasOne(p => p.Author).WithMany(u => u.Posts).HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.OriginalPost).WithOne().HasForeignKey<Post>(p=>p.OriginalPostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}