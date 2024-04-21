using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClaimsPlugin.Shared.Foundation.Features.Serializer.Converters;

public class JsonStandardGuidConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
#pragma warning disable CA1062
        return typeToConvert == typeof(Guid)
            || (typeToConvert.IsGenericType && IsNullableGuid(typeToConvert));
#pragma warning restore CA1062
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
#pragma warning disable CA1062
        return typeToConvert.IsGenericType
            ? new JsonNullableGuidConverter()
            : new JsonNotNullGuidConverter();
#pragma warning restore CA1062
    }

    private static bool IsNullableGuid(Type typeToConvert)
    {
        Type? underlyingType = Nullable.GetUnderlyingType(typeToConvert);
        return underlyingType != null && underlyingType == typeof(Guid);
    }

    private class JsonNotNullGuidConverter : JsonCustomConverter<Guid>
    {
        public override Guid Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            return ReadGuid(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
        {
            WriteGuid(writer, value);
        }
    }

    private class JsonNullableGuidConverter : JsonCustomConverter<Guid?>
    {
        public override Guid? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            return ReadGuid(ref reader);
        }

        public override void Write(
            Utf8JsonWriter writer,
            Guid? value,
            JsonSerializerOptions options
        )
        {
            WriteGuid(writer, value!.Value);
        }
    }

    private abstract class JsonCustomConverter<T> : JsonConverter<T>
    {
        protected static Guid ReadGuid(ref Utf8JsonReader reader)
        {
            if (reader.TokenType != JsonTokenType.String || !reader.TryGetGuid(out Guid value))
            {
                return Guid.Empty;
            }
            return value;
        }

        protected static void WriteGuid(Utf8JsonWriter writer, Guid value)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
