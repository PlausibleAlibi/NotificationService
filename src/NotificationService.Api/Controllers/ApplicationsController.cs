using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Models;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;

namespace NotificationService.Api.Controllers;

/// <summary>
/// Controller for managing applications within tenants
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(
        IApplicationRepository applicationRepository,
        ILogger<ApplicationsController> logger)
    {
        _applicationRepository = applicationRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all applications for a tenant
    /// </summary>
    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetByTenant(int tenantId)
    {
        var applications = await _applicationRepository.GetByTenantAsync(tenantId);
        var dtos = applications.Select(MapToDto);
        return Ok(dtos);
    }

    /// <summary>
    /// Get application by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationDto>> GetById(int id)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        if (application == null)
        {
            return NotFound();
        }
        return Ok(MapToDto(application));
    }

    /// <summary>
    /// Get application by code within a tenant
    /// </summary>
    [HttpGet("tenant/{tenantId}/code/{code}")]
    public async Task<ActionResult<ApplicationDto>> GetByCode(int tenantId, string code)
    {
        var application = await _applicationRepository.GetByCodeAsync(tenantId, code);
        if (application == null)
        {
            return NotFound();
        }
        return Ok(MapToDto(application));
    }

    /// <summary>
    /// Create a new application
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApplicationDto>> Create(CreateApplicationDto dto)
    {
        var application = new Application
        {
            TenantId = dto.TenantId,
            Code = dto.Code,
            Name = dto.Name,
            Description = dto.Description,
            IsActive = true
        };

        var created = await _applicationRepository.CreateAsync(application);
        _logger.LogInformation("Created application {Id} for tenant {TenantId}", created.Id, created.TenantId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
    }

    /// <summary>
    /// Update an application
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdateApplicationDto dto)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        if (application == null)
        {
            return NotFound();
        }

        if (dto.Name != null) application.Name = dto.Name;
        if (dto.Description != null) application.Description = dto.Description;
        if (dto.IsActive.HasValue) application.IsActive = dto.IsActive.Value;

        await _applicationRepository.UpdateAsync(application);
        _logger.LogInformation("Updated application {Id}", id);

        return NoContent();
    }

    /// <summary>
    /// Delete an application
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        if (application == null)
        {
            return NotFound();
        }

        await _applicationRepository.DeleteAsync(id);
        _logger.LogInformation("Deleted application {Id}", id);

        return NoContent();
    }

    private static ApplicationDto MapToDto(Application application)
    {
        return new ApplicationDto
        {
            Id = application.Id,
            TenantId = application.TenantId,
            Code = application.Code,
            Name = application.Name,
            Description = application.Description,
            IsActive = application.IsActive,
            CreatedAt = application.CreatedAt
        };
    }
}
