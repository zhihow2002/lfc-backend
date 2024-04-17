using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;
using Ardalis.Specification;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.AdvancedSearch.Models;
using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Queries;

namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.AdvancedSearch.Extensions;

public static class SpecificationSearchBuilderExtensions
{
    public static ISpecificationBuilder<T> SearchBy<T>(
        this ISpecificationBuilder<T> query,
        SearchQuery searchQuery
    )
    {
        return query.AdvancedSearch(searchQuery.AdvancedSearch);
    }

    private static IOrderedSpecificationBuilder<T> AdvancedSearch<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Search? search
    )
    {
        if (search is not null)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");

            Expression searchExpression;

            if (!string.IsNullOrEmpty(search.Logic))
            {
                if (search.Groups is null)
                {
                    throw new ArgumentException(
                        "The groups attribute is required when a logic attribute is provided in the block."
                    );
                }

                searchExpression = CreateSearchExpression(search.Logic, search.Groups, parameter);
            }
            else
            {
                Search result = ValidateSearch(search);

                searchExpression = CreateSearchExpression(
                    result.Field!,
                    result.Operator!,
                    result.Value,
                    parameter
                );
            }

            ((List<WhereExpressionInfo<T>>)specificationBuilder.Specification.WhereExpressions).Add(
                new WhereExpressionInfo<T>(
                    Expression.Lambda<Func<T, bool>>(searchExpression, parameter)
                )
            );
        }

        return new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);
    }

    private static Expression CreateSearchExpression(
        string logic,
        IEnumerable<Search> searches,
        ParameterExpression parameter
    )
    {
        Expression? outerSearchExpression = null;

        foreach (Search search in searches)
        {
            Expression innerSearchExpression;

            if (!string.IsNullOrEmpty(search.Logic))
            {
                if (search.Groups is null)
                {
                    throw new ArgumentException(
                        "The groups attribute is required when a logic attribute is provided in the block."
                    );
                }

                innerSearchExpression = CreateSearchExpression(
                    search.Logic,
                    search.Groups,
                    parameter
                );
            }
            else
            {
                Search result = ValidateSearch(search);

                innerSearchExpression = CreateSearchExpression(
                    result.Field!,
                    result.Operator,
                    result.Value,
                    parameter
                );
            }

            outerSearchExpression = outerSearchExpression is null
                ? innerSearchExpression
                : CombineLogic(logic, outerSearchExpression, innerSearchExpression);
        }

        if (outerSearchExpression is null)
        {
            throw new ArgumentException("Search expression should not be null after creation.");
        }

        return outerSearchExpression;
    }

    private static Expression CreateSearchExpression(
        string field,
        string searchOperator,
        object? value,
        ParameterExpression outerParameter
    )
    {
        Stack<Node> stack = new();

        Expression propertyExpression = GetPropertyExpression(
            field,
            outerParameter,
            out outerParameter,
            ref stack
        );
        ConstantExpression constantExpression = GetConstantExpression(
            field,
            value,
            propertyExpression.Type
        );
        propertyExpression = GetOperatorExpression(
            propertyExpression,
            constantExpression,
            searchOperator
        );

        while (stack.Count != 0)
        {
            LambdaExpression childFilter = Expression.Lambda(propertyExpression, outerParameter);
            Node parent = stack.Pop();

            propertyExpression = Expression.Call(
                typeof(Enumerable),
                nameof(Enumerable.Any),
                new[] { outerParameter.Type },
                parent.Body,
                childFilter
            );

            outerParameter = parent.Parameter;
        }

        return propertyExpression;
    }

    private static MemberExpression GetPropertyExpression(
        string propertyName,
        Expression innerExpression,
        out ParameterExpression outerExpression,
        ref Stack<Node> stack
    )
    {
        if (innerExpression is not ParameterExpression expression)
        {
            throw new ArgumentException("Expression must be a parameter expression.");
        }

        outerExpression = expression;

        foreach (string member in propertyName.Split('.'))
        {
            if (innerExpression.Type.IsGenericType)
            {
                Type[] genericArgs = innerExpression.Type.GetGenericArguments();
                if (
                    genericArgs.Length == 1
                    && typeof(IEnumerable<>)
                        .MakeGenericType(genericArgs)
                        .IsAssignableFrom(innerExpression.Type)
                )
                {
                    stack.Push(new Node(outerExpression, innerExpression));
                    innerExpression = outerExpression = Expression.Parameter(
                        genericArgs[0],
                        "s" + stack.Count
                    );
                }
            }

            try
            {
                innerExpression = Expression.PropertyOrField(innerExpression, member);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"The search field '{member}' is invalid.");
            }
        }

        return (MemberExpression)innerExpression;
    }

    private static Expression GetOperatorExpression(
        Expression memberExpression,
        Expression constantExpression,
        string searchOperator
    )
    {
        if (memberExpression is not MemberExpression)
        {
            throw new ArgumentException("Expression must be a property expression.");
        }

        if (constantExpression is not ConstantExpression)
        {
            throw new ArgumentException("Expression must be a constant expression.");
        }

        if (searchOperator == SearchOperator.Eq)
        {
            return Expression.Equal(memberExpression, constantExpression);
        }

        if (searchOperator == SearchOperator.Neq)
        {
            return Expression.NotEqual(memberExpression, constantExpression);
        }

        if (searchOperator == SearchOperator.Lt)
        {
            return Expression.LessThan(memberExpression, constantExpression);
        }

        if (searchOperator == SearchOperator.Lte)
        {
            return Expression.LessThanOrEqual(memberExpression, constantExpression);
        }

        if (searchOperator == SearchOperator.Gt)
        {
            return Expression.GreaterThan(memberExpression, constantExpression);
        }

        if (searchOperator == SearchOperator.Gte)
        {
            return Expression.GreaterThanOrEqual(memberExpression, constantExpression);
        }

        if (searchOperator == SearchOperator.Contains)
        {
            return Expression.Call(
                memberExpression,
                nameof(string.Contains),
                null,
                constantExpression
            );
        }

        if (searchOperator == SearchOperator.StartsWith)
        {
            return Expression.Call(
                memberExpression,
                nameof(string.StartsWith),
                null,
                constantExpression
            );
        }

        if (searchOperator == SearchOperator.EndsWith)
        {
            return Expression.Call(
                memberExpression,
                nameof(string.EndsWith),
                null,
                constantExpression
            );
        }

        throw new ArgumentException($"Operator '{searchOperator}' is invalid.");
    }

    private static ConstantExpression GetConstantExpression(
        string field,
        object? value,
        Type propertyType
    )
    {
        if (value == null)
        {
            return Expression.Constant(null, propertyType);
        }

        if (propertyType.IsEnum)
        {
            string stringEnum = GetStringFromJsonElement(value);

            if (!Enum.TryParse(propertyType, stringEnum, true, out object? valueParsed))
            {
                throw new ArgumentException($"Value '{value}' is not valid for {field}.");
            }

            return Expression.Constant(valueParsed, propertyType);
        }

        if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
        {
            string stringGuid = GetStringFromJsonElement(value);

            if (!Guid.TryParse(stringGuid, out Guid valueParsed))
            {
                throw new ArgumentException($"Value '{value}' is not valid for {field}.");
            }

            return Expression.Constant(valueParsed, propertyType);
        }

        if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
        {
            string stringDateTime = GetStringFromJsonElement(value);

            if (!DateTime.TryParse(stringDateTime, out DateTime valueParsed))
            {
                throw new ArgumentException($"Value '{value}' is not valid for {field}.");
            }

            return Expression.Constant(valueParsed, propertyType);
        }

        if (propertyType == typeof(DateTimeOffset) || propertyType == typeof(DateTimeOffset?))
        {
            string stringDateTime = GetStringFromJsonElement(value);

            if (!DateTimeOffset.TryParse(stringDateTime, out DateTimeOffset valueParsed))
            {
                throw new ArgumentException($"Value '{value}' is not valid for {field}.");
            }

            return Expression.Constant(valueParsed, propertyType);
        }

        if (propertyType == typeof(string))
        {
            string text = GetStringFromJsonElement(value);

            return Expression.Constant(text, propertyType);
        }

        return Expression.Constant(
            Convert.ChangeType(
                ((JsonElement)value).GetRawText(),
                propertyType,
                CultureInfo.InvariantCulture
            ),
            propertyType
        );
    }

    private static Expression CombineLogic(
        string searchLogic,
        Expression outerExpression,
        Expression innerExpression
    )
    {
        if (searchLogic == SearchLogic.And)
        {
            return Expression.And(outerExpression, innerExpression);
        }

        if (searchLogic == SearchLogic.Or)
        {
            return Expression.Or(outerExpression, innerExpression);
        }

        if (searchLogic == SearchLogic.Xor)
        {
            return Expression.ExclusiveOr(outerExpression, innerExpression);
        }

        throw new ArgumentException($"Logic '{searchLogic}' is invalid.");
    }

    private static string GetStringFromJsonElement(object value)
    {
        return ((JsonElement)value).GetString()!;
    }

    private static Search ValidateSearch(Search search)
    {
        if (string.IsNullOrEmpty(search.Field))
        {
            throw new ArgumentException(
                "The field attribute is required when groups and logic attributes are not provided in the block."
            );
        }

        if (string.IsNullOrEmpty(search.Operator))
        {
            throw new ArgumentException(
                "The operator attribute is required when groups and logic attributes are not provided in the block."
            );
        }

        return search;
    }

    private readonly struct Node : IEquatable<Node>
    {
        public Node(ParameterExpression parameter, Expression body)
        {
            Parameter = parameter;
            Body = body;
        }

        public ParameterExpression Parameter { get; }
        public Expression Body { get; }

        public bool Equals(Node other)
        {
            return Parameter.Equals(other.Parameter) && Body.Equals(other.Body);
        }

        public override bool Equals(object? obj)
        {
            return obj is Node other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Parameter, Body);
        }
    }
}
