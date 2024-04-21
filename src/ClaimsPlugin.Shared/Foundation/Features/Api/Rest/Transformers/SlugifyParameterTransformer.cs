using System.Text.RegularExpressions;
using ClaimsPlugin.Shared.Foundation.Common.Utilities;
using Microsoft.AspNetCore.Routing;

namespace ClaimsPlugin.Shared.Foundation.Features.Api.Rest.Transformers;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        return value?.ToString()?.ToKebabCase();
    }
}
