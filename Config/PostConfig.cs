using System;
using blogger_clone.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace blogger_clone.Config;

public class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Post");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.Id);

        builder.HasIndex(t => t.BlogId);

        builder.Property(t => t.CreatedAt)
        .IsRequired(false);

        builder.HasOne(t => t.Blog)
        .WithMany(t => t.Post)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
