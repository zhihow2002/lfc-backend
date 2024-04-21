using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Queries;

namespace ClaimsPlugin.Shared.Foundation.Common.Utilities;
public static class ListUtility
{
    public static List<T> InMemoryPaginateBy<T>(this List<T> list, PaginationQuery query) where T : class
    {
        if (query.PageNumber <= 0)
        {
            query.PageNumber = 1;
        }
        if (query.PageSize <= 0)
        {
            query.PageSize = 10;
        }
        if (query.PageNumber > 1)
        {
            list = list.Skip((query.PageNumber - 1) * query.PageSize).ToList();
        }
        return list.Take(query.PageSize).ToList();
    }
    public static List<T> InMemoryPaginateBy<T>(this List<T> list, SearchQuery query) where T : class
    {
        if (query.PageNumber <= 0)
        {
            query.PageNumber = 1;
        }
        if (query.PageSize <= 0)
        {
            query.PageSize = 10;
        }
        if (query.PageNumber > 1)
        {
            list = list.Skip((query.PageNumber - 1) * query.PageSize).ToList();
        }
        return list.Take(query.PageSize).ToList();
    }
}
