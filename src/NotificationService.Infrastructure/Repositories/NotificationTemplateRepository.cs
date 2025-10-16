using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Data;

namespace NotificationService.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for NotificationTemplate entities
/// </summary>
public class NotificationTemplateRepository : INotificationTemplateRepository
{
    private readonly NotificationDbContext _context;

    public NotificationTemplateRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NotificationTemplate>> GetByTenantAsync(int tenantId)
    {
        return await _context.NotificationTemplates
            .Where(t => t.TenantId == tenantId)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<NotificationTemplate?> GetByIdAsync(int id)
    {
        return await _context.NotificationTemplates
            .Include(t => t.Tenant)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<NotificationTemplate?> GetByCodeAsync(int tenantId, string code)
    {
        return await _context.NotificationTemplates
            .FirstOrDefaultAsync(t => t.TenantId == tenantId && t.Code == code);
    }

    public async Task<NotificationTemplate> CreateAsync(NotificationTemplate template)
    {
        template.CreatedAt = DateTime.UtcNow;
        _context.NotificationTemplates.Add(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task UpdateAsync(NotificationTemplate template)
    {
        template.UpdatedAt = DateTime.UtcNow;
        _context.NotificationTemplates.Update(template);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var template = await _context.NotificationTemplates.FindAsync(id);
        if (template != null)
        {
            _context.NotificationTemplates.Remove(template);
            await _context.SaveChangesAsync();
        }
    }
}
