using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class PositiveNumberRange : BaseValueObject
{
    protected PositiveNumberRange() { }

    private PositiveNumberRange(int minimumNumber, int maximumNumber)
    {
        MinimumNumber = minimumNumber;
        MaximumNumber = maximumNumber;
    }

    public int MinimumNumber { get; private set; } = default!;
    public int MaximumNumber { get; private set; } = default!;

    public static PositiveNumberRange Create(int minimumNumber, int maximumNumber)
    {
        int min = minimumNumber;
        int max = maximumNumber;

        if (min.IsGreaterThan(max, out string? minimumGreaterThanMaximumErrorMessage))
        {
            throw new DomainException(minimumGreaterThanMaximumErrorMessage);
        }

        return new PositiveNumberRange(minimumNumber, maximumNumber);
    }

    public static implicit operator string(PositiveNumberRange positiveNumberRange)
    {
        return positiveNumberRange.ToString();
    }

    public override string ToString()
    {
        return $"{MinimumNumber} to {MaximumNumber}";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MinimumNumber;
        yield return MaximumNumber;
    }
}
