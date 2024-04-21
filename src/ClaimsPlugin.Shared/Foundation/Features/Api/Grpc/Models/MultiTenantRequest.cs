using System.Runtime.Serialization;

namespace ClaimsPlugin.Shared.Foundation.Features.Api.Grpc.Models;

[DataContract]
public class MultiTenantRequest
{
    [DataMember(Order = 999)]
    public required string TenantId { get; set; }
}