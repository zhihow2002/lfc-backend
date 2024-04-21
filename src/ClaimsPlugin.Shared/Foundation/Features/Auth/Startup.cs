using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Helpers;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Middleware;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Models;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Providers;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.ContactConfirmation;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.PasswordReset;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.Permission;
using ClaimsPlugin.Shared.Foundation.Features.Auth.Requirements.TenantAccess;
using ClaimsPlugin.Shared.Foundation.Features.Identity.Configurations;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Foundation.Features.Identity;
using Foundation.Features.Identity.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Logging;
using OpenIddict.Validation.AspNetCore;
using Serilog;

namespace Foundation.Features.Auth;

internal static class Startup
{
    private static readonly Serilog.ILogger _logger = Log.ForContext(typeof(Startup));

    internal static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment,
        Assembly projectAssembly
    )
    {
        services
            .AddPolicyProvider(configuration)
            .AddPermissions()
            .AddTenantAccessControl()
            .AddPasswordReset()
            .AddContactConfirmation()
            .AddIdentity(configuration)
            .AddOpenIdConnect(configuration, webHostEnvironment, projectAssembly)
            .AddCurrentUser()
            .Configure<SecuritySettings>(configuration.GetSection(nameof(SecuritySettings)));
        return services;
    }

    internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CurrentIdentityMiddleware>();
    }

    private static IServiceCollection AddPasswordReset(this IServiceCollection services)
    {
        return services.AddScoped<IAuthorizationHandler, PasswordResetAuthorizationHandler>();
    }

    private static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        return services
            .AddScoped<CurrentIdentityMiddleware>()
            .AddScoped<ICurrentIdentity, CurrentIdentity>()
            .AddScoped(
                sp => (ICurrentIdentityInitializer)sp.GetRequiredService<ICurrentIdentity>()
            );
    }

    private static IServiceCollection AddPolicyProvider(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
    }

    private static IServiceCollection AddPermissions(this IServiceCollection services)
    {
        return services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }

    private static IServiceCollection AddTenantAccessControl(this IServiceCollection services)
    {
        return services.AddScoped<IAuthorizationHandler, TenantAccessAuthorizationHandler>();
    }

    private static IServiceCollection AddContactConfirmation(this IServiceCollection services)
    {
        return services.AddScoped<IAuthorizationHandler, ContactConfirmationAuthorizationHandler>();
    }

    private static IServiceCollection AddOpenIdConnect(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment,
        Assembly projectAssembly
    )
    {
        string currentApiProject =
            $"{projectAssembly.GetName().Name?.Split(new[] { "." }, StringSplitOptions.None)[0]}.Api";
        SecuritySettings? securitySettings = configuration
            .GetSection(nameof(SecuritySettings))
            .Get<SecuritySettings>();
        if (securitySettings == null)
        {
            throw new InvalidOperationException("Unable to read security setting.");
        }
        IdentityModelEventSource.ShowPII =
            webHostEnvironment.IsEnvironment("Development")
            || webHostEnvironment.IsEnvironment("SIT")
            || webHostEnvironment.IsEnvironment("UAT");
        string? pathBase = configuration["API_PATH_BASE"];
        OpenIddictBuilder openIddictBuilder = services
            .AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
            .Services.AddOpenIddict()
            .AddCore(options =>
            {
                options
                    .UseEntityFrameworkCore()
                    //  .UseDbContext<IdentityDatabaseContext>()
                    .ReplaceDefaultEntities<Guid>();
            });
        if (currentApiProject != IdentityConfiguration.IdentityApiProjectName)
        {
            openIddictBuilder.AddValidation(options =>
            {
                options.SetIssuer(securitySettings.OpenIdConnectSettings.Issuer);
                options
                    .UseIntrospection()
                    .SetClientId(IdentityConfiguration.DefaultIntrospector.ClientId)
                    .SetClientSecret(IdentityConfiguration.DefaultIntrospector.ClientSecret);
                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });
        }
        else
        {
            openIddictBuilder
                .AddServer(options =>
                {
                    options.RegisterScopes("api");
                    options.SetTokenEndpointUris(
                        $"{(pathBase.IsNotNullOrWhiteSpace() ? $"/{pathBase}" : "")}"
                            + securitySettings.OpenIdConnectSettings.Endpoint.Token,
                        securitySettings.OpenIdConnectSettings.Endpoint.Token
                    );
                    options.SetIntrospectionEndpointUris(
                        $"{(pathBase.IsNotNullOrWhiteSpace() ? $"/{pathBase}" : "")}"
                            + securitySettings.OpenIdConnectSettings.Endpoint.Introspection,
                        securitySettings.OpenIdConnectSettings.Endpoint.Introspection
                    );
                    options.SetCryptographyEndpointUris(
                        $"{(pathBase.IsNotNullOrWhiteSpace() ? $"/{pathBase}" : "")}"
                            + securitySettings.OpenIdConnectSettings.Endpoint.Cryptography,
                        securitySettings.OpenIdConnectSettings.Endpoint.Cryptography
                    );
                    options
                        .AllowClientCredentialsFlow()
                        .AllowRefreshTokenFlow()
                        .AllowPasswordFlow();
                    options.UseDataProtection();
                    options.SetRefreshTokenReuseLeeway(TimeSpan.FromSeconds(10));
                    options.UseReferenceAccessTokens();
                    options.UseReferenceRefreshTokens();
                    options.SetAccessTokenLifetime(
                        TimeSpan.FromMinutes(
                            securitySettings.OpenIdConnectSettings.TokenExpirationInMinutes
                        )
                    );
                    options.SetRefreshTokenLifetime(
                        TimeSpan.FromDays(
                            securitySettings.OpenIdConnectSettings.RefreshTokenExpirationInDays
                        )
                    );
                    if (
                        webHostEnvironment.IsEnvironment("Development")
                        || webHostEnvironment.IsEnvironment("SIT")
                        || webHostEnvironment.IsEnvironment("UAT")
                    )
                    {
                        options.AddDevelopmentEncryptionCertificate();
                        options.AddDevelopmentSigningCertificate();
                    }
                    else
                    {
                        if (
                            securitySettings.OpenIdConnectSettings.Certificate.Encryption.Key.IsNullOrWhiteSpace()
                            || securitySettings.OpenIdConnectSettings.Certificate.Encryption.Name.IsNullOrWhiteSpace()
                            || securitySettings.OpenIdConnectSettings.Certificate.Signing.Key.IsNullOrWhiteSpace()
                            || securitySettings.OpenIdConnectSettings.Certificate.Signing.Name.IsNullOrWhiteSpace()
                        )
                        {
                            throw new InvalidOperationException(
                                "Encryption and signing certificate are required for production."
                            );
                        }
                        X509Certificate2? selfSignedCertificateForEncryption =
                            LoadSelfSignedCertificate(
                                securitySettings.OpenIdConnectSettings.Certificate.Encryption.Name,
                                securitySettings.OpenIdConnectSettings.Certificate.Encryption.Key
                            );
                        if (
                            selfSignedCertificateForEncryption.IsNull()
                            || IsSelfSignedCertificateExpiringSoon(
                                selfSignedCertificateForEncryption,
                                7
                            )
                        )
                        {
                            selfSignedCertificateForEncryption = GenerateSelfSignedCertificate(
                                "",
                                DateTimeOffset.Now,
                                DateTimeOffset.Now.AddYears(1),
                                securitySettings.OpenIdConnectSettings.Certificate.Encryption.Name,
                                securitySettings.OpenIdConnectSettings.Certificate.Encryption.Key
                            );
                        }
                        X509Certificate2? selfSignedCertificateForSigning =
                            LoadSelfSignedCertificate(
                                securitySettings.OpenIdConnectSettings.Certificate.Signing.Name,
                                securitySettings.OpenIdConnectSettings.Certificate.Signing.Key
                            );
                        if (
                            selfSignedCertificateForSigning.IsNull()
                            || IsSelfSignedCertificateExpiringSoon(
                                selfSignedCertificateForSigning,
                                7
                            )
                        )
                        {
                            selfSignedCertificateForSigning = GenerateSelfSignedCertificate(
                                "",
                                DateTimeOffset.Now,
                                DateTimeOffset.Now.AddYears(1),
                                securitySettings.OpenIdConnectSettings.Certificate.Signing.Name,
                                securitySettings.OpenIdConnectSettings.Certificate.Signing.Key
                            );
                        }
                        options.AddEncryptionCertificate(selfSignedCertificateForEncryption);
                        options.AddSigningCertificate(selfSignedCertificateForSigning);
                    }
                    options
                        .UseAspNetCore()
                        .EnableTokenEndpointPassthrough()
                        .DisableTransportSecurityRequirement();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseDataProtection();
                    options.UseAspNetCore();
                });
        }
        return openIddictBuilder
            .Services.AddDataProtection()
            //    .PersistKeysToDbContext<IdentityDatabaseContext>()
            .SetApplicationName(IdentityConfiguration.IdentityApiProjectName)
            .Services;
    }

    private static X509Certificate2 GenerateSelfSignedCertificate(
        string subjectName,
        DateTimeOffset notBefore,
        DateTimeOffset notAfter,
        string certificateName,
        string password
    )
    {
        using RSA rsa = RSA.Create(2048);
        CertificateRequest certificateRequest =
            new($"CN={subjectName}", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        certificateRequest.CertificateExtensions.Add(
            new X509KeyUsageExtension(
                X509KeyUsageFlags.DataEncipherment
                    | X509KeyUsageFlags.KeyEncipherment
                    | X509KeyUsageFlags.DigitalSignature,
                false
            )
        );
        certificateRequest.CertificateExtensions.Add(
            new X509EnhancedKeyUsageExtension(new OidCollection { new("1.3.6.1.5.5.7.3.1") }, false)
        );
        certificateRequest.CertificateExtensions.Add(
            new X509SubjectKeyIdentifierExtension(certificateRequest.PublicKey, false)
        );
        X509Certificate2 certificate = certificateRequest.CreateSelfSigned(notBefore, notAfter);
        string directoryFullPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Assets",
            "Certificates"
        );
        if (!Directory.Exists(directoryFullPath))
        {
            Directory.CreateDirectory(directoryFullPath);
        }
        string certificatePath = Path.Combine(directoryFullPath, certificateName + ".pfx");
        File.WriteAllBytes(certificatePath, certificate.Export(X509ContentType.Pfx, password));
        return new X509Certificate2(
            certificate.Export(X509ContentType.Pfx, password),
            password,
            X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet
        );
    }

    private static X509Certificate2? LoadSelfSignedCertificate(
        string certificatePath,
        string password
    )
    {
        return File.Exists(certificatePath)
            ? new X509Certificate2(
                certificatePath,
                password,
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet
            )
            : null;
    }

    private static bool IsSelfSignedCertificateExpiringSoon(
        X509Certificate2 certificate,
        int daysBeforeExpiration
    )
    {
        DateTimeOffset expirationThreshold = DateTimeOffset.UtcNow.AddDays(daysBeforeExpiration);
        return certificate.NotAfter <= expirationThreshold;
    }
}
