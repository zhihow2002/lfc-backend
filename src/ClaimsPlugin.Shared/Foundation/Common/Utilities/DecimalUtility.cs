using System.Text;
using Foundation.Features.DomainDrivenDesign.ValueObjects;

namespace ClaimsPlugin.Shared.Foundation.Common.Utilities;

public static class DecimalUtility
{
    public static Amount ToDecimalPlaces(this Amount value, int decimalPlaces, MidpointRounding rounding = MidpointRounding.AwayFromZero)
    {
        return Amount.Create(value.Value.ToDecimalPlaces(decimalPlaces, rounding));
    }
    
    public static decimal ToDecimalPlaces(this decimal value, int decimalPlaces, MidpointRounding rounding = MidpointRounding.AwayFromZero)
    {
        return Math.Round(value, decimalPlaces, rounding);
    }

    public static string ToDecimalPlacesAsString(this Amount value, int decimalPlaces, MidpointRounding rounding = MidpointRounding.AwayFromZero)
    {
        return value.Value.ToDecimalPlacesAsString(decimalPlaces, rounding);
    }
    
    public static string ToDecimalPlacesAsString(this decimal value, int decimalPlaces, MidpointRounding rounding = MidpointRounding.AwayFromZero)
    {
        StringBuilder builder = new();

        for (int i = 0; i < decimalPlaces; i++)
        {
            builder.Append('0');
        }

        return value.ToDecimalPlaces(decimalPlaces, rounding).ToString("{0:0." + builder + "}");
    }

    public static int GetDecimalPlaces(this decimal value)
    {
        int[] bits = decimal.GetBits(value);
        int scale = (bits[3] >> 16) & 0x000000FF;
        return scale;
    }
    
    public static int GetDecimalPlaces(this Amount value)
    {
        return value.Value.GetDecimalPlaces();
    }
}
