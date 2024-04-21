using System.Text;
using Foundation.Common.Persistence.Models;
using Foundation.Features.ExceptionHandling.Exceptions;
using Foundation.Features.Validation.Simple;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class Address : BaseValueObject
{
    protected Address()
    {
    }

    private Address(string blockNumber, string streetName, string? buildingName, string? unitName, string postalCode, string? city, string country)
    {
        BlockNumber = blockNumber;
        StreetName = streetName;
        BuildingName = buildingName;
        UnitName = unitName;
        PostalCode = postalCode;
        City = city;
        Country = country;
    }

    public string? BlockNumber { get; private set; }
    public string? StreetName { get; private set; }
    public string? BuildingName { get; private set; }
    public string? UnitName { get; private set; }
    public string PostalCode { get; private set; } = default!;
    public string? City { get; private set; }
    public string Country { get; private set; } = default!;

    public static Address Create(
        string addressBlockNumber,
        string addressStreetName,
        string? addressBuildingName,
        string? addressUnitName,
        string postalCode,
        string? city,
        string country
    )
    {
        if (addressBlockNumber.IsNotNullOrWhiteSpace() && addressBlockNumber.HasLengthMoreThan(35, out string? addressBlockNumberMaximumLengthErrorMessage))
        {
            throw new DomainException(addressBlockNumberMaximumLengthErrorMessage);
        }

        if (addressStreetName.IsNotNullOrWhiteSpace() && addressStreetName.HasLengthMoreThan(120, out string? addressStreetNameMaximumLengthErrorMessage))
        {
            throw new DomainException(addressStreetNameMaximumLengthErrorMessage);
        }

        if (addressBuildingName.IsNotNullOrWhiteSpace() && addressBuildingName.HasLengthMoreThan(120, out string? addressBuildingNameMaximumLengthErrorMessage))
        {
            throw new DomainException(addressBuildingNameMaximumLengthErrorMessage);
        }

        if (addressUnitName.IsNotNullOrWhiteSpace() && addressUnitName.HasLengthMoreThan(35, out string? addressUnitNameMaximumLengthErrorMessage))
        {
            throw new DomainException(addressUnitNameMaximumLengthErrorMessage);
        }

        if (postalCode.IsNullOrWhiteSpace(out string? postalCodeNullOrWhiteSpaceErrorMessage))
        {
            throw new DomainException(postalCodeNullOrWhiteSpaceErrorMessage);
        }

        if (postalCode.HasLengthMoreThan(35, out string? postalCodeMaximumLengthErrorMessage))
        {
            throw new DomainException(postalCodeMaximumLengthErrorMessage);
        }

        if (city.IsNotNullOrWhiteSpace() && city.HasLengthMoreThan(35, out string? cityMaximumLengthErrorMessage))
        {
            throw new DomainException(cityMaximumLengthErrorMessage);
        }

        if (country.IsNotNullOrWhiteSpace() && country.HasLengthMoreThan(30, out string? countryMaximumLengthErrorMessage))
        {
            throw new DomainException(countryMaximumLengthErrorMessage);
        }

        return new Address(addressBlockNumber, addressStreetName, addressBuildingName, addressUnitName, postalCode, city, country);
    }

    public static implicit operator string(Address address)
    {
        return address.ToString();
    }

    public override string ToString()
    {
        StringBuilder builder = new();

        if (BlockNumber.IsNotNullOrWhiteSpace())
        {
            builder.Append(BlockNumber + ", ");
        }

        if (BuildingName.IsNotNullOrWhiteSpace())
        {
            builder.Append(BuildingName + ", ");
        }

        if (UnitName.IsNotNullOrWhiteSpace())
        {
            builder.Append(UnitName + ", ");
        }

        if (StreetName.IsNotNullOrWhiteSpace())
        {
            builder.Append(StreetName + ", ");
        }

        if (PostalCode.IsNotNullOrWhiteSpace())
        {
            builder.Append(PostalCode + ", ");
        }

        if (City.IsNotNullOrWhiteSpace())
        {
            builder.Append(City + ", ");
        }

        if (Country.IsNotNullOrWhiteSpace())
        {
            builder.Append(Country + ", ");
        }

        return builder.ToString().Remove(builder.Length - 2).TrimEnd();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return BlockNumber;
        yield return StreetName;
        yield return BuildingName;
        yield return UnitName;
        yield return PostalCode;
        yield return City;
        yield return Country;
    }
}
