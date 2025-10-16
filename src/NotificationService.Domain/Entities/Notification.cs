namespace NotificationService.Domain.Entities;

/// <summary>
/// Represents a notification/alert to be displayed as a banner in the UI.
/// Supports multi-tenant scenarios for system downtime or issues.
/// </summary>
public class Notification
{
    /// <summary>
    /// Unique identifier for the notification
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The tenant this notification belongs to
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Navigation property to the tenant
    /// </summary>
    public Tenant Tenant { get; set; } = null!;

    /// <summary>
    /// Title of the notification
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed message content
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Notification type/severity (e.g., Info, Warning, Error)
    /// </summary>
    public NotificationType Type { get; set; } = NotificationType.Info;

    /// <summary>
    /// Whether the notification is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the notification should start showing
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// When the notification should stop showing (null = no end date)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// When the notification was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the notification was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// User who created the notification
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Optional template ID if using a template
    /// </summary>
    public int? TemplateId { get; set; }

    /// <summary>
    /// Navigation property to the template
    /// </summary>
    public NotificationTemplate? Template { get; set; }

    /// <summary>
    /// Optional application ID for app-specific notifications
    /// </summary>
    public int? ApplicationId { get; set; }

    /// <summary>
    /// Navigation property to the application
    /// </summary>
    public Application? Application { get; set; }

    /// <summary>
    /// Collection of schedules for this notification
    /// </summary>
    public ICollection<NotificationSchedule> Schedules { get; set; } = new List<NotificationSchedule>();

    /// <summary>
    /// Collection of targeting rules for this notification
    /// </summary>
    public ICollection<TargetingRule> TargetingRules { get; set; } = new List<TargetingRule>();

    /// <summary>
    /// Collection of history entries for this notification
    /// </summary>
    public ICollection<NotificationHistory> History { get; set; } = new List<NotificationHistory>();

    /// <summary>
    /// Collection of acknowledgments for this notification
    /// </summary>
    public ICollection<NotificationAcknowledgment> Acknowledgments { get; set; } = new List<NotificationAcknowledgment>();
}

/// <summary>
/// Types/severity levels for notifications
/// </summary>
public enum NotificationType
{
    Info = 0,
    Warning = 1,
    Error = 2,
    Success = 3
}
