using System.Runtime.CompilerServices;
using System.Text;
using ClaimsPlugin.Shared.Foundation.Common.Utilities;

namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Simple.Builders;

public static class ValidationMessageBuilder
{
    internal enum Condition
    {
        Is,
        IsNot
    }

    public static string? GetParameterName(
        this object? value,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        if (parameterName.IsNotNullOrWhiteSpace())
        {
            if (parameterName.EndsWith(".Value"))
            {
                parameterName = parameterName.Replace(".Value", string.Empty).ToPascalCase();
            }

            return parameterName;
        }

        return string.Empty;
    }

    public static string GetParameterNameAndValue(
        this object? value,
        [CallerArgumentExpression("value")] string? parameterName = default
    )
    {
        if (parameterName.IsNotNullOrWhiteSpace())
        {
            if (parameterName.EndsWith(".Value"))
            {
                parameterName = parameterName.Replace(".Value", string.Empty).ToPascalCase();
            }

            return $"{parameterName} '{value}' ";
        }

        return $"'{value}'";
    }

    internal static string GetValidationMessage(
        bool result,
        Condition condition,
        string? parameterName,
        string description
    )
    {
        StringBuilder builder = new();

        if (parameterName.IsNotNullOrWhiteSpace())
        {
            builder.Append(parameterName.GetParameterNameAndValue());
        }
        else
        {
            builder.Append("Value ");
        }

        if (condition == Condition.Is)
        {
            builder.Append(result ? "is " : "is not ");
        }
        else if (condition == Condition.IsNot)
        {
            builder.Append(result ? "is not " : "is ");
        }
        else
        {
            throw new InvalidOperationException("The provided condition is not valid. ");
        }

        return builder.Append(description).ToString();
    }

    internal static string GetValidationMessage<T>(
        bool result,
        T value,
        Condition condition,
        string? parameterName,
        string description
    )
    {
        StringBuilder builder = new();

        if (parameterName.IsNotNullOrWhiteSpace())
        {
            builder.Append(value.GetParameterNameAndValue(parameterName));
        }
        else
        {
            builder.Append($"Value '{value}' ");
        }

        if (condition == Condition.Is)
        {
            builder.Append(result ? "is " : "is not ");
        }
        else if (condition == Condition.IsNot)
        {
            builder.Append(result ? "is not " : "is ");
        }
        else
        {
            throw new InvalidOperationException("The provided condition is not valid. ");
        }

        return builder.Append(description).ToString();
    }

    internal static string GetValidationMessage(
        bool result,
        string validConditionDescription,
        string invalidConditionDescription,
        string? parameterName,
        string description
    )
    {
        StringBuilder builder = new();

        if (parameterName.IsNotNullOrWhiteSpace())
        {
            builder.Append(parameterName.GetParameterNameAndValue());
        }
        else
        {
            builder.Append("Value ");
        }

        builder.Append(
            result ? validConditionDescription + " " : invalidConditionDescription + " "
        );

        return builder.Append(description).ToString();
    }

    internal static string GetValidationMessage<T>(
        bool result,
        T value,
        string validConditionDescription,
        string invalidConditionDescription,
        string? parameterName,
        string description
    )
    {
        StringBuilder builder = new();

        if (parameterName.IsNotNullOrWhiteSpace())
        {
            builder.Append(parameterName.GetParameterNameAndValue());
        }
        else
        {
            builder.Append($"Value '{value}' ");
        }

        builder.Append(
            result ? validConditionDescription + " " : invalidConditionDescription + " "
        );

        return builder.Append(description).ToString();
    }
}
