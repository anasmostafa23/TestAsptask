using CafeOrderSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CafeOrderSystem.Data.Context;
public class CafeDbContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    
    public CafeDbContext(DbContextOptions<CafeDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);
    }
}