namespace ClaimsPlugin.Shared.Foundation.Features.Api.Rest.OpenApi.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class SwaggerHeaderAttribute : Attribute
{
    public SwaggerHeaderAttribute(string headerName, string? description = null, string? defaultValue = null, bool isRequired = false)
    {
        HeaderName = headerName;
        Description = description;
        DefaultValue = defaultValue;
        IsRequired = isRequired;
    }

    public string HeaderName { get; }
    public string? Description { get; }
    public string? DefaultValue { get; }
    public bool IsRequired { get; }
}
