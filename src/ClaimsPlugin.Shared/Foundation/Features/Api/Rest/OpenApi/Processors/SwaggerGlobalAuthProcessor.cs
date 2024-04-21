using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using OpenIddict.Validation.AspNetCore;

namespace ClaimsPlugin.Shared.Foundation.Features.Api.Rest.OpenApi.Processors;

/// <summary>
///     The default NSwag AspNetCoreOperationProcessor doesn't take .RequireAuthorization() calls into account
///     Unless the AllowAnonymous attribute is defined, this processor will always add the security scheme
///     when it's not already there, so effectively adding "Global Auth".
/// </summary>
public class SwaggerGlobalAuthProcessor : IOperationProcessor
{
    private readonly string _name;

    public SwaggerGlobalAuthProcessor()
        : this(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
    {
    }

    public SwaggerGlobalAuthProcessor(string name)
    {
        _name = name;
    }

    public bool Process(OperationProcessorContext context)
    {
        IList<object>? list =
            ((AspNetCoreOperationProcessorContext)context).ApiDescription?.ActionDescriptor.TryGetPropertyValue<IList<object>>("EndpointMetadata");

        if (list is not null)
        {
            if (list.OfType<AllowAnonymousAttribute>().Any())
            {
                return true;
            }

            if (context.OperationDescription.Operation.Security is null ||
                !context.OperationDescription.Operation.Security.Any())
            {
                if (context.OperationDescription.Operation.Security is null)
                {
                    context.OperationDescription.Operation.Security = new List<OpenApiSecurityRequirement>();
                }

                context.OperationDescription.Operation.Security.Add(
                    new OpenApiSecurityRequirement { { _name, Array.Empty<string>() } }
                );
            }
        }

        return true;
    }
}
