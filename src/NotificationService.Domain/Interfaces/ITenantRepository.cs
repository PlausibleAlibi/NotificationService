using NotificationService.Domain.Entities;

namespace NotificationService.Domain.Interfaces;

/// <summary>
/// Repository interface for Tenant entities
/// </summary>
public interface ITenantRepository
{
    /// <summary>
    /// Get all tenants
    /// </summary>
    Task<IEnumerable<Tenant>> GetAllAsync();

    /// <summary>
    /// Get tenant by ID
    /// </summary>
    Task<Tenant?> GetByIdAsync(int id);

    /// <summary>
    /// Get tenant by code
    /// </summary>
    Task<Tenant?> GetByCodeAsync(string code);

    /// <summary>
    /// Create a new tenant
    /// </summary>
    Task<Tenant> CreateAsync(Tenant tenant);

    /// <summary>
    /// Update an existing tenant
    /// </summary>
    Task UpdateAsync(Tenant tenant);

    /// <summary>
    /// Delete a tenant
    /// </summary>
    Task DeleteAsync(int id);
}
