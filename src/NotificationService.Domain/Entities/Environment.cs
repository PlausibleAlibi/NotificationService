namespace NotificationService.Domain.Entities;

/// <summary>
/// Represents a deployment environment for notification targeting.
/// Examples: Development, Staging, Production
/// </summary>
public class Environment
{
    /// <summary>
    /// Unique identifier for the environment
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The tenant this environment belongs to
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Navigation property to the tenant
    /// </summary>
    public Tenant Tenant { get; set; } = null!;

    /// <summary>
    /// Unique environment code (e.g., "dev", "staging", "prod")
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the environment
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Whether the environment is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the environment was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
