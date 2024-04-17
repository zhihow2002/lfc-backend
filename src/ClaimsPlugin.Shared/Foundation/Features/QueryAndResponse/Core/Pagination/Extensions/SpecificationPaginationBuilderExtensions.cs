using Ardalis.Specification;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.Sorting.Extensions;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.Pagination.Extensions
{
    public static class SpecificationPaginationBuilderExtensions
    {
        public static ISpecificationBuilder<T> PaginateBy<T>(
            this ISpecificationBuilder<T> query,
            SearchQuery paginationSearchQuery
        )
        {
            if (paginationSearchQuery.PageNumber <= 0)
            {
                paginationSearchQuery.PageNumber = 1;
            }

            if (paginationSearchQuery.PageSize <= 0)
            {
                paginationSearchQuery.PageSize = 10;
            }

            if (paginationSearchQuery.PageNumber > 1)
            {
                query = query.Skip(
                    (paginationSearchQuery.PageNumber - 1) * paginationSearchQuery.PageSize
                );
            }

            return query
                .Take(paginationSearchQuery.PageSize)
                .OrderBy(paginationSearchQuery.OrderBy);
        }

        public static ISpecificationBuilder<T> PaginateBy<T>(
            this ISpecificationBuilder<T> query,
            PaginationQuery paginationQuery
        )
        {
            if (paginationQuery.PageNumber <= 0)
            {
                paginationQuery.PageNumber = 1;
            }

            if (paginationQuery.PageSize <= 0)
            {
                paginationQuery.PageSize = 10;
            }

            if (paginationQuery.PageNumber > 1)
            {
                query = query.Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize);
            }

            return query.Take(paginationQuery.PageSize).OrderBy(paginationQuery.OrderBy);
        }
    }
}
