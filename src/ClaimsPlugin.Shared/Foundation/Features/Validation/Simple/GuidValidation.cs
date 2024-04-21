using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple.Builders;

namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

public static class GuidValidation
{
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] this Guid? value)
    {
        return value.HasValue && value.Value.IsNotEmpty();
    }

    public static bool IsNotNullOrEmpty(
        [NotNullWhen(true)] this Guid? value,
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

    public static bool IsNullOrEmpty([NotNullWhen(false)] this Guid? value)
    {
        return !value.IsNotNullOrEmpty();
    }

    public static bool IsNullOrEmpty(
        [NotNullWhen(false)] this Guid? value,
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

    public static bool IsNull([NotNullWhen(false)] this Guid? value)
    {
        return !value.IsNotNull();
    }

    public static bool IsNull(
        [NotNullWhen(false)] this Guid? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNull();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "null.");

        return result;
    }

    public static bool IsNotNull([NotNullWhen(true)] this Guid? value)
    {
        return value.HasValue;
    }

    public static bool IsNotNull(
        [NotNullWhen(true)] this Guid? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotNull();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "null.");

        return result;
    }

    public static bool IsEmpty(this Guid value)
    {
        return !value.IsNotEmpty();
    }

    public static bool IsEmpty(
        this Guid value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEmpty();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "empty.");

        return result;
    }

    public static bool IsEmpty(this Guid? value)
    {
        return value.IsNotNull() && value.Value.IsEmpty();
    }

    public static bool IsEmpty(
        this Guid? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEmpty();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "empty.");

        return result;
    }

    public static bool IsNotEmpty(this Guid value)
    {
        return value != Guid.Empty;
    }

    public static bool IsNotEmpty(
        this Guid value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotEmpty();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "empty.");

        return result;
    }

    public static bool IsNotEmpty(this Guid? value)
    {
        return value.IsNotNull() && value.Value.IsNotEmpty();
    }

    public static bool IsNotEmpty(
        this Guid? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotEmpty();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "empty.");

        return result;
    }

    public static bool AnyNullOrEmpty(params Guid?[] values)
    {
        return values.IsNull() || values.Any(value => value.IsNullOrEmpty());
    }

    public static bool AnyNull(params Guid?[] values)
    {
        return values.IsNull() || values.Any(value => value.IsNull());
    }

    public static bool AnyNotNullOrEmpty(params Guid?[] values)
    {
        return values.IsNotNull() || values.Any(value => value.IsNotNullOrEmpty());
    }

    public static bool AnyNotNull(params Guid?[] values)
    {
        return values.IsNotNull() || values.Any(value => value.IsNotNull());
    }

    public static bool AllNullOrEmpty(params Guid?[] values)
    {
        return values.IsNull() || values.All(value => value.IsNullOrEmpty());
    }

    public static bool AllNull(params Guid?[] values)
    {
        return values.IsNull() || values.All(value => value.IsNull());
    }

    public static bool AllNotNullOrEmpty(params Guid?[] values)
    {
        return values.IsNotNull() || values.All(value => value.IsNotNullOrEmpty());
    }

    public static bool AllNotNull(params Guid?[] values)
    {
        return values.IsNotNull() || values.All(value => value.IsNotNull());
    }
}
