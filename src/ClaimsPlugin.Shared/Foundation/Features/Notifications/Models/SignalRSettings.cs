namespace ClaimsPlugin.Shared.Foundation.Features.Notifications.Models;

public class SignalRSettings
{
    public string? Channel { get; set; }
    public bool UseBackplane { get; set; }
    
    public BackplaneObject? Backplane { get; set; }

    public class BackplaneObject
    {
        public string? Provider { get; set; }
        public string? ConnectionString { get; set; }
    }
}
