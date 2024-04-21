using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;
public class AgeRange : BaseValueObject
{
    protected AgeRange() { }
    private AgeRange(Age minimumAge, Age maximumAge)
    {
        MinimumAge = minimumAge;
        MaximumAge = maximumAge;
    }
    public Age MinimumAge { get; private set; } = default!;
    public Age MaximumAge { get; private set; } = default!;
    public static AgeRange Create(Age minimumAge, Age maximumAge)
    {
        int min = minimumAge;
        int max = maximumAge;
        if (min.IsGreaterThan(max, out string? minimumAgeGreaterThanMaximumAgeErrorMessage))
        {
            throw new DomainException(minimumAgeGreaterThanMaximumAgeErrorMessage);
        }
        return new AgeRange(minimumAge, maximumAge);
    }
    public static implicit operator string(AgeRange ageRange)
    {
        return ageRange.ToString();
    }
    public bool Contains(Age age)
    {
        return age.TotalDays.IsBetween(MinimumAge, MaximumAge);
    }
    public override string ToString()
    {
        return $"{MinimumAge} to {MaximumAge}";
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MinimumAge;
        yield return MaximumAge;
    }
}
