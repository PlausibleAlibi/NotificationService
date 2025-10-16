using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Data;

/// <summary>
/// Database context for the Notification Service
/// </summary>
public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Tenants in the system
    /// </summary>
    public DbSet<Tenant> Tenants { get; set; }

    /// <summary>
    /// Notifications/alerts
    /// </summary>
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Tenant entity
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Code).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        });

        // Configure Notification entity
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");

            // Configure relationship with Tenant
            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Notifications)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add index for common queries
            entity.HasIndex(e => new { e.TenantId, e.IsActive });
        });

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed a default tenant for demo purposes
        modelBuilder.Entity<Tenant>().HasData(
            new Tenant
            {
                Id = 1,
                Code = "default",
                Name = "Default Tenant",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
