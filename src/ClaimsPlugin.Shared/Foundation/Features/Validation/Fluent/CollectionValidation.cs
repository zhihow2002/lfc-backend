using System.Linq.Expressions;
using FluentValidation;

namespace ClaimsPlugin.Shared.Foundation.Features.Validation.Fluent.Models;

public static class CollectionValidation
{
    public static IRuleBuilderOptions<T, IList<TElement>?> NoDuplicatesWith<T, TElement, TResult>(
        this IRuleBuilder<T, IList<TElement>?> ruleBuilder,
        Expression<Func<TElement, TResult>> keySelector
    )
    {
        return ruleBuilder.Must(
                (rootObject, list, context) =>
                {
                    if (list is null)
                    {
                        return true;
                    }

                    if (keySelector.Body is not NewExpression newExp)
                    {
                        if (keySelector.Body is not MemberExpression memberExp)
                        {
                            context.MessageFormatter
                                .AppendArgument("Property", context.DisplayName)
                                .AppendArgument("Keys", keySelector.Name);
                        }
                        else
                        {
                            context.MessageFormatter
                                .AppendArgument("Property", context.DisplayName)
                                .AppendArgument("Keys", memberExp.Member.Name);
                        }
                    }
                    else
                    {
                        List<string> props = new(newExp.Arguments.Count);

                        foreach (Expression exp in newExp.Arguments)
                        {
                            if (exp is not MemberExpression memberExp)
                            {
                                throw new ArgumentException();
                            }

                            props.Add(memberExp.Member.Name);
                        }

                        context.MessageFormatter
                            .AppendArgument("Property", context.DisplayName)
                            .AppendArgument("Keys", string.Join(", ", props));
                    }

                    IEnumerable<TElement> distinct = list.DistinctBy(keySelector.Compile());

                    return list.Except(distinct).ToList().Count == 0;
                }
            )
            .WithMessage("'{Property}' must not have duplicate records with {Keys}.");
    }
}
