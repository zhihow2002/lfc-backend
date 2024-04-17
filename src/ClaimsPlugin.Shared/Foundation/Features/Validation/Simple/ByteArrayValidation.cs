using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple.Builders;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Simple
{
    public static class ByteArrayValidation
    {
        public static bool IsNotNullOrEmpty([NotNullWhen(true)] this byte[]? value)
        {
            return value is not null && value.Length > 0;
        }

        public static bool IsNotNullOrEmpty(
            [NotNullWhen(true)] this byte[]? value,
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

        public static bool IsNullOrEmpty([NotNullWhen(false)] this byte[]? value)
        {
            return !value.IsNotNullOrEmpty();
        }

        public static bool IsNullOrEmpty(
            [NotNullWhen(false)] this byte[]? value,
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

        public static bool IsNull([NotNullWhen(false)] this byte[]? value)
        {
            return !value.IsNotNull();
        }

        public static bool IsNull(
            [NotNullWhen(false)] this byte[]? value,
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

        public static bool IsNotNull([NotNullWhen(true)] this byte[]? value)
        {
            return value is not null;
        }

        public static bool IsNotNull(
            [NotNullWhen(true)] this byte[]? value,
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

        public static bool IsEmpty(this byte[]? value)
        {
            return value.IsNotNull() && value.Length <= 0;
        }

        public static bool IsEmpty(
            this byte[]? value,
            [NotNullWhen(true)] out string? validationMessage,
            [CallerArgumentExpression("value")] string? parameterName = default
        )
        {
            bool result = value.IsEmpty();

            validationMessage = ValidationMessageBuilder.GetValidationMessage(
                result,
                value,
                ValidationMessageBuilder.Condition.Is,
                parameterName,
                "empty."
            );

            return result;
        }

        public static bool IsNotEmpty(this byte[]? value)
        {
            return value.IsNotNull() && value.Length > 0;
        }

        public static bool IsNotEmpty(
            this byte[]? value,
            [NotNullWhen(true)] out string? validationMessage,
            [CallerArgumentExpression("value")] string? parameterName = default
        )
        {
            bool result = value.IsNotEmpty();

            validationMessage = ValidationMessageBuilder.GetValidationMessage(
                result,
                value,
                ValidationMessageBuilder.Condition.IsNot,
                parameterName,
                "empty."
            );

            return result;
        }
    }
}
