//using Foundation.Common.Persistence.Models;
//using Foundation.Features.ExceptionHandling.Exceptions;
//using Foundation.Features.Validation.Simple;

//namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

//public class CostCentre : BaseValueObject
//{
//    protected CostCentre()
//    {
//    }

//    private CostCentre(string name, string number)
//    {
//        Name = name;
//        Number = number;
//    }

//    public string Name { get; private set; } = default!;
//    public string Number { get; private set; } = default!;

//    public static CostCentre Create(string name, string number)
//    {
//        if (name.IsNullOrWhiteSpace(out string? nameNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(nameNullOrWhiteSpaceErrorMessage);
//        }

//        if (name.HasLengthMoreThan(50, out string? nameLengthErrorMessage))
//        {
//            throw new DomainException(nameLengthErrorMessage);
//        }

//        if (number.IsNullOrWhiteSpace(out string? numberNullOrWhiteSpaceErrorMessage))
//        {
//            throw new DomainException(numberNullOrWhiteSpaceErrorMessage);
//        }

//        if (number.HasLengthMoreThan(50, out string? numberLengthErrorMessage))
//        {
//            throw new DomainException(numberLengthErrorMessage);
//        }

//        return new CostCentre(name, number);
//    }

//    public static implicit operator string(CostCentre costCentre)
//    {
//        return costCentre.ToString();
//    }

//    public override string ToString()
//    {
//        return $"{Number} - {Name}";
//    }

//    protected override IEnumerable<object?> GetEqualityComponents()
//    {
//        yield return Name;
//        yield return Number;
//    }
//}
