using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClaimsPlugin.Shared.Foundation.Features.Serializer.Converters;

public class JsonByteArrayConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
#pragma warning disable CA1062
        return typeToConvert == typeof(byte[])
            || (typeToConvert.IsGenericType && IsNullableByteArray(typeToConvert));
#pragma warning restore CA1062
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
#pragma warning disable CA1062
        return typeToConvert.IsGenericType
            ? new JsonNullableByteArrayConverter()
            : new JsonNotNullByteArrayConverter();
#pragma warning restore CA1062
    }

    private static bool IsNullableByteArray(Type typeToConvert)
    {
        Type? underlyingType = Nullable.GetUnderlyingType(typeToConvert);

        return underlyingType != null && underlyingType == typeof(byte[]);
    }

    private class JsonNotNullByteArrayConverter : JsonCustomConverter<byte[]>
    {
        public override byte[] Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            return ReadByteArray(ref reader);
        }

        public override void Write(
            Utf8JsonWriter writer,
            byte[] value,
            JsonSerializerOptions options
        )
        {
            WriteByteArray(writer, value);
        }
    }

    private class JsonNullableByteArrayConverter : JsonCustomConverter<byte[]?>
    {
        public override byte[]? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            return ReadByteArray(ref reader);
        }

        public override void Write(
            Utf8JsonWriter writer,
            byte[]? value,
            JsonSerializerOptions options
        )
        {
            WriteByteArray(writer, value!);
        }
    }

    private abstract class JsonCustomConverter<T> : JsonConverter<T>
    {
        protected static byte[] ReadByteArray(ref Utf8JsonReader reader)
        {
            if (
                reader.TokenType != JsonTokenType.String
                || !reader.TryGetBytesFromBase64(out byte[]? value)
            )
            {
                return Array.Empty<byte>();
            }

            return value;
        }

        protected static void WriteByteArray(Utf8JsonWriter writer, byte[] value)
        {
            writer.WriteBase64StringValue(value);
        }
    }
}
