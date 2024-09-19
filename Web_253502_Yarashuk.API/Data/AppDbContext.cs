using Microsoft.EntityFrameworkCore;
using Web_253502_Yarashuk.Domain.Entities;

namespace Web_253502_Yarashuk.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Коллекции сущностей предметной области
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка сущностей, если нужно, например, индексы, уникальные ключи и т.д.
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.NormalizedName)
            .IsUnique();

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
