using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;

namespace ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.ValueObjects;

public class AdminFeeChargeType : BaseValueObject
{
    protected AdminFeeChargeType() { }

    private AdminFeeChargeType(string value)
    {
        Value = value;
    }

    public static AdminFeeChargeType Claim => new("Per Claim");
    public static AdminFeeChargeType Member => new("Per Member");
    public static AdminFeeChargeType Employee => new("Per Employee");
    public static AdminFeeChargeType FixedRate => new("Fixed Rate");

    public string Value { get; private set; } = default!;

    public static IEnumerable<AdminFeeChargeType> SupportedItems
    {
        get
        {
            yield return Claim;
            yield return Member;
            yield return Employee;
            yield return FixedRate;
        }
    }

    public static AdminFeeChargeType From(string value)
    {
        AdminFeeChargeType item = new(value);

        if (!SupportedItems.Contains(item))
        {
            throw new DomainException($"Unsupported {nameof(AdminFeeChargeType)}: {value}");
        }

        return item;
    }

    public static implicit operator string(AdminFeeChargeType value)
    {
        return value.ToString();
    }

    public static explicit operator AdminFeeChargeType(string value)
    {
        return From(value);
    }

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
