using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple.Builders;
using JetBrains.Annotations;

namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

public static class CollectionValidation
{
    public static bool IsNotNullOrEmpty<T>([NotNullWhen(true)] this List<T>? value)
    {
        return value.IsNotNull() && value.Count > 0;
    }

    public static bool IsNotNullOrEmpty<T>(
        [NotNullWhen(true)] this List<T>? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotNullOrEmpty();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "null or empty."
        );

        return result;
    }

    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this List<T>? value)
    {
        return value.IsNull() || value.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(
        [NotNullWhen(false)] this List<T>? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNullOrEmpty();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "null or empty."
        );

        return result;
    }

    public static bool IsNotNullOrEmpty<T>(
        [NoEnumeration] [NotNullWhen(true)] this IEnumerable<T>? value
    )
    {
        return value.IsNotNull() && value.Any();
    }

    public static bool IsNotNullOrEmpty<T>(
        [NotNullWhen(true)] this IEnumerable<T>? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotNullOrEmpty();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "null or empty."
        );

        return result;
    }

    public static bool IsNullOrEmpty<T>(
        [NoEnumeration] [NotNullWhen(false)] this IEnumerable<T>? value
    )
    {
        return value.IsNull() || !value.Any();
    }

    public static bool IsNullOrEmpty<T>(
        [NotNullWhen(false)] this IEnumerable<T>? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNullOrEmpty();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "null or empty."
        );

        return result;
    }

    public static bool HasUnmatchedItems<T, TResult>(
        this IEnumerable<T> items,
        Func<T, TResult> keySelector,
        IEnumerable<TResult> compares,
        out List<TResult> unmatchedItems
    )
    {
        unmatchedItems = items.Select(keySelector).Distinct().Except(compares).ToList();

        return unmatchedItems.Count > 0;
    }

    public static bool HasNoUnmatchedItems<T, TResult>(
        this IEnumerable<T> items,
        Func<T, TResult> keySelector,
        IEnumerable<TResult> compares,
        out List<TResult> unmatchedItems
    )
        where TResult : struct
    {
        return !items.HasUnmatchedItems(keySelector, compares, out unmatchedItems);
    }

    public static bool HasMatchedItems<T, TResult>(
        this IEnumerable<T> items,
        Func<T, TResult> keySelector,
        IEnumerable<TResult> compares,
        out List<TResult> matchedItems
    )
    {
        matchedItems = items
            .Select(keySelector)
            .Distinct()
            .Join(compares, f => f, s => s, (fir, sec) => fir)
            .ToList();

        return matchedItems.Count > 0;
    }

    public static bool HasNoMatchedItems<T, TResult>(
        this IEnumerable<T> items,
        Func<T, TResult> keySelector,
        IEnumerable<TResult> compares,
        out List<TResult> matchedItems
    )
    {
        return !items.HasMatchedItems(keySelector, compares, out matchedItems);
    }
}
