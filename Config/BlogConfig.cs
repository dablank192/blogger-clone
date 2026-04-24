using System;
using blogger_clone.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace blogger_clone.Config;

public class BlogConfig : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.ToTable("Blog");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.UserId);
        
        builder.HasIndex(t => t.SubDomain);

        builder.HasOne(t => t.User)
        .WithMany(t => t.Blog)
        .HasForeignKey(t => t.UserId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
