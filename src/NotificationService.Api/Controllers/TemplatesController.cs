using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Models;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;

namespace NotificationService.Api.Controllers;

/// <summary>
/// Controller for managing notification templates
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly INotificationTemplateRepository _templateRepository;
    private readonly ILogger<TemplatesController> _logger;

    public TemplatesController(
        INotificationTemplateRepository templateRepository,
        ILogger<TemplatesController> logger)
    {
        _templateRepository = templateRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all templates for a tenant
    /// </summary>
    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<IEnumerable<TemplateDto>>> GetByTenant(int tenantId)
    {
        var templates = await _templateRepository.GetByTenantAsync(tenantId);
        var dtos = templates.Select(MapToDto);
        return Ok(dtos);
    }

    /// <summary>
    /// Get template by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TemplateDto>> GetById(int id)
    {
        var template = await _templateRepository.GetByIdAsync(id);
        if (template == null)
        {
            return NotFound();
        }
        return Ok(MapToDto(template));
    }

    /// <summary>
    /// Get template by code within a tenant
    /// </summary>
    [HttpGet("tenant/{tenantId}/code/{code}")]
    public async Task<ActionResult<TemplateDto>> GetByCode(int tenantId, string code)
    {
        var template = await _templateRepository.GetByCodeAsync(tenantId, code);
        if (template == null)
        {
            return NotFound();
        }
        return Ok(MapToDto(template));
    }

    /// <summary>
    /// Create a new template
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TemplateDto>> Create(CreateTemplateDto dto)
    {
        var template = new NotificationTemplate
        {
            TenantId = dto.TenantId,
            Code = dto.Code,
            Name = dto.Name,
            Description = dto.Description,
            Content = dto.Content,
            Format = Enum.Parse<TemplateFormat>(dto.Format, true),
            IsActive = true,
            CreatedBy = User.Identity?.Name ?? "System"
        };

        var created = await _templateRepository.CreateAsync(template);
        _logger.LogInformation("Created template {Id} for tenant {TenantId}", created.Id, created.TenantId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
    }

    /// <summary>
    /// Update a template
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdateTemplateDto dto)
    {
        var template = await _templateRepository.GetByIdAsync(id);
        if (template == null)
        {
            return NotFound();
        }

        if (dto.Name != null) template.Name = dto.Name;
        if (dto.Description != null) template.Description = dto.Description;
        if (dto.Content != null) template.Content = dto.Content;
        if (dto.IsActive.HasValue) template.IsActive = dto.IsActive.Value;

        await _templateRepository.UpdateAsync(template);
        _logger.LogInformation("Updated template {Id}", id);

        return NoContent();
    }

    /// <summary>
    /// Delete a template
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var template = await _templateRepository.GetByIdAsync(id);
        if (template == null)
        {
            return NotFound();
        }

        await _templateRepository.DeleteAsync(id);
        _logger.LogInformation("Deleted template {Id}", id);

        return NoContent();
    }

    private static TemplateDto MapToDto(NotificationTemplate template)
    {
        return new TemplateDto
        {
            Id = template.Id,
            TenantId = template.TenantId,
            Code = template.Code,
            Name = template.Name,
            Description = template.Description,
            Content = template.Content,
            Format = template.Format.ToString(),
            IsActive = template.IsActive,
            CreatedAt = template.CreatedAt,
            UpdatedAt = template.UpdatedAt,
            CreatedBy = template.CreatedBy
        };
    }
}
