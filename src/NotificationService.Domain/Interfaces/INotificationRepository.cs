using NotificationService.Domain.Entities;

namespace NotificationService.Domain.Interfaces;

/// <summary>
/// Repository interface for Notification entities
/// </summary>
public interface INotificationRepository
{
    /// <summary>
    /// Get all notifications for a specific tenant
    /// </summary>
    Task<IEnumerable<Notification>> GetByTenantAsync(int tenantId);

    /// <summary>
    /// Get active notifications for a specific tenant
    /// </summary>
    Task<IEnumerable<Notification>> GetActiveByTenantAsync(int tenantId);

    /// <summary>
    /// Get notification by ID
    /// </summary>
    Task<Notification?> GetByIdAsync(int id);

    /// <summary>
    /// Create a new notification
    /// </summary>
    Task<Notification> CreateAsync(Notification notification);

    /// <summary>
    /// Update an existing notification
    /// </summary>
    Task UpdateAsync(Notification notification);

    /// <summary>
    /// Delete a notification
    /// </summary>
    Task DeleteAsync(int id);
}
