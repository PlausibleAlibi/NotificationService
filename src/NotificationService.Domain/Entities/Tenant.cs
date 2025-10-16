namespace NotificationService.Domain.Entities;

/// <summary>
/// Represents a tenant in the multi-tenant notification system.
/// Each tenant can have their own set of notifications.
/// </summary>
public class Tenant
{
    /// <summary>
    /// Unique identifier for the tenant
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique tenant code/slug (e.g., "acme-corp")
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the tenant
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Whether the tenant is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the tenant was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Collection of notifications belonging to this tenant
    /// </summary>
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    /// <summary>
    /// Collection of applications belonging to this tenant
    /// </summary>
    public ICollection<Application> Applications { get; set; } = new List<Application>();

    /// <summary>
    /// Collection of environments belonging to this tenant
    /// </summary>
    public ICollection<Environment> Environments { get; set; } = new List<Environment>();

    /// <summary>
    /// Collection of templates belonging to this tenant
    /// </summary>
    public ICollection<NotificationTemplate> Templates { get; set; } = new List<NotificationTemplate>();
}
