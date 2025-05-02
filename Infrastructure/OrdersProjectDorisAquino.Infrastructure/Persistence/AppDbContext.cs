using Microsoft.EntityFrameworkCore;
using OrdersProjectDorisAquino.Domain.Entities;
using System.Diagnostics;

namespace OrdersProjectDorisAquino.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(e => e.OrderID);
            
            entity.Property(e => e.Freight).HasColumnType("money");
            entity.Property(e => e.OrderDate).IsRequired();
            entity.Property(o => o.ShipRegion).IsRequired(false);
            entity.Property(o => o.ShipPostalCode).IsRequired(false);
            entity.Property(o => o.ShippedDate).IsRequired(false);
            entity.Property(o => o.ShipName).HasDefaultValue("");
            entity.Property(o => o.ShipAddress).HasDefaultValue("");
            entity.Property(o => o.ShipCity).HasDefaultValue("");
            entity.Property(o => o.ShipCountry).HasDefaultValue("");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("Order Details");
            entity.HasKey(e => new { e.OrderID, e.ProductID });
            
            entity.Property(e => e.UnitPrice)
                .HasColumnType("money")
                .HasPrecision(19, 4);
            
            entity.Property(e => e.Discount)
                .HasColumnType("real")
                .HasDefaultValue(0f);
            
            entity.Property(e => e.Quantity)
                .IsRequired()
                .HasDefaultValue((short)1);

            // Relación con Order
            entity.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con Product
            entity.HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.UnitPrice).HasColumnType("money");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.CustomerId);
            entity.Property(c => c.CustomerId).IsFixedLength().HasMaxLength(5);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeID);
        });
    }
}