using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple.Builders;
namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
public static class IntegerValidation
{
    public static bool IsNotNull([NotNullWhen(true)] this int? value)
    {
        return value.HasValue;
    }
    public static bool IsNotNull(
        [NotNullWhen(true)] this int? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotNull();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "null.");
        return result;
    }
    public static bool IsNull([NotNullWhen(false)] this int? value)
    {
        return !value.IsNotNull();
    }
    public static bool IsNull(
        [NotNullWhen(false)] this int? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNull();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "null.");
        return result;
    }
    public static bool IsNegative(this int value)
    {
        return value.IsLessThan(0);
    }
    public static bool IsNegative(
        this int value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNegative();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "negative.");
        return result;
    }
    public static bool IsNegative(this int? value)
    {
        return value.HasValue && value.Value.IsNegative();
    }
    public static bool IsNegative(
        this int? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNegative();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "negative.");
        return result;
    }
    public static bool IsPositive(this int value)
    {
        return value.IsGreaterThan(-1);
    }
    public static bool IsPositive(
        this int value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsPositive();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "positive.");
        return result;
    }
    public static bool IsPositive(this int? value)
    {
        return value.HasValue && value.Value.IsPositive();
    }
    public static bool IsPositive(
        this int? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsPositive();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "positive.");
        return result;
    }
    public static bool IsZero(this int value)
    {
        return value == 0;
    }
    public static bool IsZero(
        this int value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsZero();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "zero.");
        return result;
    }
    public static bool IsZero(this int? value)
    {
        return value.HasValue && value.Value.IsZero();
    }
    public static bool IsZero(
        this int? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsZero();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "zero.");
        return result;
    }
    public static bool IsNotZero(this int value)
    {
        return value != 0;
    }
    public static bool IsNotZero(
        this int value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotZero();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "zero.");
        return result;
    }
    public static bool IsNotZero(this int? value)
    {
        return !value.HasValue || value.Value.IsNotZero();
    }
    public static bool IsNotZero(
        this int? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotZero();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "zero.");
        return result;
    }
    public static bool IsEqualTo(this int value, int compare)
    {
        return value == compare;
    }
    public static bool IsEqualTo(
        this int value,
        int compare,
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
    public static bool IsEqualTo(this int? value, int compare)
    {
        return value.HasValue && value.Value.IsEqualTo(compare);
    }
    public static bool IsEqualTo(
        this int? value,
        int compare,
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
    public static bool IsNotEqualTo(this int value, int compare)
    {
        return value != compare;
    }
    public static bool IsNotEqualTo(
        this int value,
        int compare,
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
    public static bool IsNotEqualTo(this int? value, int compare)
    {
        return !value.HasValue || value.Value.IsNotEqualTo(compare);
    }
    public static bool IsNotEqualTo(
        this int? value,
        int compare,
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
    public static bool IsGreaterThan(this int value, int min)
    {
        return value > min;
    }
    public static bool IsGreaterThan(
        this int value,
        int min,
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
    public static bool IsGreaterThan(this int? value, int min)
    {
        return value.HasValue && value.Value.IsGreaterThan(min);
    }
    public static bool IsGreaterThan(
        this int? value,
        int min,
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
    public static bool IsGreaterThanOrEqualTo(this int value, int min)
    {
        return value >= min;
    }
    public static bool IsGreaterThanOrEqualTo(
        this int value,
        int min,
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
    public static bool IsGreaterThanOrEqualTo(this int? value, int min)
    {
        return value.HasValue && value.Value.IsGreaterThanOrEqualTo(min);
    }
    public static bool IsGreaterThanOrEqualTo(
        this int? value,
        int min,
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
    public static bool IsLessThan(this int value, int max)
    {
        return value < max;
    }
    public static bool IsLessThan(
        this int value,
        int max,
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
    public static bool IsLessThan(this int? value, int max)
    {
        return value.HasValue && value.Value.IsLessThan(max);
    }
    public static bool IsLessThan(
        this int? value,
        int max,
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
    public static bool IsLessThanOrEqualTo(this int value, int max)
    {
        return value <= max;
    }
    public static bool IsLessThanOrEqualTo(
        this int value,
        int max,
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
    public static bool IsLessThanOrEqualTo(this int? value, int max)
    {
        return value.HasValue && value.Value.IsLessThanOrEqualTo(max);
    }
    public static bool IsLessThanOrEqualTo(
        this int? value,
        int max,
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
    public static bool IsBetween(this int value, int min, int max)
    {
        return value >= min && value <= max;
    }
    public static bool IsBetween(
        this int value,
        int min,
        int max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsBetween(min, max);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"between '{min}' and '{max}'."
        );
        return result;
    }
    public static bool IsBetween(this int? value, int min, int max)
    {
        return value.HasValue && value.Value.IsBetween(min, max);
    }
    public static bool IsBetween(
        this int? value,
        int min,
        int max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsBetween(min, max);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"between '{min}' and '{max}'."
        );
        return result;
    }
    public static bool IsNotBetween(this int value, int min, int max)
    {
        return value < min || value > max;
    }
    public static bool IsNotBetween(
        this int value,
        int min,
        int max,
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
    public static bool IsNotBetween(this int? value, int min, int max)
    {
        return !value.HasValue || value.Value.IsBetween(min, max);
    }
    public static bool IsNotBetween(
        this int? value,
        int min,
        int max,
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
    public static bool AnyNull(params int?[] values)
    {
        return values.IsNull() || values.Any(value => value.IsNull());
    }
    public static bool AnyNotNull(params int?[] values)
    {
        return values.IsNotNull() || values.Any(value => value.IsNotNull());
    }
    public static bool AllNull(params int?[] values)
    {
        return values.IsNull() || values.All(value => value.IsNull());
    }
    public static bool AllNotNull(params int?[] values)
    {
        return values.IsNotNull() || values.All(value => value.IsNotNull());
    }
}
