using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace ClaimsPlugin.Shared.Foundation.Features.Serializer.Converters;
public class JsonStandardDateTimeOffsetConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
#pragma warning disable CA1062
        return typeToConvert == typeof(DateTime) || (typeToConvert.IsGenericType && IsNullableDateTimeOffset(typeToConvert));
#pragma warning restore CA1062
    }
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
#pragma warning disable CA1062
        return typeToConvert.IsGenericType ? new JsonNullableDateTimeOffsetOffsetConverter() : new JsonNotNullDateTimeOffsetOffsetConverter();
#pragma warning restore CA1062
    }
    private static bool IsNullableDateTimeOffset(Type typeToConvert)
    {
        Type? underlyingType = Nullable.GetUnderlyingType(typeToConvert);
        return underlyingType != null && underlyingType == typeof(DateTime);
    }
    private class JsonNotNullDateTimeOffsetOffsetConverter : JsonCustomConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ReadDateTime(ref reader);
        }
        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            WriteDateTime(writer, value);
        }
    }
    private class JsonNullableDateTimeOffsetOffsetConverter : JsonCustomConverter<DateTimeOffset?>
    {
        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ReadDateTime(ref reader);
        }
        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            WriteDateTime(writer, value!.Value);
        }
    }
    private abstract class JsonCustomConverter<T> : JsonConverter<T>
    {
        protected static DateTimeOffset ReadDateTime(ref Utf8JsonReader reader)
        {
            if (reader.TokenType != JsonTokenType.String || !reader.TryGetDateTimeOffset(out DateTimeOffset value))
            {
                throw new JsonException($"The JSON value could not be converted to {typeof(T)}.");
            }
            return value;
        }
        protected static void WriteDateTime(Utf8JsonWriter writer, DateTimeOffset value)
        {
            writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
