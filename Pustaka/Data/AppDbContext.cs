using Microsoft.EntityFrameworkCore;
using Pustaka.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Penulis> Penulis { get; set; }
    public DbSet<Buku> Buku { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Penulis>()
            .HasMany(a => a.Buku)
            .WithOne(b => b.Penulis);
    }
}
