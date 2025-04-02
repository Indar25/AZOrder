using Microsoft.EntityFrameworkCore;
using Order_Domain.Domain;

namespace Order_Persistence;

public class OrderDBContext : DbContext
{
    public OrderDBContext(DbContextOptions opt) : base(opt)
    {

    }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CustomerEmail).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.CustomerEmail).IsUnique();
        });
    }
}

