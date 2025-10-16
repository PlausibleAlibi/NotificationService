using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Data;

namespace NotificationService.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Tenant entities
/// </summary>
public class TenantRepository : ITenantRepository
{
    private readonly NotificationDbContext _context;

    public TenantRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tenant>> GetAllAsync()
    {
        return await _context.Tenants
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<Tenant?> GetByIdAsync(int id)
    {
        return await _context.Tenants.FindAsync(id);
    }

    public async Task<Tenant?> GetByCodeAsync(string code)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Code == code);
    }

    public async Task<Tenant> CreateAsync(Tenant tenant)
    {
        tenant.CreatedAt = DateTime.UtcNow;
        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();
        return tenant;
    }

    public async Task UpdateAsync(Tenant tenant)
    {
        _context.Tenants.Update(tenant);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var tenant = await _context.Tenants.FindAsync(id);
        if (tenant != null)
        {
            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();
        }
    }
}
