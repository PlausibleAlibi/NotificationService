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

    /// <summary>
    /// Applications within tenants
    /// </summary>
    public DbSet<Application> Applications { get; set; }

    /// <summary>
    /// Environments for deployment targeting
    /// </summary>
    public DbSet<Domain.Entities.Environment> Environments { get; set; }

    /// <summary>
    /// Notification templates
    /// </summary>
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; }

    /// <summary>
    /// Notification schedules
    /// </summary>
    public DbSet<NotificationSchedule> NotificationSchedules { get; set; }

    /// <summary>
    /// Targeting rules for notifications
    /// </summary>
    public DbSet<TargetingRule> TargetingRules { get; set; }

    /// <summary>
    /// Notification history/audit trail
    /// </summary>
    public DbSet<NotificationHistory> NotificationHistories { get; set; }

    /// <summary>
    /// Notification acknowledgments
    /// </summary>
    public DbSet<NotificationAcknowledgment> NotificationAcknowledgments { get; set; }

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

            // Configure relationship with Template
            entity.HasOne(e => e.Template)
                .WithMany()
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure relationship with Application
            entity.HasOne(e => e.Application)
                .WithMany(a => a.Notifications)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Application entity
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Applications)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
        });

        // Configure Environment entity
        modelBuilder.Entity<Domain.Entities.Environment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Environments)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
        });

        // Configure NotificationTemplate entity
        modelBuilder.Entity<NotificationTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Templates)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
        });

        // Configure NotificationSchedule entity
        modelBuilder.Entity<NotificationSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TimeZone).HasMaxLength(100);
            entity.Property(e => e.RecurrenceDaysOfWeek).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Notification)
                .WithMany(n => n.Schedules)
                .HasForeignKey(e => e.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure TargetingRule entity
        modelBuilder.Entity<TargetingRule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TargetEnvironment).HasMaxLength(50);
            entity.Property(e => e.TargetUserGroup).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Notification)
                .WithMany(n => n.TargetingRules)
                .HasForeignKey(e => e.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.TargetApplication)
                .WithMany()
                .HasForeignKey(e => e.TargetApplicationId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure NotificationHistory entity
        modelBuilder.Entity<NotificationHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PerformedBy).IsRequired().HasMaxLength(200);
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.Timestamp).HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Notification)
                .WithMany(n => n.History)
                .HasForeignKey(e => e.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.NotificationId, e.Timestamp });
        });

        // Configure NotificationAcknowledgment entity
        modelBuilder.Entity<NotificationAcknowledgment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Device).HasMaxLength(50);
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.AcknowledgedAt).HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Notification)
                .WithMany(n => n.Acknowledgments)
                .HasForeignKey(e => e.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.NotificationId, e.UserId }).IsUnique();
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
