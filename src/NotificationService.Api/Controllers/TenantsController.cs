using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Models;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;

namespace NotificationService.Api.Controllers;

/// <summary>
/// Controller for managing tenants
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ILogger<TenantsController> _logger;

    public TenantsController(
        ITenantRepository tenantRepository,
        ILogger<TenantsController> logger)
    {
        _tenantRepository = tenantRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all tenants
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TenantDto>>> GetAll()
    {
        var tenants = await _tenantRepository.GetAllAsync();
        var dtos = tenants.Select(MapToDto);
        return Ok(dtos);
    }

    /// <summary>
    /// Get tenant by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TenantDto>> GetById(int id)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id);
        if (tenant == null)
        {
            return NotFound();
        }
        return Ok(MapToDto(tenant));
    }

    /// <summary>
    /// Get tenant by code
    /// </summary>
    [HttpGet("code/{code}")]
    public async Task<ActionResult<TenantDto>> GetByCode(string code)
    {
        var tenant = await _tenantRepository.GetByCodeAsync(code);
        if (tenant == null)
        {
            return NotFound();
        }
        return Ok(MapToDto(tenant));
    }

    /// <summary>
    /// Create a new tenant
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TenantDto>> Create(CreateTenantDto dto)
    {
        var tenant = new Tenant
        {
            Code = dto.Code,
            Name = dto.Name
        };

        var created = await _tenantRepository.CreateAsync(tenant);
        _logger.LogInformation("Created tenant {Id} with code {Code}", created.Id, created.Code);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
    }

    /// <summary>
    /// Delete a tenant
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id);
        if (tenant == null)
        {
            return NotFound();
        }

        await _tenantRepository.DeleteAsync(id);
        _logger.LogInformation("Deleted tenant {Id}", id);

        return NoContent();
    }

    private static TenantDto MapToDto(Tenant tenant)
    {
        return new TenantDto
        {
            Id = tenant.Id,
            Code = tenant.Code,
            Name = tenant.Name,
            IsActive = tenant.IsActive,
            CreatedAt = tenant.CreatedAt
        };
    }
}
