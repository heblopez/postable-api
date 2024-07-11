using Microsoft.EntityFrameworkCore;
using PostableApi.Models;

namespace PostableApi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Likes)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Likes)
            .WithOne(l => l.Post)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Like>()
            .HasOne(l => l.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Like>()
            .HasIndex(l => new { l.PostId, l.UserId })
            .IsUnique();
    }
}