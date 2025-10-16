using NotificationService.Domain.Entities;

namespace NotificationService.Domain.Interfaces;

/// <summary>
/// Repository interface for Application entities
/// </summary>
public interface IApplicationRepository
{
    /// <summary>
    /// Get all applications for a specific tenant
    /// </summary>
    Task<IEnumerable<Application>> GetByTenantAsync(int tenantId);

    /// <summary>
    /// Get application by ID
    /// </summary>
    Task<Application?> GetByIdAsync(int id);

    /// <summary>
    /// Get application by code within a tenant
    /// </summary>
    Task<Application?> GetByCodeAsync(int tenantId, string code);

    /// <summary>
    /// Create a new application
    /// </summary>
    Task<Application> CreateAsync(Application application);

    /// <summary>
    /// Update an existing application
    /// </summary>
    Task UpdateAsync(Application application);

    /// <summary>
    /// Delete an application
    /// </summary>
    Task DeleteAsync(int id);
}
