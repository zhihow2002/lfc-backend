using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple.Builders;

namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

public static class DecimalValidation
{
    public static bool IsNotNull([NotNullWhen(true)] this decimal? value)
    {
        return value.HasValue;
    }

    public static bool IsNotNull(
        [NotNullWhen(true)] this decimal? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotNull();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "null.");

        return result;
    }

    public static bool IsNull([NotNullWhen(false)] this decimal? value)
    {
        return !value.IsNotNull();
    }

    public static bool IsNull(
        [NotNullWhen(false)] this decimal? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNull();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "null.");

        return result;
    }

    public static bool IsNegative(this decimal value)
    {
        return value.IsLessThan(0);
    }

    public static bool IsNegative(
        this decimal value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNegative();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "negative.");

        return result;
    }

    public static bool IsNegative(this decimal? value)
    {
        return value.HasValue && value.Value.IsNegative();
    }

    public static bool IsNegative(
        this decimal? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNegative();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "negative.");

        return result;
    }

    public static bool IsPositive(this decimal value)
    {
        return value.IsGreaterThan(-1);
    }

    public static bool IsPositive(
        this decimal value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsPositive();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "positive.");

        return result;
    }

    public static bool IsPositive(this decimal? value)
    {
        return value.HasValue && value.Value.IsPositive();
    }

    public static bool IsPositive(
        this decimal? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsPositive();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "positive.");

        return result;
    }

    public static bool IsZero(this decimal value)
    {
        return value == 0;
    }

    public static bool IsZero(
        this decimal value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsZero();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "zero.");

        return result;
    }

    public static bool IsZero(this decimal? value)
    {
        return value.HasValue && value.Value.IsZero();
    }

    public static bool IsZero(
        this decimal? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsZero();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "zero.");

        return result;
    }

    public static bool IsNotZero(this decimal value)
    {
        return value != 0;
    }

    public static bool IsNotZero(
        this decimal value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotZero();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "zero.");

        return result;
    }

    public static bool IsNotZero(this decimal? value)
    {
        return !value.HasValue || value.Value.IsNotZero();
    }

    public static bool IsNotZero(
        this decimal? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotZero();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "zero.");

        return result;
    }

    public static bool IsEqualTo(this decimal value, decimal compare)
    {
        return value == compare;
    }

    public static bool IsEqualTo(
        this decimal value,
        decimal compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEqualTo(compare);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"equal to '{compare}'."
        );

        return result;
    }

    public static bool IsEqualTo(this decimal? value, decimal compare)
    {
        return value.HasValue && value.Value.IsEqualTo(compare);
    }

    public static bool IsEqualTo(
        this decimal? value,
        decimal compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEqualTo(compare);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"equal to '{compare}'."
        );

        return result;
    }

    public static bool IsNotEqualTo(this decimal value, decimal compare)
    {
        return value != compare;
    }

    public static bool IsNotEqualTo(
        this decimal value,
        decimal compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotEqualTo(compare);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            $"equal to '{compare}'."
        );

        return result;
    }

    public static bool IsNotEqualTo(this decimal? value, decimal compare)
    {
        return !value.HasValue || value.Value.IsNotEqualTo(compare);
    }

    public static bool IsNotEqualTo(
        this decimal? value,
        decimal compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotEqualTo(compare);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            $"equal to '{compare}'."
        );

        return result;
    }

    public static bool IsGreaterThan(this decimal value, decimal min)
    {
        return value > min;
    }

    public static bool IsGreaterThan(
        this decimal value,
        decimal min,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? sourceParameterName = default,
        [CallerArgumentExpression("min")] string? targetParameterName = default
    )
    {
        bool result = value.IsGreaterThan(min);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            sourceParameterName,
            $"greater than {min.GetParameterNameAndValue(targetParameterName)}."
        );

        return result;
    }

    public static bool IsGreaterThan(this decimal? value, decimal min)
    {
        return value.HasValue && value.Value.IsGreaterThan(min);
    }

    public static bool IsGreaterThan(
        this decimal? value,
        decimal min,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? sourceParameterName = default,
        [CallerArgumentExpression("min")] string? targetParameterName = default
    )
    {
        bool result = value.IsGreaterThan(min);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            sourceParameterName,
            $"greater than {min.GetParameterNameAndValue(targetParameterName)}."
        );

        return result;
    }

    public static bool IsGreaterThanOrEqualTo(this decimal value, decimal min)
    {
        return value >= min;
    }

    public static bool IsGreaterThanOrEqualTo(
        this decimal value,
        decimal min,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? sourceParameterName = default,
        [CallerArgumentExpression("min")] string? targetParameterName = default
    )
    {
        bool result = value.IsGreaterThanOrEqualTo(min);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            sourceParameterName,
            $"greater than or equal to {min.GetParameterNameAndValue(targetParameterName)}."
        );

        return result;
    }

    public static bool IsGreaterThanOrEqualTo(this decimal? value, decimal min)
    {
        return value.HasValue && value.Value.IsGreaterThanOrEqualTo(min);
    }

    public static bool IsGreaterThanOrEqualTo(
        this decimal? value,
        decimal min,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? sourceParameterName = default,
        [CallerArgumentExpression("min")] string? targetParameterName = default
    )
    {
        bool result = value.IsGreaterThanOrEqualTo(min);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            sourceParameterName,
            $"greater than or equal to {min.GetParameterNameAndValue(targetParameterName)}."
        );

        return result;
    }

    public static bool IsLessThan(this decimal value, decimal max)
    {
        return value < max;
    }

    public static bool IsLessThan(
        this decimal value,
        decimal max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? sourceParameterName = default,
        [CallerArgumentExpression("max")] string? targetParameterName = default
    )
    {
        bool result = value.IsLessThan(max);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            sourceParameterName,
            $"less than {max.GetParameterNameAndValue(targetParameterName)}."
        );

        return result;
    }

    public static bool IsLessThan(this decimal? value, decimal max)
    {
        return value.HasValue && value.Value.IsLessThan(max);
    }

    public static bool IsLessThan(
        this decimal? value,
        decimal max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? sourceParameterName = default,
        [CallerArgumentExpression("max")] string? targetParameterName = default
    )
    {
        bool result = value.IsLessThan(max);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            sourceParameterName,
            $"less than {max.GetParameterNameAndValue(targetParameterName)}."
        );

        return result;
    }

    public static bool IsLessThanOrEqualTo(this decimal value, decimal max)
    {
        return value <= max;
    }

    public static bool IsLessThanOrEqualTo(
        this decimal value,
        decimal max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? sourceParameterName = default,
        [CallerArgumentExpression("max")] string? targetParameterName = default
    )
    {
        bool result = value.IsLessThanOrEqualTo(max);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            sourceParameterName,
            $"less than or equal to {max.GetParameterNameAndValue(targetParameterName)}."
        );

        return result;
    }

    public static bool IsLessThanOrEqualTo(this decimal? value, decimal max)
    {
        return value.HasValue && value.Value.IsLessThanOrEqualTo(max);
    }

    public static bool IsLessThanOrEqualTo(
        this decimal? value,
        decimal max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? sourceParameterName = default,
        [CallerArgumentExpression("max")] string? targetParameterName = default
    )
    {
        bool result = value.IsLessThanOrEqualTo(max);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            sourceParameterName,
            $"less than or equal to {max.GetParameterNameAndValue(targetParameterName)}."
        );

        return result;
    }

    public static bool IsBetween(this decimal value, decimal min, decimal max)
    {
        return value >= min && value <= max;
    }

    public static bool IsBetween(
        this decimal value,
        decimal min,
        decimal max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? sourceParameterName = default
    )
    {
        bool result = value.IsBetween(min, max);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            sourceParameterName,
            $"between '{min}' and '{max}'."
        );

        return result;
    }

    public static bool IsBetween(this decimal? value, decimal min, decimal max)
    {
        return value.HasValue && value.Value.IsBetween(min, max);
    }

    public static bool IsBetween(
        this decimal? value,
        decimal min,
        decimal max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? sourceParameterName = default
    )
    {
        bool result = value.IsBetween(min, max);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            sourceParameterName,
            $"between '{min}' and '{max}'."
        );

        return result;
    }

    public static bool IsNotBetween(this decimal value, decimal min, decimal max)
    {
        return value < min || value > max;
    }

    public static bool IsNotBetween(
        this decimal value,
        decimal min,
        decimal max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotBetween(min, max);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            $"between '{min}' and '{max}'."
        );

        return result;
    }

    public static bool IsNotBetween(this decimal? value, decimal min, decimal max)
    {
        return !value.HasValue || value.Value.IsBetween(min, max);
    }

    public static bool IsNotBetween(
        this decimal? value,
        decimal min,
        decimal max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotBetween(min, max);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            $"between '{min}' and '{max}'."
        );

        return result;
    }

    public static bool AnyNull(params decimal?[] values)
    {
        return values.IsNull() || values.Any(value => value.IsNull());
    }

    public static bool AnyNotNull(params decimal?[] values)
    {
        return values.IsNotNull() && values.Any(value => value.IsNotNull());
    }

    public static bool AllNull(params decimal?[] values)
    {
        return values.IsNull() || values.All(value => value.IsNull());
    }

    public static bool AllNotNull(params decimal?[] values)
    {
        return values.IsNotNull() || values.All(value => value.IsNotNull());
    }
}
