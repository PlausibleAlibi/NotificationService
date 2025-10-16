using NotificationService.Domain.Entities;

namespace NotificationService.Domain.Interfaces;

/// <summary>
/// Repository interface for NotificationTemplate entities
/// </summary>
public interface INotificationTemplateRepository
{
    /// <summary>
    /// Get all templates for a specific tenant
    /// </summary>
    Task<IEnumerable<NotificationTemplate>> GetByTenantAsync(int tenantId);

    /// <summary>
    /// Get template by ID
    /// </summary>
    Task<NotificationTemplate?> GetByIdAsync(int id);

    /// <summary>
    /// Get template by code within a tenant
    /// </summary>
    Task<NotificationTemplate?> GetByCodeAsync(int tenantId, string code);

    /// <summary>
    /// Create a new template
    /// </summary>
    Task<NotificationTemplate> CreateAsync(NotificationTemplate template);

    /// <summary>
    /// Update an existing template
    /// </summary>
    Task UpdateAsync(NotificationTemplate template);

    /// <summary>
    /// Delete a template
    /// </summary>
    Task DeleteAsync(int id);
}
