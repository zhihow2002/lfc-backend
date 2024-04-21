using System.Reflection;
using ClaimsPlugin.Shared.Foundation.Features.Api.Grpc.Interceptors;
using ClaimsPlugin.Shared.Foundation.Features.Api.Grpc.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Hosting.Models;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;
using Grpc.Net.ClientFactory;
using ProtoBuf.Grpc.ClientFactory;

namespace ClaimsPlugin.Shared.Foundation.Features.Api.Grpc;

public static class Startup
{
    public static IServiceCollection AddGrpcClients(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly projectAssembly
    )
    {
        const string protobufNamespace = "Common.Protobufs";
        string assemblyName = projectAssembly.GetName().Name!;
        string projectName = assemblyName.Split(new[] { "." }, StringSplitOptions.None)[0];
        AssemblyName? assembly = projectAssembly
            .GetReferencedAssemblies()
            .SingleOrDefault(p => p.Name!.Equals(protobufNamespace));
        List<HostingSettings>? hostingSettings = configuration
            .GetSection("HostingSettings")
            .Get<List<HostingSettings>>();
        if (hostingSettings.IsNull())
        {
            throw new InvalidOperationException("Unable to read hosting setting.");
        }
        if (assembly is not null)
        {
            foreach (
                Type concreteType in Assembly
                    .Load(assembly)
                    .GetExportedTypes()
                    .Where(p => typeof(IGrpc).IsAssignableFrom(p) && p.IsInterface)
            )
            {
                string uniqueClientName = concreteType.FullName!.Replace(".", "_");
                MethodInfo? method = typeof(ServicesExtensions)
                    .GetMethod(
                        "AddCodeFirstGrpcClient",
                        new[]
                        {
                            typeof(IServiceCollection),
                            typeof(string),
                            typeof(Action<GrpcClientFactoryOptions>)
                        }
                    )
                    ?.MakeGenericMethod(concreteType);
                if (method is not null)
                {
                    string[]? namespaces = concreteType
                        .Namespace?.Replace(protobufNamespace, string.Empty)
                        .Split(".", StringSplitOptions.RemoveEmptyEntries);
                    if (namespaces is null || namespaces.Length == 0)
                    {
                        throw new InvalidOperationException(
                            $"Couldn't match the namespace for client {concreteType.Name}. Expecting namespace starting with {protobufNamespace}."
                        );
                    }
                    if (projectName == namespaces[0])
                    {
                        continue;
                    }
                    HostingSettings? setting = hostingSettings.FirstOrDefault(
                        x =>
                            x.Name.IsEqualTo(
                                $"{namespaces[0]}.Api",
                                StringComparison.OrdinalIgnoreCase
                            )
                    );
                    if (setting.IsNull())
                    {
                        throw new InvalidOperationException(
                            "Unable to read particular hosting setting."
                        );
                    }
                    void Options(GrpcClientFactoryOptions o)
                    {
                        o.Address = new Uri($"{setting.Grpc.Url}:{setting.Grpc.Port}");
                        o.ChannelOptionsActions.Add(options =>
                        {
                            options.HttpHandler = new SocketsHttpHandler
                            {
                                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                                EnableMultipleHttp2Connections = true
                            };
                        });
                        o.InterceptorRegistrations.Add(
                            new InterceptorRegistration(
                                InterceptorScope.Channel,
                                options => options.GetRequiredService<TenantInterceptor>()
                            )
                        );
                    }
                    method.Invoke(
                        null,
                        new object?[]
                        {
                            services,
                            uniqueClientName,
                            (Action<GrpcClientFactoryOptions>)Options
                        }
                    );
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Couldn't add GrpcClient for client {concreteType.Name}"
                    );
                }
            }
        }
        services.AddTransient<TenantInterceptor>();
        return services;
    }

    internal static IEndpointRouteBuilder MapGrpcHandlers(
        this IEndpointRouteBuilder endpoints,
        Assembly projectAssembly
    )
    {
        string assemblyName = projectAssembly.GetName().Name!;
        string projectName = assemblyName.Split(new[] { "." }, StringSplitOptions.None)[0];
        AssemblyName? assembly = projectAssembly
            .GetReferencedAssemblies()
            .SingleOrDefault(p => p.Name!.Equals($"{projectName}.Application"));
        if (assembly is not null)
        {
            foreach (
                Type concreteType in Assembly
                    .Load(assembly)
                    .GetExportedTypes()
                    .Where(p => typeof(IGrpc).IsAssignableFrom(p) && p.IsClass)
            )
            {
                MethodInfo? method = typeof(GrpcEndpointRouteBuilderExtensions)
                    .GetMethod("MapGrpcService")
                    ?.MakeGenericMethod(concreteType);
                if (method is not null)
                {
                    method.Invoke(null, new object?[] { endpoints });
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Couldn't map GrpcHandler for handler {concreteType.Name}"
                    );
                }
            }
            foreach (
                Type concreteType in projectAssembly
                    .GetExportedTypes()
                    .Where(p => typeof(IGrpc).IsAssignableFrom(p) && p.IsClass)
            )
            {
                MethodInfo? method = typeof(GrpcEndpointRouteBuilderExtensions)
                    .GetMethod("MapGrpcService")
                    ?.MakeGenericMethod(concreteType);
                if (method is not null)
                {
                    method.Invoke(null, new object?[] { endpoints });
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Couldn't map GrpcHandler for handler {concreteType.Name}"
                    );
                }
            }
        }
        return endpoints;
    }
}
