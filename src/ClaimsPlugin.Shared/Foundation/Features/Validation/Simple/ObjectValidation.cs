using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple.Builders;

namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

public static class ObjectValidation
{
    public static bool IsNull([NotNullWhen(false)] this object? value)
    {
        return value is null;
    }

    public static bool IsNull(
        [NotNullWhen(false)] this object? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNull();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "null."
        );

        return result;
    }

    public static bool IsNotNull([NotNullWhen(true)] this object? value)
    {
        return !value.IsNull();
    }

    public static bool IsNotNull(
        [NotNullWhen(true)] this object? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotNull();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "null."
        );

        return result;
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static bool Is(this object value, Func<bool> func)
    {
        return func();
    }

    public static bool Is(
        this object value,
        Func<bool> func,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.Is(func);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "within the requirement."
        );

        return result;
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static bool IsNot(this object value, Func<bool> func)
    {
        return !func();
    }

    public static bool IsNot(
        this object value,
        Func<bool> func,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNot(func);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "within the requirement."
        );

        return result;
    }

    public static bool AnyNull(params object?[] values)
    {
        return values.IsNull() || values.Any(value => value.IsNull());
    }

    public static bool AnyNotNull(params object?[] values)
    {
        return values.IsNotNull() || values.Any(value => value.IsNotNull());
    }

    public static bool AllNull(params object?[] values)
    {
        return values.IsNull() || values.All(value => value.IsNull());
    }

    public static bool AllNotNull(params object?[] values)
    {
        return values.IsNotNull() || values.All(value => value.IsNotNull());
    }
}
