namespace NotificationService.Domain.Entities;

/// <summary>
/// Represents an application within a tenant's ecosystem.
/// Each tenant can have multiple applications that need notifications.
/// </summary>
public class Application
{
    /// <summary>
    /// Unique identifier for the application
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The tenant this application belongs to
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Navigation property to the tenant
    /// </summary>
    public Tenant Tenant { get; set; } = null!;

    /// <summary>
    /// Unique application code (e.g., "web-portal", "mobile-app")
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the application
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the application
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Whether the application is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the application was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Collection of notifications targeting this application
    /// </summary>
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
