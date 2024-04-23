//using System.Reflection;
//using ClaimsPlugin.Shared.Foundation.Common.Persistence.Constants;
//using ClaimsPlugin.Shared.Foundation.Common.Persistence.Decorators;
//using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
//using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
//using ClaimsPlugin.Shared.Foundation.Common.Persistence.Services;
//using ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.Interfaces;
//using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Configurations;
//using ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;
//using Foundation.Features.MultiTenancy.Services;
//using Microsoft.EntityFrameworkCore;
//using Serilog;

//namespace ClaimsPlugin.Shared.Foundation.Common.Persistence;

//public static class Startup
//{
//    private static readonly Serilog.ILogger _logger = Log.ForContext(typeof(Startup));

//    public static IServiceCollection AddMultiTenancyDatabaseInitializer(
//        this IServiceCollection services
//    )
//    {
//        return services.AddTransient<
//            IMultiTenancyDatabaseInitializer,
//            MultiTenancyDatabaseInitializer
//        >();
//    }

//    // public static IServiceCollection AddIdentityDatabaseInitializer(this IServiceCollection services)
//    // {
//    //     return services.AddTransient<IIdentityDatabaseInitializer, IdentityDatabaseInitializer>();
//    // }
//    internal static IServiceCollection AddPersistence<TDbContext, TDbSeeder>(
//        this IServiceCollection services,
//        IConfiguration configuration,
//        Assembly projectAssembly
//    )
//        where TDbContext : BaseDatabaseContext, IDatabaseContext
//        where TDbSeeder : class, IDatabaseSeeder
//    {
//        services
//            .Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)))
//            //.Configure<IdentityDatabaseSettings>(
//            //    configuration.GetSection(nameof(IdentityDatabaseSettings))
//            //)
//            //.Configure<MultiTenancyDatabaseSettings>(
//            //    configuration.GetSection(nameof(MultiTenancyDatabaseSettings))
//            //)
//            .AddDbContext<TDbContext>()
//            .AddTransient<TDbSeeder>()
//            .AddTransient<IDatabaseInitializer, BaseDatabaseInitializer<TDbContext, TDbSeeder>>()
//            .AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>()
//            .AddTransient<IConnectionDetailValidator, ConnectionDetailValidator>()
//            .AddRepositories<TDbContext>(projectAssembly);
//        return services;
//    }

//    internal static IApplicationBuilder UseDatabaseInitializer(
//        this IApplicationBuilder builder,
//        IConfiguration configuration
//    )
//    {
//        using IServiceScope scope = builder.ApplicationServices.CreateScope();
//        IWebHostEnvironment environment =
//            scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
//        bool allowMigration =
//            environment.IsEnvironment("Development")
//            || environment.IsEnvironment("SIT")
//            || environment.IsEnvironment("UAT");
//        _logger.Information(
//            "Allow Migration for {EnvironmentName} environment: {AllowMigration}",
//            environment.EnvironmentName,
//            allowMigration
//        );
//        scope
//            .ServiceProvider.GetService<IMultiTenancyDatabaseInitializer>()
//            ?.InitializeAsync(
//                allowMigration ? DatabaseInitializeMode.Both : DatabaseInitializeMode.OnlyDataSeed,
//                CancellationToken.None
//            )
//            .GetAwaiter()
//            .GetResult();
//        // scope.ServiceProvider.GetService<IIdentityDatabaseInitializer>()?.InitializeAsync(allowMigration ? DatabaseInitializeMode.Both : DatabaseInitializeMode.OnlyDataSeed, CancellationToken.None).GetAwaiter().GetResult();
//        if (configuration.GetValue<bool>("DatabaseSettings:IsMultiTenancy"))
//        {
//            scope
//                .ServiceProvider.GetRequiredService<ICurrentTenantInitializer>()
//                .SetCurrentTenantId(TenantConfiguration.GetAllDefaultTenantList().First().Id);
//        }
//        scope
//            .ServiceProvider.GetRequiredService<IDatabaseInitializer>()
//            .InitializeAsync(
//                allowMigration ? DatabaseInitializeMode.Both : DatabaseInitializeMode.OnlyDataSeed,
//                CancellationToken.None
//            )
//            .GetAwaiter()
//            .GetResult();
//        return builder;
//    }

//    internal static DbContextOptionsBuilder UseDatabase(
//        this DbContextOptionsBuilder builder,
//        string? connectionString,
//        string? assemblyName = null
//    )
//    {
//        if (assemblyName is not null)
//        {
//            return builder.UseSqlServer(
//                connectionString ?? throw new ArgumentNullException(nameof(connectionString)),
//                e =>
//                {
//                    e.MigrationsAssembly(assemblyName);
//                    e.EnableRetryOnFailure(3, TimeSpan.FromSeconds(30), null);
//                    e.UseNetTopologySuite();
//                }
//            );
//        }
//        return builder.UseSqlServer(
//            connectionString ?? throw new ArgumentNullException(nameof(connectionString)),
//            e =>
//            {
//                e.EnableRetryOnFailure(3, TimeSpan.FromSeconds(30), null);
//                e.UseNetTopologySuite();
//            }
//        );
//    }

//    private static IServiceCollection AddRepositories<TDbContext>(
//        this IServiceCollection services,
//        Assembly projectAssembly
//    )
//        where TDbContext : BaseDatabaseContext, IDatabaseContext
//    {
//        string assemblyName = projectAssembly.GetName().Name!;
//        string? projectName = assemblyName.Split(new[] { "." }, StringSplitOptions.None)?[0];
//        AssemblyName? assembly = projectAssembly
//            .GetReferencedAssemblies()
//            .SingleOrDefault(p => p.Name!.Equals($"{projectName}.Domain"));
//        if (assembly is not null)
//        {
//            foreach (
//                Type aggregateRootType in Assembly
//                    .Load(assembly)
//                    .GetExportedTypes()
//                    .Where(p => typeof(IAggregateRoot).IsAssignableFrom(p) && p.IsClass)
//            )
//            {
//                services.AddScoped(
//                    typeof(IRepository<>).MakeGenericType(aggregateRootType),
//                    typeof(BaseRepository<,>).MakeGenericType(typeof(TDbContext), aggregateRootType)
//                );
//                services.AddScoped(
//                    typeof(IReadRepository<>).MakeGenericType(aggregateRootType),
//                    typeof(BaseRepository<,>).MakeGenericType(typeof(TDbContext), aggregateRootType)
//                );
//                services.AddScoped(
//                    typeof(IRepositoryWithEvents<>).MakeGenericType(aggregateRootType),
//                    sp =>
//                        Activator.CreateInstance(
//                            typeof(EventAddingRepositoryDecorator<>).MakeGenericType(
//                                aggregateRootType
//                            ),
//                            sp.GetRequiredService(
//                                typeof(IRepository<>).MakeGenericType(aggregateRootType)
//                            )
//                        )
//                        ?? throw new InvalidOperationException(
//                            $"Couldn't create EventAddingRepositoryDecorator for aggregateRootType {aggregateRootType.Name}"
//                        )
//                );
//            }
//        }
//        return services;
//    }
//}
