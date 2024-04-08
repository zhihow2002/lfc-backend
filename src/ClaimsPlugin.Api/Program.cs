using System;
using ClaimsPlugin.Api.Configurations;
using ClaimsPlugin.Application;
using ClaimsPlugin.Shared;
using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.Conventions;
using ClaimsPlugin.Shared.Foundation.Features.Hosting;
using ClaimsPlugin.Shared.Foundation.Features.Logging;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

[assembly: ApiConventionType(typeof(ApiConvention))]

namespace ClaimsPlugin.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            StaticLogger.EnsureInitialized();
            Log.Information("Claims Plugin API Microservice Booting Up...");
            try
            {
                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

                // Configure Serilog
                builder.Host.UseSerilog(
                    (_, configuration) =>
                        configuration.ReadFrom.Configuration(builder.Configuration)
                );

                // Add configurations specific to this application
                builder.AddConfigurations(builder.Environment);

                // Configure Kestrel
                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ConfigureEndpoints(
                        "ClaimsPlugin",
                        builder.Environment,
                        builder.Configuration
                    );
                });

                // Register MVC / API Controllers
                builder.Services.AddControllers(); // Add services for MVC controllers

                // Add application services
                // builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
                builder.Services.AddApplication(builder.Configuration);

                // Configure the HTTP request pipeline
                WebApplication app = builder.Build();

                // Use routing, necessary for endpoint mapping
                app.UseRouting();

                // Map controller endpoints
                app.MapControllers(); // This maps routes to your controllers

                // Additional middleware configurations
                // app.UseInfrastructure(builder.Configuration, builder.Environment);

                // Start the application
                app.Run();
            }
            catch (Exception ex) when (ex.GetType() != typeof(HostAbortedException))
            {
                Log.Fatal(ex, "Unhandled exception during startup.");
            }
            finally
            {
                Log.Information("ClaimsPlugin.Api Shutting down...");
                Log.CloseAndFlush();
            }
        }
    }
}
