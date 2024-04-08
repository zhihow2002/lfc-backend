namespace ClaimsPlugin.Shared.Foundation.Features.Hosting.Models;

public class HostingSettings
{
    public string Name { get; set; } = default!;
    public HostObject RestHttps { get; set; } = default!;
    public HostObject RestHttp { get; set; } = default!;
    public HostObject Grpc { get; set; } = default!;

    public class HostObject
    {
        public string Url { get; set; } = default!;
        public int Port { get; set; } = default!;
        public bool Enabled { get; set; } = default!;
    }
}
