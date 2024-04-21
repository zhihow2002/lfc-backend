using System.Runtime.Serialization;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Queries;

namespace ClaimsPlugin.Shared.Foundation.Features.Api.Grpc.Models;

public class SingleTenantPaginatedRequest : PaginationQuery
{
    [DataMember(Order = 1)]
    public new int PageNumber { get; set; } = 1;

    [DataMember(Order = 2)]
    public new int PageSize { get; set; } = int.MaxValue;

    [DataMember(Order = 3)]
    public new string[]? OrderBy { get; set; }
}
