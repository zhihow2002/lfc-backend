namespace ClaimsPlugin.Shared.Foundation.Features.Notifications.Interfaces;

// A marker interface to mark objects that can be sent to the client as notification messages.
public interface INotificationMessage
{
    public string MethodName { get; }
}
