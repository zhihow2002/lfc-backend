using ClaimsPlugin.Shared.Foundation.Features.Notifications.Interfaces;

namespace Foundation.Features.Notifications.Models;

public class StandardNotification : INotificationMessage
{
    public enum MessageType
    {
        Information,
        Success,
        Warning,
        Error
    }

    public string? Message { get; private set; }
    public MessageType Type { get; private set; }
    public string MethodName { get; private set; }

    public StandardNotification(string? message, MessageType type, string methodName = nameof(StandardNotification))
    {
        Message = message;
        Type = type;
        MethodName = methodName;
    }
}
