using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace ClaimsPlugin.Shared.Foundation.Features.Serializer.Converters;
public class JsonStandardDateTimeConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
#pragma warning disable CA1062
        return typeToConvert == typeof(DateTime) || (typeToConvert.IsGenericType && IsNullableDateTime(typeToConvert));
#pragma warning restore CA1062
    }
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
#pragma warning disable CA1062
        return typeToConvert.IsGenericType ? new JsonNullableDateTimeConverter() : new JsonNotNullDateTimeConverter();
#pragma warning restore CA1062
    }
    private static bool IsNullableDateTime(Type typeToConvert)
    {
        Type? underlyingType = Nullable.GetUnderlyingType(typeToConvert);
        return underlyingType != null && underlyingType == typeof(DateTime);
    }
    private class JsonNotNullDateTimeConverter : JsonCustomConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ReadDateTime(ref reader);
        }
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            WriteDateTime(writer, value);
        }
    }
    private class JsonNullableDateTimeConverter : JsonCustomConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ReadDateTime(ref reader);
        }
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            WriteDateTime(writer, value!.Value);
        }
    }
    private abstract class JsonCustomConverter<T> : JsonConverter<T>
    {
        protected static DateTime ReadDateTime(ref Utf8JsonReader reader)
        {
            if (reader.TokenType != JsonTokenType.String || !reader.TryGetDateTime(out DateTime value))
            {
                throw new JsonException($"The JSON value could not be converted to {typeof(T)}.");
            }
            return value;
        }
        protected static void WriteDateTime(Utf8JsonWriter writer, DateTime value)
        {
            writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
