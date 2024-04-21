using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple.Builders;

namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

public static class DoubleValidation
{
    public static bool IsNotNull([NotNullWhen(true)] this double? value)
    {
        return value.HasValue;
    }

    public static bool IsNotNull(
        [NotNullWhen(true)] this double? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotNull();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "null.");

        return result;
    }

    public static bool IsNull([NotNullWhen(false)] this double? value)
    {
        return !value.IsNotNull();
    }

    public static bool IsNull(
        [NotNullWhen(false)] this double? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNull();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "null.");

        return result;
    }

    public static bool IsNegative(this double value)
    {
        return value.IsLessThan(0);
    }

    public static bool IsNegative(
        this double value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNegative();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "negative.");

        return result;
    }

    public static bool IsNegative(this double? value)
    {
        return value.HasValue && value.Value.IsNegative();
    }

    public static bool IsNegative(
        this double? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNegative();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "negative.");

        return result;
    }

    public static bool IsPositive(this double value)
    {
        return value.IsGreaterThan(-1);
    }

    public static bool IsPositive(
        this double value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsPositive();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "positive.");

        return result;
    }

    public static bool IsPositive(this double? value)
    {
        return value.HasValue && value.Value.IsPositive();
    }

    public static bool IsPositive(
        this double? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsPositive();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "positive.");

        return result;
    }

    public static bool IsZero(this double value)
    {
        return Math.Abs(value) < 0.0001;
    }

    public static bool IsZero(
        this double value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsZero();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "zero.");

        return result;
    }

    public static bool IsZero(this double? value)
    {
        return value.HasValue && value.Value.IsZero();
    }

    public static bool IsZero(
        this double? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsZero();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "zero.");

        return result;
    }

    public static bool IsNotZero(this double value)
    {
        return Math.Abs(value) >= 0.0001;
    }

    public static bool IsNotZero(
        this double value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotZero();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "zero.");

        return result;
    }

    public static bool IsNotZero(this double? value)
    {
        return !value.HasValue || value.Value.IsNotZero();
    }

    public static bool IsNotZero(
        this double? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotZero();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "zero.");

        return result;
    }

    public static bool IsEqualTo(this double value, double compare)
    {
        return Math.Abs(value - compare) < 0.0001;
    }

    public static bool IsEqualTo(
        this double value,
        double compare,
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

    public static bool IsEqualTo(this double? value, double compare)
    {
        return value.HasValue && value.Value.IsEqualTo(compare);
    }

    public static bool IsEqualTo(
        this double? value,
        double compare,
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

    public static bool IsNotEqualTo(this double value, double compare)
    {
        return Math.Abs(value - compare) > 0.0001;
    }

    public static bool IsNotEqualTo(
        this double value,
        double compare,
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

    public static bool IsNotEqualTo(this double? value, double compare)
    {
        return !value.HasValue || value.Value.IsNotEqualTo(compare);
    }

    public static bool IsNotEqualTo(
        this double? value,
        double compare,
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

    public static bool IsGreaterThan(this double value, double min)
    {
        return value > min;
    }

    public static bool IsGreaterThan(
        this double value,
        double min,
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

    public static bool IsGreaterThan(this double? value, double min)
    {
        return value.HasValue && value.Value.IsGreaterThan(min);
    }

    public static bool IsGreaterThan(
        this double? value,
        double min,
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

    public static bool IsGreaterThanOrEqualTo(this double value, double min)
    {
        return value >= min;
    }

    public static bool IsGreaterThanOrEqualTo(
        this double value,
        double min,
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

    public static bool IsGreaterThanOrEqualTo(this double? value, double min)
    {
        return value.HasValue && value.Value.IsGreaterThanOrEqualTo(min);
    }

    public static bool IsGreaterThanOrEqualTo(
        this double? value,
        double min,
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

    public static bool IsLessThan(this double value, double max)
    {
        return value < max;
    }

    public static bool IsLessThan(
        this double value,
        double max,
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

    public static bool IsLessThan(this double? value, double max)
    {
        return value.HasValue && value.Value.IsLessThan(max);
    }

    public static bool IsLessThan(
        this double? value,
        double max,
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

    public static bool IsLessThanOrEqualTo(this double value, double max)
    {
        return value <= max;
    }

    public static bool IsLessThanOrEqualTo(
        this double value,
        double max,
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

    public static bool IsLessThanOrEqualTo(this double? value, double max)
    {
        return value.HasValue && value.Value.IsLessThanOrEqualTo(max);
    }

    public static bool IsLessThanOrEqualTo(
        this double? value,
        double max,
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

    public static bool IsBetween(this double value, double min, double max)
    {
        return value >= min && value <= max;
    }

    public static bool IsBetween(
        this double value,
        double min,
        double max,
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

    public static bool IsBetween(this double? value, double min, double max)
    {
        return value.HasValue && value.Value.IsBetween(min, max);
    }

    public static bool IsBetween(
        this double? value,
        double min,
        double max,
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

    public static bool IsNotBetween(this double value, double min, double max)
    {
        return value < min || value > max;
    }

    public static bool IsNotBetween(
        this double value,
        double min,
        double max,
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

    public static bool IsNotBetween(this double? value, double min, double max)
    {
        return !value.HasValue || value.Value.IsBetween(min, max);
    }

    public static bool IsNotBetween(
        this double? value,
        double min,
        double max,
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

    public static bool AnyNull(params double?[] values)
    {
        return values.IsNull() || values.Any(value => value.IsNull());
    }

    public static bool AnyNotNull(params double?[] values)
    {
        return values.IsNotNull() || values.Any(value => value.IsNotNull());
    }

    public static bool AllNull(params double?[] values)
    {
        return values.IsNull() || values.All(value => value.IsNull());
    }

    public static bool AllNotNull(params double?[] values)
    {
        return values.IsNotNull() || values.All(value => value.IsNotNull());
    }
}
