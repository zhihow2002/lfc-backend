using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple.Builders;

namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

public static class StringValidation
{
    public static bool IsNotNullOrEmpty([NotNullWhen(true)] this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }

    public static bool IsNotNullOrEmpty(
        [NotNullWhen(true)] this string? value,
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

    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value)
    {
        return !value.IsNotNullOrEmpty();
    }

    public static bool IsNullOrEmpty(
        [NotNullWhen(false)] this string? value,
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

    public static bool IsNotNullOrWhiteSpace([NotNullWhen(true)] this string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    public static bool IsNotNullOrWhiteSpace(
        [NotNullWhen(true)] this string? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotNullOrWhiteSpace();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "null or white spaces."
        );

        return result;
    }

    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? value)
    {
        return !value.IsNotNullOrWhiteSpace();
    }

    public static bool IsNullOrWhiteSpace(
        [NotNullWhen(false)] this string? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNullOrWhiteSpace();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "null or white spaces."
        );

        return result;
    }

    public static bool HasLengthBetween(this string? value, int min, int max)
    {
        if (value.IsNullOrEmpty() && min == 0)
        {
            return true;
        }

        if (value.IsNullOrEmpty())
        {
            return false;
        }

        return value!.Length >= min && value.Length <= max;
    }

    public static bool HasLengthBetween(
        this string? value,
        int min,
        int max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.HasLengthBetween(min, max);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            "has length",
            "has length not",
            parameterName,
            $"between '{min}' and '{max}'."
        );

        return result;
    }

    public static bool HasLengthNotBetween(this string? value, int min, int max)
    {
        if (value.IsNullOrEmpty() && min == 0)
        {
            return false;
        }

        if (value.IsNullOrEmpty())
        {
            return true;
        }

        return value!.Length < min || value.Length > max;
    }

    public static bool HasLengthNotBetween(
        this string? value,
        int min,
        int max,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.HasLengthBetween(min, max);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            "has length not",
            "has length",
            parameterName,
            $"between '{min}' and '{max}'."
        );

        return result;
    }

    public static bool HasLengthLessThan(this string? value, int target)
    {
        if (value.IsNullOrEmpty())
        {
            return true;
        }

        return value!.Length < target;
    }

    public static bool HasLengthLessThan(
        this string? value,
        int target,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.HasLengthLessThan(target);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            "has length",
            "has length not",
            parameterName,
            $"less than '{target}'."
        );

        return result;
    }

    public static bool HasLengthNotLessThan(this string? value, int target)
    {
        if (value.IsNullOrEmpty())
        {
            return false;
        }

        return value!.Length > target;
    }

    public static bool HasLengthNotLessThan(
        this string? value,
        int target,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.HasLengthNotLessThan(target);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            "has length not",
            "has length",
            parameterName,
            $"less than '{target}'."
        );

        return result;
    }

    public static bool HasLengthMoreThan(this string? value, int target)
    {
        if (value.IsNullOrEmpty() && target == 0)
        {
            return true;
        }

        if (value.IsNullOrEmpty())
        {
            return false;
        }

        return value!.Length > target;
    }

    public static bool HasLengthMoreThan(
        this string? value,
        int target,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.HasLengthMoreThan(target);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            "has length",
            "has length not",
            parameterName,
            $"more than '{target}'."
        );

        return result;
    }

    public static bool HasLengthNotMoreThan(this string? value, int target)
    {
        if (value.IsNullOrEmpty() && target == 0)
        {
            return false;
        }

        if (value.IsNullOrEmpty())
        {
            return true;
        }

        return value!.Length < target;
    }

    public static bool HasLengthNotMoreThan(
        this string? value,
        int target,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.HasLengthNotMoreThan(target);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            "has length not",
            "has length",
            parameterName,
            $"more than '{target}'."
        );

        return result;
    }

    public static bool HasExactLengthOf(this string? value, int length)
    {
        return value.HasLengthBetween(length, length);
    }

    public static bool HasExactLengthOf(
        this string? value,
        int length,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.HasExactLengthOf(length);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            "has length",
            "has length not",
            parameterName,
            $"equal to '{length}'."
        );

        return result;
    }

    public static bool HasNoExactLengthOf(this string? value, int length)
    {
        return !value.HasLengthBetween(length, length);
    }

    public static bool HasNoExactLengthOf(
        this string? value,
        int length,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.HasNoExactLengthOf(length);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            "has length not",
            "has length",
            parameterName,
            $"equal to '{length}'."
        );

        return result;
    }

    /// <summary>
    ///     Check if the current value is a valid email address. It uses the following regular expression
    ///     ^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&
    ///     '\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$
    ///     null values will fail.
    ///     Empty strings will fail.
    ///     It performs the check from method
    ///     <see>
    ///         <cref>IsNotNullOrEmpty</cref>
    ///     </see>
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <returns>True if the value is a valid email address</returns>
    public static bool IsEmail(this string? value)
    {
        if (value.IsNullOrEmpty())
        {
            return false; // if it's null it cannot possibly be an email
        }

        const string exp =
            @"^([a-zA-Z0-9_\-\.&]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,5}|[0-9]{1,3})(\]?)$";
        return value.IsRegex(exp);
    }

    public static bool IsEmail(
        this string? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEmail();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "an email."
        );

        return result;
    }

    public static bool IsNotEmail(this string? value)
    {
        return !value.IsEmail();
    }

    public static bool IsNotEmail(
        this string? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotEmail();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "an email."
        );

        return result;
    }

    public static bool IsPhoneNumber(this string? value)
    {
        if (value.IsNullOrEmpty())
        {
            return false; // if it's null it cannot possibly be an email
        }

        string exp = @"^\+?[1-9][0-9]{9,14}$";

        return value.IsRegex(exp);
    }

    public static bool IsPhoneNumber(
        this string? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsPhoneNumber();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "a phone number."
        );

        return result;
    }

    public static bool IsNotPhoneNumber(this string? value)
    {
        return !value.IsPhoneNumber();
    }

    public static bool IsNotPhoneNumber(
        this string? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotPhoneNumber();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "a phone number."
        );

        return result;
    }

    public static bool IsWebAddress(this string? value)
    {
        if (value.IsNullOrEmpty())
        {
            return false;
        }

        string exp =
            @"^((http|ftp|https)://)?(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$";

        return value.IsRegex(exp);
    }

    public static bool IsWebAddress(
        this string? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsWebAddress();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "a web address."
        );

        return result;
    }

    public static bool IsNotWebAddress(this string? value)
    {
        return !value.IsWebAddress();
    }

    public static bool IsNotWebAddress(
        this string? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotWebAddress();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "a web address."
        );

        return result;
    }

    public static bool IsBase64String(this string? value)
    {
        if (value.IsNullOrEmpty())
        {
            return false;
        }

        return (value.Length % 4 == 0) && IsRegex(value, @"^[a-zA-Z0-9\+/]*={0,3}$");
    }

    public static bool IsBase64String(
        this string? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsBase64String();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "a base 64 string."
        );

        return result;
    }

    public static bool IsNotBase64String(this string? value)
    {
        return !value.IsBase64String();
    }

    public static bool IsNotBase64String(
        this string? value,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotBase64String();

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "a base 64 string."
        );

        return result;
    }

    /// <summary>
    ///     Validates if the specified object passes the regular expression provided. If the object is null, it will fail. The
    ///     method calls ToString on the object to get the string value.
    /// </summary>
    /// <param name="value">The value to be evaluated</param>
    /// <param name="exp">The regular expression</param>
    /// <returns></returns>
    public static bool IsRegex(this string? value, string exp)
    {
        if (value.IsNullOrEmpty())
        {
            return false;
        }

        return new Regex(exp, RegexOptions.IgnoreCase).IsMatch(value!);
    }

    public static bool IsRegex(
        this string? value,
        string exp,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsRegex(exp);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            "valid."
        );

        return result;
    }

    /// <summary>
    ///     Validates if the specified object passes the regular expression provided. If the object is null, it will fail. The
    ///     method calls ToString on the object to get the string value.
    /// </summary>
    /// <param name="value">The value to be evaluated</param>
    /// <param name="exp">The regular expression</param>
    /// <returns></returns>
    public static bool IsNotRegex(this string? value, string exp)
    {
        if (value.IsNullOrEmpty())
        {
            return true;
        }

        return !value.IsRegex(exp);
    }

    public static bool IsNotRegex(
        this string? value,
        string exp,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotRegex(exp);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            "valid."
        );

        return result;
    }

    public static bool IsEqualTo(
        this string? value,
        string compare,
        StringComparison comparison = StringComparison.Ordinal
    )
    {
        if (value.IsNullOrEmpty() && compare.IsNullOrEmpty())
        {
            return true;
        }

        if (value.IsNullOrEmpty() || compare.IsNullOrEmpty())
        {
            return false;
        }

        return string.Equals(value, compare, comparison);
    }

    public static bool IsEqualTo(
        this string? value,
        string compare,
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

    public static bool IsNotEqualTo(
        this string? value,
        string compare,
        StringComparison comparison = StringComparison.Ordinal
    )
    {
        if (value.IsNullOrEmpty() && compare.IsNullOrEmpty())
        {
            return false;
        }

        if (value.IsNullOrEmpty() || compare.IsNullOrEmpty())
        {
            return true;
        }

        return !string.Equals(value, compare, comparison);
    }

    public static bool IsNotEqualTo(
        this string? value,
        string compare,
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

    public static bool IsEndWith(
        this string? value,
        string compare,
        StringComparison comparison = StringComparison.Ordinal
    )
    {
        if (value.IsNullOrEmpty() && compare.IsNullOrEmpty())
        {
            return true;
        }

        if (value.IsNullOrEmpty() || compare.IsNullOrEmpty())
        {
            return false;
        }

        return value.EndsWith(compare, comparison);
    }

    public static bool IsEndWith(
        this string? value,
        string compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsEndWith(compare);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"end with '{compare}'."
        );

        return result;
    }

    public static bool IsNotEndWith(
        this string? value,
        string compare,
        StringComparison comparison = StringComparison.Ordinal
    )
    {
        if (value.IsNullOrEmpty() && compare.IsNullOrEmpty())
        {
            return false;
        }

        if (value.IsNullOrEmpty() || compare.IsNullOrEmpty())
        {
            return true;
        }

        return !value.EndsWith(compare, comparison);
    }

    public static bool IsNotEndWith(
        this string? value,
        string compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotEndWith(compare);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            $"end with '{compare}'."
        );

        return result;
    }

    public static bool IsStartWith(
        this string? value,
        string compare,
        StringComparison comparison = StringComparison.Ordinal
    )
    {
        if (value.IsNullOrEmpty() && compare.IsNullOrEmpty())
        {
            return true;
        }

        if (value.IsNullOrEmpty() || compare.IsNullOrEmpty())
        {
            return false;
        }

        return value.StartsWith(compare, comparison);
    }

    public static bool IsStartWith(
        this string? value,
        string compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsStartWith(compare);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.Is,
            parameterName,
            $"end with '{compare}'."
        );

        return result;
    }

    public static bool IsNotStartWith(
        this string? value,
        string compare,
        StringComparison comparison = StringComparison.Ordinal
    )
    {
        if (value.IsNullOrEmpty() && compare.IsNullOrEmpty())
        {
            return false;
        }

        if (value.IsNullOrEmpty() || compare.IsNullOrEmpty())
        {
            return true;
        }

        return !value.StartsWith(compare, comparison);
    }

    public static bool IsNotStartWith(
        this string? value,
        string compare,
        [NotNullWhen(true)] out string? validationMessage,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        bool result = value.IsNotStartWith(compare);

        validationMessage = ValidationMessageBuilder.GetValidationMessage(
            result,
            value,
            ValidationMessageBuilder.Condition.IsNot,
            parameterName,
            $"end with '{compare}'."
        );

        return result;
    }

    public static bool AnyNotNullOrWhiteSpace(params string?[] values)
    {
        return values.IsNotNull() && values.Any(value => value.IsNotNullOrWhiteSpace());
    }
}
