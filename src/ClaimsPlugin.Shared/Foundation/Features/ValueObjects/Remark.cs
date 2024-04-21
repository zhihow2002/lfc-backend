using Foundation.Common.Persistence.Models;
using Foundation.Features.ExceptionHandling.Exceptions;
using Foundation.Features.Validation.Simple;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class Remarks : BaseValueObject
{
    protected Remarks()
    {
    }

    private Remarks(string value)
    {
        Value = value;
    }

    public Remarks Duplicate()
    {
        return Create(Value);
    }

    public string Value { get; private set; } = default!;

    public static Remarks Create(string remarks)
    {
        if (remarks.IsNullOrWhiteSpace(out string? remarksNullOrWhiteSpaceErrorMessage))
        {
            throw new DomainException(remarksNullOrWhiteSpaceErrorMessage);
        }

        if (remarks.HasLengthMoreThan(500, out string? remarkMaximumLengthErrorMessage))
        {
            throw new DomainException(remarkMaximumLengthErrorMessage);
        }

        return new Remarks(remarks);
    }


    public static implicit operator string(Remarks remarks)
    {
        return remarks.ToString();
    }


    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
