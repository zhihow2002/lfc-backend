using System.Linq.Expressions;
using Ardalis.Specification;

namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.Sorting.Extensions
{
    public static class SpecificationSortBuilderExtensions
    {
        public static IOrderedSpecificationBuilder<T> OrderBy<T>(
            this ISpecificationBuilder<T> specificationBuilder,
            string[]? orderByFields
        )
        {
            if (orderByFields is not null)
            {
                foreach (var field in ParseOrderBy(orderByFields))
                {
                    var paramExpr = Expression.Parameter(typeof(T));

                    Expression propertyExpr = paramExpr;
                    foreach (string member in field.Key.Split('.'))
                    {
                        propertyExpr = Expression.PropertyOrField(propertyExpr, member);
                    }

                    var keySelector = Expression.Lambda<Func<T, object?>>(
                        Expression.Convert(propertyExpr, typeof(object)),
                        paramExpr
                    );

                    (
                        (List<OrderExpressionInfo<T>>)
                            specificationBuilder.Specification.OrderExpressions
                    ).Add(new OrderExpressionInfo<T>(keySelector, field.Value));
                }
            }

            return new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);
        }

        private static Dictionary<string, OrderTypeEnum> ParseOrderBy(string[] orderByFields)
        {
            return new Dictionary<string, OrderTypeEnum>(
                orderByFields.Select(
                    (orderByField, index) =>
                    {
                        string[] fieldParts = orderByField.Split(' ');
                        string field = fieldParts[0];
                        bool descending =
                            fieldParts.Length > 1
                            && fieldParts[1].StartsWith("Desc", StringComparison.OrdinalIgnoreCase);
                        OrderTypeEnum orderBy =
                            index == 0
                                ? descending
                                    ? OrderTypeEnum.OrderByDescending
                                    : OrderTypeEnum.OrderBy
                                : descending
                                    ? OrderTypeEnum.ThenByDescending
                                    : OrderTypeEnum.ThenBy;

                        return new KeyValuePair<string, OrderTypeEnum>(field, orderBy);
                    }
                )
            );
        }
    }
}
