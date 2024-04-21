using System.Text.Json.Serialization;
using ClaimsPlugin.Shared.Foundation.Features.Serializer.Converters;

namespace ClaimsPlugin.Shared.Foundation.Features.Serializer;

public static class Startup
{
    public static IMvcBuilder AddJsonConverters(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder
            .AddJsonOptions(
                options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonByteArrayConverter())
            )
            .AddJsonOptions(
                options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            )
            .AddJsonOptions(
                options =>
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumMemberConverter()
                    )
            )
            .AddJsonOptions(
                options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonIPAddressConverter())
            )
            .AddJsonOptions(
                options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonIPEndPointConverter())
            )
            .AddJsonOptions(
                options =>
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStandardDateTimeConverter()
                    )
            )
            .AddJsonOptions(
                options =>
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStandardDateTimeOffsetConverter()
                    )
            )
            .AddJsonOptions(
                options =>
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonMicrosoftDateTimeConverter()
                    )
            )
            .AddJsonOptions(
                options =>
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonMicrosoftDateTimeOffsetConverter()
                    )
            )
            .AddJsonOptions(
                options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStandardGuidConverter())
            );
    }
}
