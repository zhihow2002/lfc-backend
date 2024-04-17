using Ardalis.Specification;
using LinqKit;

namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.Specification
{
    public sealed class SingleQuerySpecification<T, TResult> : Specification<T, TResult>
    {
        public SingleQuerySpecification() { }

        public SingleQuerySpecification(Action<ISpecificationBuilder<T>> action)
        {
            action(Query);
        }

        public SingleQuerySpecification(
            Action<ISpecificationBuilder<T>, ExpressionStarter<T>> action
        )
        {
            action(Query, PredicateBuilder.New<T>());
        }
    }

    public sealed class SingleQuerySpecification<T> : Specification<T>
    {
        public SingleQuerySpecification() { }

        public SingleQuerySpecification(Action<ISpecificationBuilder<T>> action)
        {
            action(Query);
        }

        public SingleQuerySpecification(
            Action<ISpecificationBuilder<T>, ExpressionStarter<T>> action
        )
        {
            action(Query, PredicateBuilder.New<T>());
        }
    }
}
