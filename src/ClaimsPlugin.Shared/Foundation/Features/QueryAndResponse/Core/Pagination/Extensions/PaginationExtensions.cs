using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Queries;

namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.Pagination.Extensions
{
    public static class PaginationExtensions
    {
        public static bool HasOrderBy(this SearchQuery query)
        {
            return query.OrderBy?.Any() is true;
        }

        public static bool HasOrderBy(this PaginationQuery query)
        {
            return query.OrderBy?.Any() is true;
        }
    }
}
