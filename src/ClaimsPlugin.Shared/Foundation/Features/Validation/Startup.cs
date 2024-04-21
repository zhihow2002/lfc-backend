using FluentValidation.AspNetCore;

namespace ClaimsPlugin.Shared.Foundation.Features.Validation;

internal static class Startup
{
    internal static IServiceCollection AddValidation(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
    }
}
