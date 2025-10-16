namespace NotificationService.Api.Models;

/// <summary>
/// Data Transfer Object for Tenant
/// </summary>
public class TenantDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for creating a new tenant
/// </summary>
public class CreateTenantDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
