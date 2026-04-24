using System;
using blogger_clone.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace blogger_clone.Config;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.Id);

        builder.Property(t => t.Email)
        .IsRequired();

        builder.Property(t => t.Username)
        .IsRequired();
        builder.HasIndex(t => t.Username);
    }
}
