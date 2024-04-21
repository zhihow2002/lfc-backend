using System.Linq.Expressions;
using System.Reflection;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    internal static ModelBuilder AppendGlobalQueryFilter<TInterface>(
        this ModelBuilder modelBuilder,
        Expression<Func<TInterface, bool>> filter
    )
    {
        IEnumerable<Type> entities = modelBuilder
            .Model.GetEntityTypes()
            .Where(
                e =>
                    e.BaseType is null
                    && e.ClrType.GetInterface(typeof(TInterface).Name) is not null
            )
            .Select(e => e.ClrType);
        foreach (var entity in entities)
        {
            ParameterExpression parameterType = Expression.Parameter(
                modelBuilder.Entity(entity).Metadata.ClrType
            );
            Expression filterBody = ReplacingExpressionVisitor.Replace(
                filter.Parameters.Single(),
                parameterType,
                filter.Body
            );
            if (modelBuilder.Entity(entity).Metadata.GetQueryFilter() is { } existingFilter)
            {
                Expression existingFilterBody = ReplacingExpressionVisitor.Replace(
                    existingFilter.Parameters.Single(),
                    parameterType,
                    existingFilter.Body
                );
                filterBody = Expression.AndAlso(existingFilterBody, filterBody);
            }
            modelBuilder
                .Entity(entity)
                .HasQueryFilter(Expression.Lambda(filterBody, parameterType));
        }
        return modelBuilder;
    }

    public static void AddSequences(this ModelBuilder modelBuilder, Type databaseContextType)
    {
        foreach (
            PropertyInfo type in databaseContextType
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.PropertyType == typeof(DbSequence))
        )
        {
            object? sequence = type.GetValue(BindingFlags.Public);
            object? property = sequence
                ?.GetType()
                .GetProperty(nameof(DbSequence.SequenceName))
                ?.GetValue(sequence, null);
            if (property is null)
            {
                throw new InvalidDataException(
                    $"Unable to get the value of '{nameof(DbSequence.SequenceName)}' when searching in '{type.Name}'."
                );
            }
            modelBuilder
                .HasSequence<int>(property.ToString()!, "Sequence")
                .StartsAt(1)
                .IncrementsBy(1);
        }
    }
}
