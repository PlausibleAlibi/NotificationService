namespace NotificationService.Api.Models;

/// <summary>
/// Data transfer object for Application entity
/// </summary>
public class ApplicationDto
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for creating a new application
/// </summary>
public class CreateApplicationDto
{
    public int TenantId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// DTO for updating an application
/// </summary>
public class UpdateApplicationDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}
