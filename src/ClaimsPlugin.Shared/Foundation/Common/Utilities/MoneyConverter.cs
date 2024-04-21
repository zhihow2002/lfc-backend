using FileHelpers;

namespace ClaimsPlugin.Shared.Foundation.Common.Utilities;

public class MoneyConverter : ConverterBase
{
    public override object StringToField(string from)
    {
        return Convert.ToDecimal(decimal.Parse(from) / 100);
    }

    public override string FieldToString(object fieldValue)
    {
        return ((decimal) fieldValue).ToString("#.##").Replace(".", "");
    }
}
