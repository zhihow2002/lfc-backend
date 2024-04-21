using Foundation.Common.Persistence.Models;
using Foundation.Features.ExceptionHandling.Exceptions;
using Foundation.Features.Validation.Simple;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class PersonName : BaseValueObject
{
    protected PersonName()
    {
    }

    private PersonName(string value)
    {
        Value = value;
    }

    public string Value { get; private set; } = default!;

    public static PersonName Create(string name)
    {
        if (name.IsNullOrWhiteSpace(out string? fullNameNullOrWhiteSpaceErrorMessage))
        {
            throw new DomainException(fullNameNullOrWhiteSpaceErrorMessage);
        }

        if (name.HasLengthMoreThan(255, out string? fullNameLengthErrorMessage))
        {
            throw new DomainException(fullNameLengthErrorMessage);
        }

        return new PersonName(name);
    }

    public static implicit operator string(PersonName personName)
    {
        return personName.ToString();
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
