namespace NotificationService.Domain.Entities;

/// <summary>
/// Represents an audit trail entry for notification changes and delivery tracking.
/// </summary>
public class NotificationHistory
{
    /// <summary>
    /// Unique identifier for the history entry
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The notification this history entry relates to
    /// </summary>
    public int NotificationId { get; set; }

    /// <summary>
    /// Navigation property to the notification
    /// </summary>
    public Notification Notification { get; set; } = null!;

    /// <summary>
    /// Type of action that occurred
    /// </summary>
    public HistoryAction Action { get; set; }

    /// <summary>
    /// User who performed the action
    /// </summary>
    public string PerformedBy { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of the action
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Previous state of the notification (JSON serialized)
    /// </summary>
    public string? PreviousState { get; set; }

    /// <summary>
    /// New state of the notification (JSON serialized)
    /// </summary>
    public string? NewState { get; set; }

    /// <summary>
    /// Additional details about the action
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// IP address of the user who performed the action
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent of the client that performed the action
    /// </summary>
    public string? UserAgent { get; set; }
}

/// <summary>
/// Types of actions that can be logged in notification history
/// </summary>
public enum HistoryAction
{
    Created = 0,
    Updated = 1,
    Deleted = 2,
    Activated = 3,
    Deactivated = 4,
    Delivered = 5,
    Viewed = 6,
    Acknowledged = 7,
    Expired = 8
}
