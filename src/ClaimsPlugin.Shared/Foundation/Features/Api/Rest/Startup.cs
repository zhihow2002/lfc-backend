//using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.OpenApi.Processors;
//using ClaimsPlugin.Shared.Foundation.Features.Api.Rest.Transformers;
//using ClaimsPlugin.Shared.Foundation.Features.Auth.Models;
//using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Exceptions;
//using ClaimsPlugin.Shared.Foundation.Features.ExceptionHandling.Models;
//using ClaimsPlugin.Shared.Foundation.Features.Hosting.Models;
//using ClaimsPlugin.Shared.Foundation.Features.Identity.Configurations;
//using ClaimsPlugin.Shared.Foundation.Features.Serializer;
//using ClaimsPlugin.Shared.Foundation.Features.Serializer.Interfaces;
//using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
//using Flurl;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ApplicationModels;
//using NJsonSchema;
//using NJsonSchema.Generation.TypeMappers;
//using NSwag;
//using NSwag.AspNetCore;
//using NSwag.Generation.Processors;
//using NSwag.Generation.Processors.Security;
//using OpenIddict.Validation.AspNetCore;
//using Serilog;
//using ZymLabs.NSwag.FluentValidation;
//using ILogger = Serilog.ILogger;
//using SwaggerSettings = ClaimsPlugin.Shared.Foundation.Features.Api.Rest.OpenApi.Models.SwaggerSettings;

//namespace ClaimsPlugin.Shared.Foundation.Features.Api.Rest;

//public static class Startup
//{
//    private static readonly ILogger _logger = Log.ForContext(typeof(Startup));

//    internal static IServiceCollection AddOpenApiDocumentation(
//        this IServiceCollection services,
//        IConfiguration configuration
//    )
//    {
//        SwaggerSettings? settings = configuration
//            .GetSection(nameof(SwaggerSettings))
//            .Get<SwaggerSettings>();
//        if (settings?.Enabled is true)
//        {
//            SecuritySettings? securitySettings = configuration
//                .GetSection(nameof(SecuritySettings))
//                .Get<SecuritySettings>();

//            if (securitySettings == null)
//            {
//                throw new InvalidOperationException("Unable to read security setting.");
//            }

//            //HostingSettings? hostingSettings = configuration
//            //    .GetSection(nameof(HostingSettings))
//            //    .Get<List<HostingSettings>>()
//            //    ?.FirstOrDefault(x => x.Name == IdentityConfiguration.IdentityApiProjectName);

//            //if (hostingSettings == null)
//            //{
//            //    throw new InvalidOperationException("Unable to read hosting setting.");
//            //}

//            services.AddVersionedApiExplorer(o => o.SubstituteApiVersionInUrl = true);
//            services.AddEndpointsApiExplorer();
//            services.AddOpenApiDocument(
//                (document, serviceProvider) =>
//                {
//                    document.PostProcess = doc =>
//                    {
//                        doc.Info.Title = settings.Title;
//                        doc.Info.Version = settings.Version;
//                        doc.Info.Description = settings.Description;
//                        doc.Info.Contact = new OpenApiContact
//                        {
//                            Name = settings.ContactName,
//                            Email = settings.ContactEmail,
//                            Url = settings.ContactUrl
//                        };

//                        if (settings.License)
//                        {
//                            doc.Info.License = new OpenApiLicense
//                            {
//                                Name = settings.LicenseName,
//                                Url = settings.LicenseUrl
//                            };
//                        }
//                    };

//                    Dictionary<string, string> scopes =
//                        new() { { "offline_access", "To use refresh token." } };

//                    string tokenUrl;
//                    string refreshUrl;

//                    //if (hostingSettings.RestHttps.Enabled)
//                    //{
//                    //    tokenUrl =
//                    //        $"{hostingSettings.RestHttps.Url}:{hostingSettings.RestHttps.Port}".AppendPathSegment(
//                    //            securitySettings.OpenIdConnectSettings.Endpoint.Token
//                    //        );
//                    //    refreshUrl =
//                    //        $"{hostingSettings.RestHttps.Url}:{hostingSettings.RestHttps.Port}".AppendPathSegment(
//                    //            securitySettings.OpenIdConnectSettings.Endpoint.Token
//                    //        );
//                    //}
//                    //else if (hostingSettings.RestHttp.Enabled)
//                    //{
//                    //    tokenUrl =
//                    //        $"{hostingSettings.RestHttp.Url}:{hostingSettings.RestHttp.Port}".AppendPathSegment(
//                    //            securitySettings.OpenIdConnectSettings.Endpoint.Token
//                    //        );
//                    //    refreshUrl =
//                    //        $"{hostingSettings.RestHttp.Url}:{hostingSettings.RestHttp.Port}".AppendPathSegment(
//                    //            securitySettings.OpenIdConnectSettings.Endpoint.Token
//                    //        );
//                    //}
//                    //else
//                    //{
//                    //    throw new InternalServerException(
//                    //        "Either one of the http protocol must be enabled."
//                    //    );
//                    //}

//                    //document.AddSecurity(
//                    //    OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
//                    //    new OpenApiSecurityScheme
//                    //    {
//                    //        Type = OpenApiSecuritySchemeType.OAuth2,
//                    //        Flow = OpenApiOAuth2Flow.Password,
//                    //        Description = "OAuth 2.0",
//                    //        Flows = new OpenApiOAuthFlows
//                    //        {
//                    //            Password = new OpenApiOAuthFlow
//                    //            {
//                    //                TokenUrl = tokenUrl,
//                    //                RefreshUrl = refreshUrl,
//                    //                Scopes = scopes
//                    //            }
//                    //        }
//                    //    }
//                    //);

//                    document.OperationProcessors.Add(
//                        new AspNetCoreOperationSecurityScopeProcessor()
//                    );
//                    document.OperationProcessors.Add(new SwaggerGlobalAuthProcessor());

//                    // // ...
//                    // document.TypeMappers.Add(
//                    //     new PrimitiveTypeMapper(
//                    //         typeof(TimeSpan),
//                    //         schema =>
//                    //         {
//                    //             schema.Type = JsonObjectType.String;
//                    //             schema.IsNullableRaw = true;
//                    //             schema.Pattern =
//                    //                 @"^([0-9]{1}|(?:0[0-9]|1[0-9]|2[0-3])+):([0-5]?[0-9])(?::([0-5]?[0-9])(?:.(\d{1,9}))?)?$";
//                    //             schema.Example = "02:00:00";
//                    //         }
//                    //     )
//                    // );




//                    document.OperationProcessors.Add(new SwaggerHeaderAttributeProcessor());

//                    FluentValidationSchemaProcessor? fluentValidationSchemaProcessor =
//                        serviceProvider
//                            .CreateScope()
//                            .ServiceProvider.GetService<FluentValidationSchemaProcessor>();

//                    // document.SchemaProcessors.Add(fluentValidationSchemaProcessor);
//                }
//            );

//            services.AddScoped<FluentValidationSchemaProcessor>(provider =>
//            {
//                IEnumerable<FluentValidationRule>? validationRules = provider.GetService<
//                    IEnumerable<FluentValidationRule>
//                >();
//                ILoggerFactory? loggerFactory = provider.GetService<ILoggerFactory>();

//                return new FluentValidationSchemaProcessor(
//                    provider,
//                    validationRules,
//                    loggerFactory
//                );
//            });
//        }

//        return services;
//    }

//    internal static IApplicationBuilder UseOpenApiDocumentation(
//        this IApplicationBuilder app,
//        IConfiguration configuration
//    )
//    {
//        SwaggerSettings? settings = configuration
//            .GetSection(nameof(SwaggerSettings))
//            .Get<SwaggerSettings>();

//        if (settings?.Enabled is true)
//        {
//            SecuritySettings? securitySettings = configuration
//                .GetSection(nameof(SecuritySettings))
//                .Get<SecuritySettings>();

//            if (securitySettings == null)
//            {
//                throw new InvalidOperationException("Unable to read security setting.");
//            }

//            app.UseOpenApi(options =>
//            {
//                options.PostProcess = (document, request) =>
//                {
//                    // To append the ingress rewrite rules to the url to load the contents
//                    string? pathBase = configuration["API_PATH_BASE"];

//                    if (pathBase.IsNotNullOrWhiteSpace())
//                    {
//                        document.BasePath = new PathString("/" + pathBase);
//                    }
//                };
//            });
//            app.UseSwaggerUi(options =>
//            {
//                options.DefaultModelsExpandDepth = -1;
//                options.DocExpansion = "none";
//                options.TagsSorter = "alpha";
//                options.TransformToExternalPath = (url, request) =>
//                {
//                    // To append the ingress rewrite rules to the url to load the contents
//                    if (url.EndsWith(".json") || request.Path.ToString().EndsWith("/"))
//                    {
//                        return ".." + url;
//                    }
//                    else
//                    {
//                        return request.PathBase + "." + url;
//                    }
//                };

//                //DefaultApplication? firstApplication = IdentityConfiguration
//                //    .DefaultApplications.Applications()
//                //    .FirstOrDefault();

//                //if (firstApplication.IsNotNull())
//                //{
//                //    options.OAuth2Client = new OAuth2ClientSettings
//                //    {
//                //        AppName = firstApplication.DisplayName,
//                //        ClientId = firstApplication.ClientId,
//                //        ClientSecret = firstApplication.ClientSecret,
//                //        ScopeSeparator = " "
//                //    };
//                //    options.OAuth2Client.Scopes.Add(string.Join(" ", firstApplication.Permissions));
//                }
//            });
//        }

//        return app;
//    }

//    internal static IServiceCollection AddApiVersioning(
//        this IServiceCollection services,
//        IConfiguration configuration
//    )
//    {
//        return services.AddApiVersioning(options =>
//        {
//            options.DefaultApiVersion = new ApiVersion(1, 0);
//            options.AssumeDefaultVersionWhenUnspecified = true;
//            options.ReportApiVersions = true;
//        });
//    }

//    internal static IServiceCollection AddControllers(
//        this IServiceCollection services,
//        IConfiguration configuration
//    )
//    {
//        return services
//            .AddControllers(options =>
//            {
//                options.Conventions.Add(
//                    new RouteTokenTransformerConvention(new SlugifyParameterTransformer())
//                );
//            })
//            .AddJsonConverters()
//            .ConfigureApiBehaviorOptions(options =>
//            {
//                options.InvalidModelStateResponseFactory = c =>
//                {
//                    ISerializerService jsonSerializer =
//                        c.HttpContext.RequestServices.GetRequiredService<ISerializerService>();

//                    List<string> errors = c.ModelState.Values.Where(v => v.Errors.Count > 0)
//                        .SelectMany(v => v.Errors)
//                        .Select(v => v.ErrorMessage)
//                        .ToList();

//                    ValidationException exception = new(errors!);

//                    string traceId = Guid.NewGuid().ToString();

//                    ErrorResult errorResult =
//                        new()
//                        {
//                            Title = "One or more validation errors occurred.",
//                            Messages = exception.ErrorMessages.IsNotNullOrEmpty()
//                                ? exception
//                                    .ErrorMessages.Where(x => x.IsNotNullOrEmpty())
//                                    .Select(x => x!)
//                                    .ToList()
//                                : new List<string>(),
//                            TraceId = traceId,
//                            StatusCode = (int)exception.StatusCode
//                        };

//                    _logger.Error(
//                        "HTTP {Method} Request failed with Status Code {StatusCode}, Uri: {Request}, Trace Id: {TraceId}, Error: {ErrorResult}",
//                        c.HttpContext.Request.Method,
//                        c.HttpContext.Response.StatusCode,
//                        $"{c.HttpContext.Request.Scheme}://{c.HttpContext.Request.Host.Value}{(c.HttpContext.Request.PathBase.HasValue ? $"/{c.HttpContext.Request.PathBase.Value}" : string.Empty)}{c.HttpContext.Request.Path}{c.HttpContext.Request.QueryString}",
//                        traceId,
//                        jsonSerializer.Serialize(errorResult, escapeNonAsciiCharacters: false)
//                    );

//                    return new BadRequestObjectResult(errorResult);
//                };
//            })
//            .Services;
//    }
//}
