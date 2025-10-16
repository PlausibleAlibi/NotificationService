namespace NotificationService.Domain.Entities;

/// <summary>
/// Represents a user acknowledgment of a notification.
/// Tracks which users have seen and acknowledged notifications.
/// </summary>
public class NotificationAcknowledgment
{
    /// <summary>
    /// Unique identifier for the acknowledgment
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The notification that was acknowledged
    /// </summary>
    public int NotificationId { get; set; }

    /// <summary>
    /// Navigation property to the notification
    /// </summary>
    public Notification Notification { get; set; } = null!;

    /// <summary>
    /// User identifier who acknowledged the notification
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Username of the user who acknowledged
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// When the user first viewed the notification
    /// </summary>
    public DateTime? ViewedAt { get; set; }

    /// <summary>
    /// When the user acknowledged the notification
    /// </summary>
    public DateTime AcknowledgedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Optional feedback or comments from the user
    /// </summary>
    public string? Feedback { get; set; }

    /// <summary>
    /// Device or platform used to acknowledge (e.g., "web", "mobile")
    /// </summary>
    public string? Device { get; set; }

    /// <summary>
    /// IP address of the user when acknowledging
    /// </summary>
    public string? IpAddress { get; set; }
}
