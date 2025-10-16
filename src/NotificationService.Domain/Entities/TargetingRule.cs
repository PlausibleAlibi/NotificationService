namespace NotificationService.Domain.Entities;

/// <summary>
/// Represents a targeting rule for a notification to control which users/groups see it.
/// </summary>
public class TargetingRule
{
    /// <summary>
    /// Unique identifier for the targeting rule
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The notification this rule applies to
    /// </summary>
    public int NotificationId { get; set; }

    /// <summary>
    /// Navigation property to the notification
    /// </summary>
    public Notification Notification { get; set; } = null!;

    /// <summary>
    /// Type of targeting rule
    /// </summary>
    public TargetingType TargetType { get; set; }

    /// <summary>
    /// Target application ID (if targeting by application)
    /// </summary>
    public int? TargetApplicationId { get; set; }

    /// <summary>
    /// Navigation property to the target application
    /// </summary>
    public Application? TargetApplication { get; set; }

    /// <summary>
    /// Target environment code (if targeting by environment)
    /// </summary>
    public string? TargetEnvironment { get; set; }

    /// <summary>
    /// User group identifier (if targeting by user group)
    /// </summary>
    public string? TargetUserGroup { get; set; }

    /// <summary>
    /// Custom filter expression (JSON or SQL-like syntax for advanced targeting)
    /// </summary>
    public string? CustomFilter { get; set; }

    /// <summary>
    /// Priority of this rule (higher number = higher priority)
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// Whether this rule is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the rule was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Types of targeting for notifications
/// </summary>
public enum TargetingType
{
    All = 0,
    Application = 1,
    Environment = 2,
    UserGroup = 3,
    Custom = 4
}
