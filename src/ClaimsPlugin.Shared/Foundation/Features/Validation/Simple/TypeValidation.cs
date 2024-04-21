using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple.Builders;
namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
public static class TypeValidation
{
    public static bool Is<T>(this object? value)
    {
        if (value == null)
        {
            return false;
        }
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        try
        {
            T? result = (T?)converter.ConvertFromString(value.ToString()!);
            return EqualityComparer<T>.Default.Equals(result, default);
        }
        catch (Exception)
        {
            return false;
        }
    }
    public static bool Is<T>(
        this object? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        if (value.IsNull(out validationMessage, parameterName))
        {
            return false;
        }
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        try
        {
            T? conversion = (T?)converter.ConvertFromString(value.ToString()!);
            bool result = !EqualityComparer<T>.Default.Equals(conversion, default);
            validationMessage = ValidationMessageBuilder.GetValidationMessage(
                result,
                value,
                ValidationMessageBuilder.Condition.Is,
                parameterName,
                "failed to convert the object"
            );
            return result;
        }
        catch (Exception ex)
        {
            validationMessage = ex.ToString();
            return false;
        }
    }
    public static bool IsInt(this object value)
    {
        return value.Is<int>();
    }
    public static bool IsInt(
        this object value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        return value.Is<int>(out validationMessage, parameterName);
    }
    public static bool IsShort(this object value)
    {
        return value.Is<short>();
    }
    public static bool IsShort(
        this object value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        return value.Is<short>(out validationMessage, parameterName);
    }
    public static bool IsLong(this object value)
    {
        return value.Is<long>();
    }
    public static bool IsLong(
        this object value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        return value.Is<long>(out validationMessage, parameterName);
    }
    public static bool IsDouble(this object value)
    {
        return value.Is<double>();
    }
    public static bool IsDouble(
        this object value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        return value.Is<double>(out validationMessage, parameterName);
    }
    public static bool IsDecimal(this object value)
    {
        return value.Is<decimal>();
    }
    public static bool IsDecimal(
        this object value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        return value.Is<decimal>(out validationMessage, parameterName);
    }
    public static bool IsBool(this object value)
    {
        return value.Is<bool>();
    }
    public static bool IsBool(
        this object value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        return value.Is<bool>(out validationMessage, parameterName);
    }
    public static bool IsNumber(this object value)
    {
        return
            value.IsLong() ||
            value.IsDouble() ||
            value.IsDecimal() ||
            value.IsDouble();
    }
    public static bool IsNumber(
        this object value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNumber();
        validationMessage = ValidationMessageBuilder.GetValidationMessage(result, value, ValidationMessageBuilder.Condition.Is, parameterName, "a number.");
        return result;
    }
}
