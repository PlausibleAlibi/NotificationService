using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Models;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Interfaces;

namespace NotificationService.Api.Controllers;

/// <summary>
/// Controller for managing notifications/alerts
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(
        INotificationRepository notificationRepository,
        ILogger<NotificationsController> logger)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all notifications for a tenant
    /// </summary>
    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetByTenant(int tenantId)
    {
        var notifications = await _notificationRepository.GetByTenantAsync(tenantId);
        var dtos = notifications.Select(MapToDto);
        return Ok(dtos);
    }

    /// <summary>
    /// Get active notifications for a tenant (for banner display)
    /// </summary>
    [HttpGet("tenant/{tenantId}/active")]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetActiveByTenant(int tenantId)
    {
        var notifications = await _notificationRepository.GetActiveByTenantAsync(tenantId);
        var dtos = notifications.Select(MapToDto);
        return Ok(dtos);
    }

    /// <summary>
    /// Get notification by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<NotificationDto>> GetById(int id)
    {
        var notification = await _notificationRepository.GetByIdAsync(id);
        if (notification == null)
        {
            return NotFound();
        }
        return Ok(MapToDto(notification));
    }

    /// <summary>
    /// Create a new notification
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<NotificationDto>> Create(CreateNotificationDto dto)
    {
        var notification = new Notification
        {
            TenantId = dto.TenantId,
            Title = dto.Title,
            Message = dto.Message,
            Type = Enum.Parse<NotificationType>(dto.Type, true),
            IsActive = dto.IsActive,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CreatedBy = User.Identity?.Name ?? "System"
        };

        var created = await _notificationRepository.CreateAsync(notification);
        _logger.LogInformation("Created notification {Id} for tenant {TenantId}", created.Id, created.TenantId);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
    }

    /// <summary>
    /// Update a notification
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdateNotificationDto dto)
    {
        var notification = await _notificationRepository.GetByIdAsync(id);
        if (notification == null)
        {
            return NotFound();
        }

        if (dto.Title != null) notification.Title = dto.Title;
        if (dto.Message != null) notification.Message = dto.Message;
        if (dto.Type != null) notification.Type = Enum.Parse<NotificationType>(dto.Type, true);
        if (dto.IsActive.HasValue) notification.IsActive = dto.IsActive.Value;
        if (dto.StartDate.HasValue) notification.StartDate = dto.StartDate;
        if (dto.EndDate.HasValue) notification.EndDate = dto.EndDate;

        await _notificationRepository.UpdateAsync(notification);
        _logger.LogInformation("Updated notification {Id}", id);

        return NoContent();
    }

    /// <summary>
    /// Delete a notification
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var notification = await _notificationRepository.GetByIdAsync(id);
        if (notification == null)
        {
            return NotFound();
        }

        await _notificationRepository.DeleteAsync(id);
        _logger.LogInformation("Deleted notification {Id}", id);

        return NoContent();
    }

    private static NotificationDto MapToDto(Notification notification)
    {
        return new NotificationDto
        {
            Id = notification.Id,
            TenantId = notification.TenantId,
            Title = notification.Title,
            Message = notification.Message,
            Type = notification.Type.ToString(),
            IsActive = notification.IsActive,
            StartDate = notification.StartDate,
            EndDate = notification.EndDate,
            CreatedAt = notification.CreatedAt,
            UpdatedAt = notification.UpdatedAt,
            CreatedBy = notification.CreatedBy
        };
    }
}
