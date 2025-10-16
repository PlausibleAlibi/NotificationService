using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Data;

namespace NotificationService.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Application entities
/// </summary>
public class ApplicationRepository : IApplicationRepository
{
    private readonly NotificationDbContext _context;

    public ApplicationRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Application>> GetByTenantAsync(int tenantId)
    {
        return await _context.Applications
            .Where(a => a.TenantId == tenantId)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<Application?> GetByIdAsync(int id)
    {
        return await _context.Applications
            .Include(a => a.Tenant)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Application?> GetByCodeAsync(int tenantId, string code)
    {
        return await _context.Applications
            .FirstOrDefaultAsync(a => a.TenantId == tenantId && a.Code == code);
    }

    public async Task<Application> CreateAsync(Application application)
    {
        application.CreatedAt = DateTime.UtcNow;
        _context.Applications.Add(application);
        await _context.SaveChangesAsync();
        return application;
    }

    public async Task UpdateAsync(Application application)
    {
        _context.Applications.Update(application);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var application = await _context.Applications.FindAsync(id);
        if (application != null)
        {
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
        }
    }
}
