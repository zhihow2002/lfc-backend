﻿using System.Globalization;
using Foundation.Common.Persistence.Models;
using Foundation.Features.ExceptionHandling.Exceptions;
using Foundation.Features.Validation.Simple;

namespace Foundation.Features.DomainDrivenDesign.ValueObjects;

public class QuantityWithDecimalType : BaseValueObject
{
    protected QuantityWithDecimalType()
    {
    }

    private QuantityWithDecimalType(decimal value)
    {
        Value = value;
    }

    public static QuantityWithDecimalType Default => new(0);
    public decimal Value { get; private set; }

    public static QuantityWithDecimalType Create(decimal quantity)
    {
        if (quantity.IsNegative(out string? quantityNegativeErrorMessage))
        {
            throw new DomainException(quantityNegativeErrorMessage);
        }

        return new QuantityWithDecimalType(quantity);
    }


    public static implicit operator decimal(QuantityWithDecimalType value)
    {
        return value.Value;
    }

    public static implicit operator string(QuantityWithDecimalType value)
    {
        return value.ToString();
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
