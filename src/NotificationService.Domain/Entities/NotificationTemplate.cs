namespace NotificationService.Domain.Entities;

/// <summary>
/// Represents a reusable notification template with HTML or Markdown content.
/// Templates can be used to standardize notification formatting.
/// </summary>
public class NotificationTemplate
{
    /// <summary>
    /// Unique identifier for the template
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The tenant this template belongs to
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Navigation property to the tenant
    /// </summary>
    public Tenant Tenant { get; set; } = null!;

    /// <summary>
    /// Unique template code for referencing
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the template
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the template's purpose
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Template content (HTML or Markdown)
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Format type of the template content
    /// </summary>
    public TemplateFormat Format { get; set; } = TemplateFormat.Html;

    /// <summary>
    /// Whether the template is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the template was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the template was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// User who created the template
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
}

/// <summary>
/// Supported template content formats
/// </summary>
public enum TemplateFormat
{
    Html = 0,
    Markdown = 1
}
