using System.Reflection;
using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.OpenApi.Attributes;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace ClaimsPlugin.Shared.Foundation.Features.Api.Rest.OpenApi.Processors;

public class SwaggerHeaderAttributeProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        IEnumerable<Attribute>? attributes = context.MethodInfo?.GetCustomAttributes(
            typeof(SwaggerHeaderAttribute)
        );

        if (attributes.IsNotNull())
        {
            foreach (Attribute customAttribute in attributes)
            {
                if (customAttribute is SwaggerHeaderAttribute attribute)
                {
                    IList<OpenApiParameter>? parameters = context
                        .OperationDescription
                        .Operation
                        .Parameters;

                    OpenApiParameter? existingParam = parameters.FirstOrDefault(
                        p => p.Kind == OpenApiParameterKind.Header && p.Name == attribute.HeaderName
                    );
                    if (existingParam is not null)
                    {
                        parameters.Remove(existingParam);
                    }

                    parameters.Add(
                        new OpenApiParameter
                        {
                            Name = attribute.HeaderName,
                            Kind = OpenApiParameterKind.Header,
                            Description = attribute.Description,
                            IsRequired = attribute.IsRequired,
                            Schema = new JsonSchema
                            {
                                Type = JsonObjectType.String,
                                Default = attribute.DefaultValue
                            }
                        }
                    );
                }
            }
        }

        return true;
    }
}
