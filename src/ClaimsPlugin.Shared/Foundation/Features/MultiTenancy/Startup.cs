using Foundation.Common.Persistence;
using Foundation.Features.MultiTenancy.Configurations;
using Foundation.Features.MultiTenancy.Interfaces;
using Foundation.Features.MultiTenancy.Middleware;
using Foundation.Features.MultiTenancy.Models;
using Foundation.Features.MultiTenancy.Persistence;
using Foundation.Features.MultiTenancy.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Identity.Client;

namespace Foundation.Features.MultiTenancy;

public static class Startup

{
    internal static IServiceCollection AddMultiTenancy(this IServiceCollection services, IConfiguration configuration)
    {
        MultiTenancyDatabaseSettings? multiTenancyDatabaseSettings =
            configuration.GetSection(nameof(MultiTenancyDatabaseSettings)).Get<MultiTenancyDatabaseSettings>();
        
        string? rootConnectionString = multiTenancyDatabaseSettings?.GetConnectionString();
        
        if (string.IsNullOrEmpty(rootConnectionString))
        {
            throw new InvalidOperationException("MultiTenancy database connection string is not configured.");
        }

        return services
            .AddDbContext<TenantDatabaseContext>(m => m.UseDatabase(rootConnectionString, typeof(Startup).Assembly.GetName().Name!))
            .AddScoped<CurrentTenantMiddleware>()
            .AddScoped<ICurrentTenant, CurrentTenant>()
            .AddScoped(sp => (ICurrentTenantInitializer)sp.GetRequiredService<ICurrentTenant>());
    }

    internal static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseMiddleware<CurrentTenantMiddleware>();
        
        if (configuration.GetValue<bool>("DatabaseSettings:IsMultiTenancy"))
        {
            app.Use(async (context, next) =>
                {
                    if (!context.Response.HasStarted)
                    {
                        using IServiceScope scope = app.ApplicationServices.CreateScope();

                        ICurrentTenant currentTenant = scope.ServiceProvider.GetRequiredService<ICurrentTenant>();

                        context.Response.Headers.Add(TenantConfiguration.TenantIdFieldName, currentTenant.GetTenantId());
                    }

                    await next();
                }
            );
        }

        return app;
    }
}
