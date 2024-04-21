using System.Data.SqlTypes;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Helpers;

public static class EntityPropertyBuilder
{
    public static PropertyBuilder HasColumnTypeAsVarcharWithMaxLength(this PropertyBuilder propertyBuilder)
    {
        return propertyBuilder.HasColumnType("VARCHAR(MAX)");
    }

    public static PropertyBuilder HasColumnTypeAsVarchar(this PropertyBuilder propertyBuilder)
    {
        return propertyBuilder.HasColumnType("VARCHAR");
    }

    public static PropertyBuilder HasColumnTypeAsNvarcharWithMaxLength(this PropertyBuilder propertyBuilder)
    {
        return propertyBuilder.HasColumnType("NVARCHAR(MAX)");
    }

    public static PropertyBuilder HasColumnTypeAsNvarchar(this PropertyBuilder propertyBuilder)
    {
        return propertyBuilder.HasColumnType("NVARCHAR");
    }

    public static PropertyBuilder HasColumnTypeAsVarbinaryWithMaxLength(this PropertyBuilder propertyBuilder)
    {
        return propertyBuilder.HasColumnType("VARBINARY(MAX)");
    }

    public static PropertyBuilder HasColumnTypeAsVarbinary(this PropertyBuilder propertyBuilder)
    {
        return propertyBuilder.HasColumnType("VARBINARY");
    }

    public static PropertyBuilder HasSequentialNumber(
        this PropertyBuilder propertyBuilder,
        string sequenceName,
        string prefix = "",
        char? characterToFillRemainingLength = null,
        int? maxLength = null
    )
    {
        if (propertyBuilder.Metadata.ClrType != typeof(string) && propertyBuilder.Metadata.ClrType != typeof(int))
        {
            throw new SqlTypeException($"Unsupported type '{propertyBuilder.Metadata.ClrType}' for sequential number.");
        }

        StringBuilder builder = new();

        if (maxLength.HasValue)
        {
            characterToFillRemainingLength ??= '0';

            builder.Append(
                $"CONCAT('{prefix}',RIGHT(CONCAT('{new string(characterToFillRemainingLength.Value, maxLength.Value - prefix.Length - 1)}', NEXT VALUE FOR Sequence.{sequenceName}), {maxLength.Value - prefix.Length}))"
            );

            return propertyBuilder.HasMaxLength(maxLength.Value).HasDefaultValueSql(builder.ToString());
        }

        return propertyBuilder.HasDefaultValueSql(
            !string.IsNullOrWhiteSpace(prefix) ? $"CONCAT('{prefix}', NEXT VALUE FOR Sequence.{sequenceName})" : $"NEXT VALUE FOR Sequence.{sequenceName}"
        );
    }

    public static PropertyBuilder HasSequentialNumber(
        this PropertyBuilder propertyBuilder,
        string sequenceName,
        string prefix = "",
        char? characterToFillRemainingLength = null,
        int? maxLength = null,
        int? maxPrefixLength = null
    )
    {
        if (propertyBuilder.Metadata.ClrType != typeof(string) && propertyBuilder.Metadata.ClrType != typeof(int))
        {
            throw new SqlTypeException($"Unsupported type '{propertyBuilder.Metadata.ClrType}' for sequential number.");
        }

        StringBuilder builder = new();

        if (maxLength.HasValue)
        {
            maxPrefixLength ??= prefix.Length;
            characterToFillRemainingLength ??= '0';

            int maxSequenceLength = maxLength.Value - maxPrefixLength.Value;

            builder.Append(
                $"CONCAT('{prefix}',RIGHT(CONCAT('{new string(characterToFillRemainingLength.Value, maxSequenceLength)}', NEXT VALUE FOR Sequence.{sequenceName}), {maxSequenceLength}))"
            );

            return propertyBuilder.HasMaxLength(maxLength.Value).HasDefaultValueSql(builder.ToString());
        }

        return propertyBuilder.HasDefaultValueSql(
            !string.IsNullOrWhiteSpace(prefix) ? $"CONCAT('{prefix}', NEXT VALUE FOR Sequence.{sequenceName})" : $"NEXT VALUE FOR Sequence.{sequenceName}"
        );
    }
}
