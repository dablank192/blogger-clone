using System;
using blogger_clone.Model;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> option) : base (option) {}

    public DbSet<User> User {get; set;}
    public DbSet<Post> Post {get; set;}
    public DbSet<Blog> Blog {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
