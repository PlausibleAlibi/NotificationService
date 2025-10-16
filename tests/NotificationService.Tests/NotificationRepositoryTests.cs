using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Data;
using NotificationService.Infrastructure.Repositories;

namespace NotificationService.Tests;

/// <summary>
/// Unit tests for NotificationRepository
/// </summary>
public class NotificationRepositoryTests
{
    private NotificationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<NotificationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new NotificationDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddNotification()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new NotificationRepository(context);
        var notification = new Notification
        {
            TenantId = 1,
            Title = "Test Notification",
            Message = "Test Message",
            Type = NotificationType.Info,
            CreatedBy = "TestUser"
        };

        // Act
        var result = await repository.CreateAsync(notification);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Test Notification", result.Title);
    }

    [Fact]
    public async Task GetByTenantAsync_ShouldReturnNotificationsForTenant()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new NotificationRepository(context);

        await repository.CreateAsync(new Notification
        {
            TenantId = 1,
            Title = "Notification 1",
            Message = "Message 1",
            Type = NotificationType.Info,
            CreatedBy = "TestUser"
        });

        await repository.CreateAsync(new Notification
        {
            TenantId = 1,
            Title = "Notification 2",
            Message = "Message 2",
            Type = NotificationType.Warning,
            CreatedBy = "TestUser"
        });

        await repository.CreateAsync(new Notification
        {
            TenantId = 2,
            Title = "Notification 3",
            Message = "Message 3",
            Type = NotificationType.Error,
            CreatedBy = "TestUser"
        });

        // Act
        var results = await repository.GetByTenantAsync(1);

        // Assert
        Assert.Equal(2, results.Count());
        Assert.All(results, n => Assert.Equal(1, n.TenantId));
    }

    [Fact]
    public async Task GetActiveByTenantAsync_ShouldReturnOnlyActiveNotifications()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new NotificationRepository(context);

        await repository.CreateAsync(new Notification
        {
            TenantId = 1,
            Title = "Active Notification",
            Message = "Active Message",
            Type = NotificationType.Info,
            IsActive = true,
            CreatedBy = "TestUser"
        });

        await repository.CreateAsync(new Notification
        {
            TenantId = 1,
            Title = "Inactive Notification",
            Message = "Inactive Message",
            Type = NotificationType.Warning,
            IsActive = false,
            CreatedBy = "TestUser"
        });

        // Act
        var results = await repository.GetActiveByTenantAsync(1);

        // Assert
        Assert.Single(results);
        Assert.True(results.First().IsActive);
        Assert.Equal("Active Notification", results.First().Title);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateNotification()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        
        // Add a tenant first (required for Include in GetByIdAsync)
        var tenant = new Tenant { Id = 1, Code = "test", Name = "Test Tenant" };
        context.Tenants.Add(tenant);
        await context.SaveChangesAsync();
        
        var repository = new NotificationRepository(context);

        var notification = await repository.CreateAsync(new Notification
        {
            TenantId = 1,
            Title = "Original Title",
            Message = "Original Message",
            Type = NotificationType.Info,
            CreatedBy = "TestUser"
        });

        // Detach the entity to simulate a fresh load
        context.Entry(notification).State = EntityState.Detached;

        // Load fresh from database
        var toUpdate = await repository.GetByIdAsync(notification.Id);
        Assert.NotNull(toUpdate);

        // Act
        toUpdate.Title = "Updated Title";
        toUpdate.IsActive = false;
        await repository.UpdateAsync(toUpdate);

        // Detach again and reload
        context.Entry(toUpdate).State = EntityState.Detached;
        var updated = await repository.GetByIdAsync(notification.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Updated Title", updated.Title);
        Assert.False(updated.IsActive);
        Assert.NotNull(updated.UpdatedAt);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveNotification()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new NotificationRepository(context);

        var notification = await repository.CreateAsync(new Notification
        {
            TenantId = 1,
            Title = "To Be Deleted",
            Message = "Delete Me",
            Type = NotificationType.Info,
            CreatedBy = "TestUser"
        });

        // Act
        await repository.DeleteAsync(notification.Id);
        var deleted = await repository.GetByIdAsync(notification.Id);

        // Assert
        Assert.Null(deleted);
    }
}
