using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple.Builders;
namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
public static class DateTimeValidation
{
    public static bool IsNotNull([NotNullWhen(true)] this DateTime? value)
    {
        return value.HasValue;
    }
    public static bool IsNotNull(
        [NotNullWhen(true)] this DateTime? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotNull();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "null.");
        return result;
    }
    public static bool IsNull([NotNullWhen(false)] this DateTime? value)
    {
        return !value.IsNotNull();
    }
    public static bool IsNull(
        [NotNullWhen(false)] this DateTime? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNull();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "null.");
        return result;
    }
    public static bool IsDate(this object value)
    {
        return value.IsDate(CultureInfo.InvariantCulture);
    }
    public static bool IsDate(
        this object value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsDate();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "a date.");
        return result;
    }
    public static bool IsDate(this object value, CultureInfo info)
    {
        return value.IsDate(info, DateTimeStyles.None);
    }
    public static bool IsDate(
        this object value,
        CultureInfo info,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsDate(info);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "a date.");
        return result;
    }
    public static bool IsDate(this object? value, CultureInfo info, DateTimeStyles styles)
    {
        if (value.IsNotNull())
        {
            if (DateTime.TryParse(value.ToString(), info, styles, out DateTime _))
            {
                return true;
            }
            return false;
        }
        return false; // if it's null it cannot be a date
    }
    public static bool IsDate(
        this object value,
        CultureInfo info,
        DateTimeStyles styles,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsDate(info, styles);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "a date.");
        return result;
    }
    public static bool IsNotDate(this object value)
    {
        return !value.IsDate(CultureInfo.InvariantCulture);
    }
    public static bool IsNotDate(
        this object value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotDate();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "a date.");
        return result;
    }
    public static bool IsNotDate(this object value, CultureInfo info)
    {
        return !value.IsDate(info, DateTimeStyles.None);
    }
    public static bool IsNotDate(
        this object value,
        CultureInfo info,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotDate(info);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "a date.");
        return result;
    }
    public static bool IsNotDate(this object? value, CultureInfo info, DateTimeStyles styles)
    {
        if (value.IsNotNull())
        {
            if (DateTime.TryParse(value.ToString(), info, styles, out DateTime _))
            {
                return false;
            }
        }
        return true;
    }
    public static bool IsNotDate(
        this object value,
        CultureInfo info,
        DateTimeStyles styles,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotDate(info, styles);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.IsNot, parameterName, "a date.");
        return result;
    }
    public static bool IsLaterThan(this DateTime value, DateTime compare)
    {
        return value > compare;
    }
    public static bool IsLaterThan(this DateTime value, DateTime? compare)
    {
        return value > compare;
    }
    public static bool IsLaterThan(
        this DateTime value,
        DateTime compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsLaterThan(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"later than '{compare}'."
        );
        return result;
    }
    public static bool IsLaterThan(this DateTime? value, DateTime compare)
    {
        return value.HasValue && value.Value.IsLaterThan(compare);
    }
    public static bool IsLaterThan(this DateTime? value, DateTime? compare)
    {
        return value.HasValue && value.Value.IsLaterThan(compare);
    }
    public static bool IsLaterThan(
        this DateTime? value,
        DateTime compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsLaterThan(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"later than '{compare}'."
        );
        return result;
    }
    public static bool IsLaterThan(
        this DateTime? value,
        DateTime? compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsLaterThan(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"later than '{compare}'."
        );
        return result;
    }
    public static bool IsLaterThanOrEqualTo(this DateTime value, DateTime compare)
    {
        return value >= compare;
    }
    public static bool IsLaterThanOrEqualTo(this DateTime value, DateTime? compare)
    {
        return value >= compare;
    }
    public static bool IsLaterThanOrEqualTo(
        this DateTime value,
        DateTime compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsLaterThanOrEqualTo(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"later than or equal to '{compare}'."
        );
        return result;
    }
    public static bool IsLaterThanOrEqualTo(
        this DateTime value,
        DateTime? compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsLaterThanOrEqualTo(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"later than or equal to '{compare}'."
        );
        return result;
    }
    public static bool IsLaterThanOrEqualTo(this DateTime? value, DateTime compare)
    {
        return value.HasValue && value.Value.IsLaterThanOrEqualTo(compare);
    }
    public static bool IsLaterThanOrEqualTo(this DateTime? value, DateTime? compare)
    {
        return value.HasValue && value.Value.IsLaterThanOrEqualTo(compare);
    }
    public static bool IsLaterThanOrEqualTo(
        this DateTime? value,
        DateTime compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsLaterThanOrEqualTo(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"later than or equal to '{compare}'."
        );
        return result;
    }
    public static bool IsLaterThanOrEqualTo(
        this DateTime? value,
        DateTime? compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsLaterThanOrEqualTo(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"later than or equal to '{compare}'."
        );
        return result;
    }
    public static bool IsEarlierThan(this DateTime value, DateTime compare)
    {
        return value < compare;
    }
    public static bool IsEarlierThan(this DateTime value, DateTime? compare)
    {
        return value < compare;
    }
    public static bool IsEarlierThan(
        this DateTime value,
        DateTime compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEarlierThan(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"earlier than '{compare}'."
        );
        return result;
    }
    public static bool IsEarlierThan(
        this DateTime value,
        DateTime? compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEarlierThan(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"earlier than '{compare}'."
        );
        return result;
    }
    public static bool IsEarlierThan(this DateTime? value, DateTime compare)
    {
        return value.HasValue && value.Value.IsEarlierThan(compare);
    }
    public static bool IsEarlierThan(this DateTime? value, DateTime? compare)
    {
        return value.HasValue && value.Value.IsEarlierThan(compare);
    }
    public static bool IsEarlierThan(
        this DateTime? value,
        DateTime compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEarlierThan(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"earlier than '{compare}'."
        );
        return result;
    }
    public static bool IsEarlierThan(
        this DateTime? value,
        DateTime? compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEarlierThan(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"earlier than '{compare}'."
        );
        return result;
    }
    public static bool IsEarlierThanOrEqualTo(this DateTime value, DateTime compare)
    {
        return value <= compare;
    }
    public static bool IsEarlierThanOrEqualTo(this DateTime value, DateTime? compare)
    {
        return value <= compare;
    }
    public static bool IsEarlierThanOrEqualTo(
        this DateTime value,
        DateTime compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEarlierThanOrEqualTo(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"earlier than or equal to '{compare}'."
        );
        return result;
    }
    public static bool IsEarlierThanOrEqualTo(
        this DateTime value,
        DateTime? compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEarlierThanOrEqualTo(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"earlier than or equal to '{compare}'."
        );
        return result;
    }
    public static bool IsEarlierThanOrEqualTo(this DateTime? value, DateTime compare)
    {
        return value.HasValue && value.Value.IsEarlierThanOrEqualTo(compare);
    }
    public static bool IsEarlierThanOrEqualTo(this DateTime? value, DateTime? compare)
    {
        return value.HasValue && value.Value.IsEarlierThanOrEqualTo(compare);
    }
    public static bool IsEarlierThanOrEqualTo(
        this DateTime? value,
        DateTime compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEarlierThanOrEqualTo(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"earlier than or equal to '{compare}'."
        );
        return result;
    }
    public static bool IsEarlierThanOrEqualTo(
        this DateTime? value,
        DateTime? compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEarlierThanOrEqualTo(compare);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"earlier than or equal to '{compare}'."
        );
        return result;
    }
    public static bool IsEqualTo(this DateTime value, DateTime compare)
    {
        return value == compare;
    }
    public static bool IsEqualTo(
        this DateTime value,
        DateTime compare,
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
    public static bool IsEqualTo(this DateTime? value, DateTime compare)
    {
        return value.HasValue && value.Value.IsEqualTo(compare);
    }
    public static bool IsEqualTo(
        this DateTime? value,
        DateTime compare,
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
    public static bool IsNotEqualTo(this DateTime value, DateTime compare)
    {
        return !value.IsEqualTo(compare);
    }
    public static bool IsNotEqualTo(
        this DateTime value,
        DateTime compare,
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
    public static bool IsNotEqualTo(this DateTime? value, DateTime compare)
    {
        return !value.HasValue || value.Value.IsNotEqualTo(compare);
    }
    public static bool IsNotEqualTo(
        this DateTime? value,
        DateTime compare,
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
    public static bool IsBetween(this DateTime value, DateTime from, DateTime to)
    {
        return value >= from && (to.IsNull() || value <= to);
    }
    public static bool IsBetween(this DateTime value, DateTime? from, DateTime? to)
    {
        return value >= from && (to.IsNull() || value <= to);
    }
    public static bool IsBetweenDate(this DateTime value, DateTime from, DateTime? to)
    {
        return value.Date >= from.Date && (to.IsNull() || value <= to);
    }
    public static bool IsBetween(
        this DateTime value,
        DateTime from,
        DateTime to,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsBetween(from, to);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"between '{from}' and '{to}'."
        );
        return result;
    }
    public static bool IsBetween(
        this DateTime value,
        DateTime? from,
        DateTime? to,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsBetween(from, to);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"between '{from}' and '{to}'."
        );
        return result;
    }
    public static bool IsBetween(this DateTime? value, DateTime from, DateTime to)
    {
        return value.HasValue && value.Value.IsBetween(from, to);
    }
    public static bool IsBetween(this DateTime? value, DateTime? from, DateTime? to)
    {
        return value.HasValue && value.Value.IsBetween(from, to);
    }
    public static bool IsBetween(
        this DateTime? value,
        DateTime from,
        DateTime to,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsBetween(from, to);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"between '{from}' and '{to}'."
        );
        return result;
    }
    public static bool IsBetween(
        this DateTime? value,
        DateTime? from,
        DateTime? to,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsBetween(from, to);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"between '{from}' and '{to}'."
        );
        return result;
    }
    public static bool IsNotBetween(this DateTime value, DateTime from, DateTime to)
    {
        return value < from || value > to;
    }
    public static bool IsNotBetween(this DateTime value, DateTime? from, DateTime? to)
    {
        return value < from || value > to;
    }
    public static bool IsNotBetween(
        this DateTime value,
        DateTime from,
        DateTime to,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotBetween(from, to);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            $"between '{from}' and '{to}'."
        );
        return result;
    }
    public static bool IsNotBetween(
        this DateTime value,
        DateTime? from,
        DateTime? to,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotBetween(from, to);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            $"between '{from}' and '{to}'."
        );
        return result;
    }
    public static bool IsNotBetween(this DateTime? value, DateTime from, DateTime to)
    {
        return !value.HasValue || value.Value.IsNotBetween(from, to);
    }
    public static bool IsNotBetween(this DateTime? value, DateTime? from, DateTime? to)
    {
        return !value.HasValue || value.Value.IsNotBetween(from, to);
    }
    public static bool IsNotBetween(
        this DateTime? value,
        DateTime from,
        DateTime to,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotBetween(from, to);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            $"between '{from}' and '{to}'."
        );
        return result;
    }
    public static bool IsNotBetween(
        this DateTime? value,
        DateTime? from,
        DateTime? to,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotBetween(from, to);
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            $"between '{from}' and '{to}'."
        );
        return result;
    }
    public static bool IsNotOutOfSqlDateRange(this DateTime value)
    {
        const long sqlMinDateTicks = 552877920000000000;
        const long sqlMaxDateTicks = 3155378975999970000;
        return value >= new DateTime(sqlMinDateTicks) && value <= new DateTime(sqlMaxDateTicks);
    }
    public static bool IsNotOutOfSqlDateRange(
        this DateTime value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotOutOfSqlDateRange();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "out of sql date range."
        );
        return result;
    }
    public static bool IsNotOutOfSqlDateRange(this DateTime? value)
    {
        return value.HasValue && value.Value.IsNotOutOfSqlDateRange();
    }
    public static bool IsNotOutOfSqlDateRange(
        this DateTime? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotOutOfSqlDateRange();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "out of sql date range."
        );
        return result;
    }
    public static bool IsOutOfSqlDateRange(this DateTime value)
    {
        const long sqlMinDateTicks = 552877920000000000;
        const long sqlMaxDateTicks = 3155378975999970000;
        return value < new DateTime(sqlMinDateTicks) || value > new DateTime(sqlMaxDateTicks);
    }
    public static bool IsOutOfSqlDateRange(
        this DateTime value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsOutOfSqlDateRange();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "out of sql date range."
        );
        return result;
    }
    public static bool IsOutOfSqlDateRange(this DateTime? value)
    {
        return !value.HasValue || value.Value.IsOutOfSqlDateRange();
    }
    public static bool IsOutOfSqlDateRange(
        this DateTime? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsOutOfSqlDateRange();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "out of sql date range."
        );
        return result;
    }
    public static bool AnyNull(params DateTime?[] values)
    {
        return values.IsNull() || values.Any(value => value.IsNull());
    }
    public static bool AnyNotNull(params DateTime?[] values)
    {
        return values.IsNotNull() || values.Any(value => value.IsNotNull());
    }
    public static bool AllNull(params DateTime?[] values)
    {
        return values.IsNull() || values.All(value => value.IsNull());
    }
    public static bool AllNotNull(params DateTime?[] values)
    {
        return values.IsNotNull() || values.All(value => value.IsNotNull());
    }
}
