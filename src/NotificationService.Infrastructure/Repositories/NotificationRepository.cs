using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Data;

namespace NotificationService.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Notification entities
/// </summary>
public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _context;

    public NotificationRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Notification>> GetByTenantAsync(int tenantId)
    {
        return await _context.Notifications
            .Where(n => n.TenantId == tenantId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetActiveByTenantAsync(int tenantId)
    {
        var now = DateTime.UtcNow;
        return await _context.Notifications
            .Where(n => n.TenantId == tenantId
                && n.IsActive
                && (n.StartDate == null || n.StartDate <= now)
                && (n.EndDate == null || n.EndDate >= now))
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await _context.Notifications
            .Include(n => n.Tenant)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<Notification> CreateAsync(Notification notification)
    {
        notification.CreatedAt = DateTime.UtcNow;
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task UpdateAsync(Notification notification)
    {
        notification.UpdatedAt = DateTime.UtcNow;
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}
