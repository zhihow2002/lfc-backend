using ClaimsPlugin.Shared.Foundation.Features.DependencyInjection.Interfaces;

namespace ClaimsPlugin.Shared.Foundation.Features.Serializer.Interfaces;

public interface ISerializerService : ITransientService
{
    string Serialize<T>(T obj, bool forceCamelCase = true, bool escapeNonAsciiCharacters = true);

    string Serialize<T>(
        T obj,
        Type type,
        bool forceCamelCase = true,
        bool escapeNonAsciiCharacters = true
    );

    T? Deserialize<T>(
        string text,
        bool forceCamelCase = true,
        bool escapeNonAsciiCharacters = true
    );
}
