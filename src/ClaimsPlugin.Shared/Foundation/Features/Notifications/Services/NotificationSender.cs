using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Notifications.Hubs;
using ClaimsPlugin.Shared.Foundation.Features.Notifications.Interfaces;
using Foundation.Features.Notifications.Interfaces;
using Microsoft.AspNetCore.SignalR;
namespace ClaimsPlugin.Shared.Foundation.Features.Notifications.Services;
public class NotificationSender : INotificationSender
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IHubContext<NotificationHub> _notificationHubContext;
    private readonly ILogger<NotificationSender> _logger;
    public NotificationSender(IHubContext<NotificationHub> notificationHubContext, ICurrentTenant currentTenant, ILogger<NotificationSender> logger)
    {
        _logger = logger;
        (_notificationHubContext, _currentTenant) = (notificationHubContext, currentTenant);
    }
    public async Task BroadcastAsync(INotificationMessage notification, CancellationToken cancellationToken = default)
    {
        try
        {
            await _notificationHubContext.Clients.All.SendAsync(notification.MethodName, notification.GetType().FullName, notification, cancellationToken);
            _logger.LogInformation("[NotificationSender] Message broadcast successfully to all clients: {Notification}", notification);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NotificationSender] Failed to broadcast message to all clients: {Message}", ex.Message);
            throw;
        }
    }
    public async Task BroadcastAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken = default)
    {
        try
        {
            await _notificationHubContext.Clients.AllExcept(excludedConnectionIds)
                .SendAsync(notification.MethodName, notification.GetType().FullName, notification, cancellationToken);
            _logger.LogInformation("[NotificationSender] Message broadcast successfully to all clients: {Notification}", notification);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NotificationSender] Failed to broadcast message to all clients: {Message}", ex.Message);
            throw;
        }
    }
    public async Task SendToAllAsync(INotificationMessage notification, CancellationToken cancellationToken = default)
    {
        try
        {
            await _notificationHubContext.Clients.Group($"Group{(_currentTenant.HasTenant() ? $"-{_currentTenant.GetTenantId()}" : "")}")
                .SendAsync(notification.MethodName, notification.GetType().FullName, notification, cancellationToken);
            _logger.LogInformation("[NotificationSender] Message sent successfully to all clients: {Notification}", notification);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NotificationSender] Failed to send message to all clients: {Message}", ex.Message);
            throw;
        }
    }
    public async Task SendToAllAsync(INotificationMessage notification, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<string> connectionIds = excludedConnectionIds.ToList();
            await _notificationHubContext.Clients.GroupExcept($"Group{(_currentTenant.HasTenant() ? $"-{_currentTenant.GetTenantId()}" : "")}", connectionIds)
                .SendAsync(notification.MethodName, notification.GetType().FullName, notification, cancellationToken);
            _logger.LogInformation("[NotificationSender] Message sent successfully to all clients: {Notification}", notification);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NotificationSender] Failed to send message to all clients: {Message}", ex.Message);
            throw;
        }
    }
    public async Task SendToGroupAsync(INotificationMessage notification, string group, CancellationToken cancellationToken = default)
    {
        try
        {
            await _notificationHubContext.Clients.Group(group)
                .SendAsync(notification.MethodName, notification.GetType().FullName, notification, cancellationToken);
            _logger.LogInformation("[NotificationSender] Message sent successfully to group: {Notification}", notification);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NotificationSender] Failed to send message to group: {Message}", ex.Message);
            throw;
        }
    }
    public async Task SendToGroupAsync(
        INotificationMessage notification,
        string group,
        IEnumerable<string> excludedConnectionIds,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await _notificationHubContext.Clients.GroupExcept(group, excludedConnectionIds)
                .SendAsync(notification.MethodName, notification.GetType().FullName, notification, cancellationToken);
            _logger.LogInformation("[NotificationSender] Message sent successfully to group: {Notification}", notification);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NotificationSender] Failed to send message to group: {Message}", ex.Message);
            throw;
        }
    }
    public async Task SendToGroupsAsync(INotificationMessage notification, IEnumerable<string> groupNames, CancellationToken cancellationToken = default)
    {
        try
        {
            await _notificationHubContext.Clients.Groups(groupNames)
                .SendAsync(notification.MethodName, notification.GetType().FullName, notification, cancellationToken);
            _logger.LogInformation("[NotificationSender] Message sent successfully to all groups: {Notification}", notification);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NotificationSender] Failed to send message to all groups: {Message}", ex.Message);
            throw;
        }
    }
    public async Task SendToUserAsync(INotificationMessage notification, string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            await _notificationHubContext.Clients.User(userId)
                .SendAsync(notification.MethodName, notification.GetType().FullName, notification, cancellationToken);
            _logger.LogInformation("[NotificationSender] Message sent successfully to user: {Notification}", notification);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NotificationSender] Failed to send message to user: {Message}", ex.Message);
            throw;
        }
    }
    public Task SendToUserAsync(INotificationMessage notification, Guid userId, CancellationToken cancellationToken = default)
    {
        return SendToUserAsync(notification, userId.ToString(), cancellationToken);
    }
    public async Task SendToUsersAsync(INotificationMessage notification, IEnumerable<string> userIds, CancellationToken cancellationToken = default)
    {
        try
        {
            await _notificationHubContext.Clients.Users(userIds)
                .SendAsync(notification.MethodName, notification.GetType().FullName, notification, cancellationToken);
            _logger.LogInformation("[NotificationSender] Message sent successfully to all users: {Notification}", notification);
        }
        catch (Exception ex)
        {
            _logger.LogError("[NotificationSender] Failed to send message to all users: {Message}", ex.Message);
            throw;
        }
    }
    public Task SendToUsersAsync(INotificationMessage notification, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        return SendToUsersAsync(notification, userIds.Select(x => x.ToString()), cancellationToken);
    }
}
