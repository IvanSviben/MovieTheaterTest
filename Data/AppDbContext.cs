using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<User, Role, int >
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Category> Categories{ get; set; } = null!;
    public DbSet<MovieCategory> movieCategories{ get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MovieCategory>()
            .HasKey(mc => new { mc.MovieId, mc.CategoryId });

        builder.Entity<MovieCategory>()
            .HasOne(mc => mc.Movie)
            .WithMany(m => m.MovieCategories)
            .HasForeignKey(mc => mc.MovieId);

        builder.Entity<MovieCategory>()
            .HasOne(mc => mc.Category)
            .WithMany(c => c.MovieCategories)
            .HasForeignKey(mc => mc.CategoryId);
        
        builder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}