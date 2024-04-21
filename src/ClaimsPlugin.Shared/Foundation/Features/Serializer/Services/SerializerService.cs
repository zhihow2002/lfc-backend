using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClaimsPlugin.Shared.Foundation.Features.Serializer.Interfaces;
namespace ClaimsPlugin.Shared.Foundation.Features.Serializer.Services;
public class SerializerService : ISerializerService
{
    public string Serialize<T>(
        T obj,
        bool forceCamelCase = true,
        bool escapeNonAsciiCharacters = true
    )
    {
        return JsonSerializer.Serialize(obj, GetOptions(forceCamelCase, escapeNonAsciiCharacters));
    }
    public string Serialize<T>(
        T obj,
        Type type,
        bool forceCamelCase = true,
        bool escapeNonAsciiCharacters = true
    )
    {
        return JsonSerializer.Serialize(
            obj,
            type,
            GetOptions(forceCamelCase, escapeNonAsciiCharacters)
        );
    }
    public T? Deserialize<T>(
        string text,
        bool forceCamelCase = true,
        bool escapeNonAsciiCharacters = true
    )
    {
        return JsonSerializer.Deserialize<T>(
            text,
            options: GetOptions(forceCamelCase, escapeNonAsciiCharacters)
        );
    }
    private JsonSerializerOptions GetOptions(bool forceCamelCase, bool escapeNonAsciiCharacters)
    {
        return new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = forceCamelCase ? JsonNamingPolicy.CamelCase : null,
            DictionaryKeyPolicy = forceCamelCase ? JsonNamingPolicy.CamelCase : null,
            Encoder = escapeNonAsciiCharacters
                ? JavaScriptEncoder.Default
                : JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
    }
}
