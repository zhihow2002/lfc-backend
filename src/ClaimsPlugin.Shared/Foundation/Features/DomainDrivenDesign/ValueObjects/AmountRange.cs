using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class AmountRange : BaseValueObject
{
    protected AmountRange() { }

    private AmountRange(Amount minimumAmount, Amount maximumAmount)
    {
        MinimumAmount = minimumAmount;
        MaximumAmount = maximumAmount;
    }

    public Amount MinimumAmount { get; private set; } = default!;
    public Amount MaximumAmount { get; private set; } = default!;

    public AmountRange Duplicate()
    {
        return Create(Amount.Create(MinimumAmount.Value), Amount.Create(MaximumAmount.Value));
    }

    public static AmountRange Create(Amount minimumAmount, Amount maximumAmount)
    {
        if (
            minimumAmount.Value.IsGreaterThan(
                maximumAmount.Value,
                out string? minimumAmountGreaterThanErrorMessage
            )
        )
        {
            throw new DomainException(minimumAmountGreaterThanErrorMessage);
        }

        return new AmountRange(minimumAmount, maximumAmount);
    }

    public static implicit operator string(AmountRange amountRange)
    {
        return amountRange.ToString();
    }

    public bool Contains(Amount amount)
    {
        return amount.Value.IsBetween(MinimumAmount, MaximumAmount);
    }

    public override string ToString()
    {
        return $"{MinimumAmount} ~ {MaximumAmount}";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MinimumAmount;
        yield return MaximumAmount;
    }
}
