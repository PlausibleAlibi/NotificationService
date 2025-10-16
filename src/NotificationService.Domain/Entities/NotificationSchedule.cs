namespace NotificationService.Domain.Entities;

/// <summary>
/// Represents a schedule for a notification with recurrence and expiration support.
/// </summary>
public class NotificationSchedule
{
    /// <summary>
    /// Unique identifier for the schedule
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The notification this schedule belongs to
    /// </summary>
    public int NotificationId { get; set; }

    /// <summary>
    /// Navigation property to the notification
    /// </summary>
    public Notification Notification { get; set; } = null!;

    /// <summary>
    /// When the notification should start showing
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// When the notification should stop showing
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Recurrence pattern (e.g., "daily", "weekly", "monthly")
    /// </summary>
    public RecurrencePattern Recurrence { get; set; } = RecurrencePattern.None;

    /// <summary>
    /// Interval for recurrence (e.g., every 2 weeks)
    /// </summary>
    public int RecurrenceInterval { get; set; } = 1;

    /// <summary>
    /// Days of week for weekly recurrence (comma-separated: 0=Sunday, 6=Saturday)
    /// </summary>
    public string? RecurrenceDaysOfWeek { get; set; }

    /// <summary>
    /// Day of month for monthly recurrence (1-31)
    /// </summary>
    public int? RecurrenceDayOfMonth { get; set; }

    /// <summary>
    /// Time zone for the schedule (IANA time zone identifier)
    /// </summary>
    public string TimeZone { get; set; } = "UTC";

    /// <summary>
    /// When the schedule expires (no longer applies)
    /// </summary>
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Whether the schedule is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the schedule was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the schedule was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Recurrence patterns for notification schedules
/// </summary>
public enum RecurrencePattern
{
    None = 0,
    Daily = 1,
    Weekly = 2,
    Monthly = 3,
    Yearly = 4
}
