using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Extensions;

public static class DatabaseSetExtensions
{
    public static IQueryable<TSource> WithSpecification<TSource>(
        this IQueryable<TSource> source,
        ISpecification<TSource> specification,
        bool evaluateCriteriaOnly,
        ISpecificationEvaluator? evaluator = null
    ) where TSource : class
    {
        evaluator = evaluator ?? SpecificationEvaluator.Default;
        return evaluator.GetQuery(source, specification, evaluateCriteriaOnly);
    }
}
