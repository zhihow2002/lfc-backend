using System.Linq.Expressions;
using Ardalis.Specification;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.AdvancedSearch.Extensions;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.Pagination.Extensions;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Queries;
using LinqKit;

namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.Specification
{
    public sealed class PaginationQuerySpecification<T, TResult> : Specification<T, TResult>
    {
        public PaginationQuerySpecification() { }

        public PaginationQuerySpecification(Action<ISpecificationBuilder<T>> action)
        {
            action(Query);
        }

        public PaginationQuerySpecification(
            Action<ISpecificationBuilder<T>> action,
            Expression<Func<T, TResult>> mappingExpressions
        )
        {
            action(Query.Select(mappingExpressions));
        }

        public PaginationQuerySpecification(
            Action<ISpecificationBuilder<T>, ExpressionStarter<T>> action
        )
        {
            action(Query, PredicateBuilder.New<T>());
        }

        public PaginationQuerySpecification(PaginationQuery paginationQuery)
        {
            Query.PaginateBy(paginationQuery);
        }

        public PaginationQuerySpecification(
            PaginationQuery paginationQuery,
            Action<ISpecificationBuilder<T>> action
        )
        {
            action(Query.PaginateBy(paginationQuery));
        }

        public PaginationQuerySpecification(
            PaginationQuery paginationQuery,
            Action<ISpecificationBuilder<T>, ExpressionStarter<T>> action
        )
        {
            action(Query.PaginateBy(paginationQuery), PredicateBuilder.New<T>());
        }

        public PaginationQuerySpecification(SearchQuery paginationSearchQuery)
        {
            Query.PaginateBy(paginationSearchQuery).SearchBy(paginationSearchQuery);
        }

        public PaginationQuerySpecification(
            SearchQuery query,
            Action<ISpecificationBuilder<T>> action
        )
        {
            action(Query.PaginateBy(query).SearchBy(query));
        }

        public PaginationQuerySpecification(
            SearchQuery query,
            Action<ISpecificationBuilder<T>> action,
            Expression<Func<T, TResult>> mappingExpressions
        )
        {
            action(Query.Select(mappingExpressions).PaginateBy(query).SearchBy(query));
        }

        public PaginationQuerySpecification(
            SearchQuery query,
            Action<ISpecificationBuilder<T>, ExpressionStarter<T>> action
        )
        {
            action(Query.PaginateBy(query).SearchBy(query), PredicateBuilder.New<T>());
        }
    }

    // TODO: To rename it to something more generic to list
    public sealed class PaginationQuerySpecification<T> : Specification<T>
    {
        public PaginationQuerySpecification() { }

        public PaginationQuerySpecification(Action<ISpecificationBuilder<T>> action)
        {
            action(Query);
        }

        public PaginationQuerySpecification(
            Action<ISpecificationBuilder<T>, ExpressionStarter<T>> action
        )
        {
            action(Query, PredicateBuilder.New<T>());
        }

        public PaginationQuerySpecification(PaginationQuery paginationQuery)
        {
            Query.PaginateBy(paginationQuery);
        }

        public PaginationQuerySpecification(
            PaginationQuery paginationQuery,
            Action<ISpecificationBuilder<T>> action
        )
        {
            action(Query.PaginateBy(paginationQuery));
        }

        public PaginationQuerySpecification(
            PaginationQuery paginationQuery,
            Action<ISpecificationBuilder<T>, ExpressionStarter<T>> action
        )
        {
            action(Query.PaginateBy(paginationQuery), PredicateBuilder.New<T>());
        }

        public PaginationQuerySpecification(SearchQuery paginationSearchQuery)
        {
            Query.PaginateBy(paginationSearchQuery).SearchBy(paginationSearchQuery);
        }

        public PaginationQuerySpecification(
            SearchQuery paginationSearchQuery,
            Action<ISpecificationBuilder<T>> action
        )
        {
            action(Query.PaginateBy(paginationSearchQuery).SearchBy(paginationSearchQuery));
        }

        public PaginationQuerySpecification(
            SearchQuery paginationSearchQuery,
            Action<ISpecificationBuilder<T>, ExpressionStarter<T>> action
        )
        {
            action(
                Query.PaginateBy(paginationSearchQuery).SearchBy(paginationSearchQuery),
                PredicateBuilder.New<T>()
            );
        }
    }
}
