using Serilog;
using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;

namespace ClaimsPlugin.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // ConfigureHttp(builder.Services, builder.Configuration);

            // ConfigureRateLimit(builder.Services, builder.Configuration);

            // ConfigureApiVersioning(builder.Services);

            ConfigureSwagger(builder.Services);

            // ConfigureLog(builder.Host);

            // ConfigureHealthChecks(builder.Services, builder.Configuration);

            var app = builder.Build();

            //app.UseMiddleware<ExceptionHandlerMiddleware>();

            //app.UseForwardedHeaders();

            //app.UseIpRateLimiting();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseAuditLog();

            //app.UseHttpsRedirection();

            //app.UseCors();

            //app.UseAuthorization();

            //app.UseEppHealthChecks();

            app.MapControllers();

            // if (app.Environment.IsDevelopment())
            // {
            //     var apiVersionDescriptionProvider =
            //         app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            //     app.UseSwaggerUI(options =>
            //     {
            //         foreach (
            //             var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse()
            //         )
            //         {
            //             options.SwaggerEndpoint(
            //                 $"{description.GroupName}/swagger.json",
            //                 description.GroupName.ToUpperInvariant()
            //             );
            //         }
            //     });

            //     app.UseVersionEndpointMiddleware();
            // }

            app.Run();
        }

        // public static void ConfigureHttp(IServiceCollection services, IConfiguration configuration)
        // {
        //     services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //     services.Configure<ForwardedHeadersOptions>(opt =>
        //     {
        //         opt.ForwardedForHeaderName = configuration.GetValue<string>(
        //             "IpRateLimitOptions:RealIpHeader"
        //         );
        //         opt.ForwardedHeaders = ForwardedHeaders.XForwardedFor;
        //         opt.KnownNetworks.Add(new IPNetwork(IPAddress.Any, 0));
        //         opt.KnownNetworks.Add(new IPNetwork(IPAddress.IPv6Any, 0));
        //     });
        // }

        // public static void ConfigureLog(IHostBuilder hostBuilder)
        // {
        //     hostBuilder.UseSerilog(
        //         (context, config) =>
        //         {
        //             config.ReadFrom.Configuration(context.Configuration);
        //         }
        //     );
        // }

        // public static void ConfigureHealthChecks(
        //     IServiceCollection services,
        //     IConfiguration configuration
        // )
        // {
        //     services.AddEppHealthChecks(
        //         configuration,
        //         opt =>
        //         {
        //             opt.CheckActiveDirectory = false;
        //             opt.CheckDatabase = true;
        //         }
        //     );
        // }

        // public static void ConfigureRateLimit(
        //     IServiceCollection services,
        //     IConfiguration configuration
        // )
        // {
        //     services.AddMemoryCache();
        //     services.Configure<IpRateLimitOptions>(
        //         configuration.GetSection(nameof(IpRateLimitOptions))
        //     );
        //     services.Configure<IpRateLimitPolicies>(
        //         configuration.GetSection(nameof(IpRateLimitPolicies))
        //     );
        //     services.AddInMemoryRateLimiting();
        //     services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        // }

        public static void ConfigureSwagger(IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            var apiVersionDescriptionProvider = services
                .BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();
            services.AddSwaggerGen(swagger =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    var apiInfo = new OpenApiInfo
                    {
                        Title = "LFC-Backend.ClaimsPluginApi",
                        Version = $"{description.ApiVersion}"
                    };
                    if (description.IsDeprecated)
                    {
                        apiInfo.Description += " This API version has been deprecated.";
                    }
                    swagger.SwaggerDoc(description.GroupName, apiInfo);
                }
            });
        }

        // public static void ConfigureApiVersioning(IServiceCollection services)
        // {
        //     services.AddApiVersioning(o =>
        //     {
        //         o.AssumeDefaultVersionWhenUnspecified = true;
        //         o.DefaultApiVersion = new ApiVersion(1, 0);
        //         o.ReportApiVersions = true;
        //         o.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
        //     });
        //     services.AddVersionedApiExplorer(setup =>
        //     {
        //         setup.GroupNameFormat = "'v'VVV";
        //         setup.SubstituteApiVersionInUrl = true;
        //     });
        // }
    }
}
