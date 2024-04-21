using System;
using System.Linq;
using System.Reflection;
using Ardalis.Specification;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Decorators;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ClaimsPlugin.Api.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScopedRepositoriesWithEvents(
            this IServiceCollection services,
            Assembly assembly
        )
        {
            var typesFromAssemblies = assembly
                .GetTypes()
                .Where(
                    t =>
                        t.GetInterfaces()
                            .Any(
                                i =>
                                    i.IsGenericType
                                    && i.GetGenericTypeDefinition()
                                        == typeof(IRepositoryWithEvents<>)
                            )
                )
                .ToList();

            foreach (var type in typesFromAssemblies)
            {
                foreach (
                    var intf in type.GetInterfaces()
                        .Where(
                            i =>
                                i.IsGenericType
                                && i.GetGenericTypeDefinition() == typeof(IRepositoryWithEvents<>)
                        )
                )
                {
                    services.AddScoped(intf, type);
                }
            }

            return services;
        }

        public static IServiceCollection AddMediatRHandlers(
            this IServiceCollection services,
            Assembly assembly
        )
        {
            // Logic to scan for and register handlers
            var handlerTypes = assembly
                .GetTypes()
                .Where(
                    type =>
                        !type.IsAbstract
                        && type.GetInterfaces()
                            .Any(
                                i =>
                                    i.IsGenericType
                                    && (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                            )
                )
                .ToList();

            foreach (var type in handlerTypes)
            {
                var interfaces = type.GetInterfaces()
                    .Where(
                        i =>
                            i.IsGenericType
                            && (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                    );

                foreach (var intf in interfaces)
                {
                    services.AddScoped(intf, type);
                }
            }

            return services;
        }

        public static IServiceCollection AddRepositoryDecorators(this IServiceCollection services)
        {
            // Register the base repository with its concrete implementation
            services.AddScoped(
                typeof(IRepository<>),
               // typeof(YourConcreteRepositoryImplementation<>)
            );

            // Register the decorator to wrap the base repository
            services.AddScoped(
                typeof(IRepositoryWithEvents<>),
                serviceProvider =>
                {
                    // Resolve the base repository that our decorator will wrap
                    var decoratedRepositoryType = typeof(IRepository<>).MakeGenericType(typeof(T));
                    var decoratedRepository = serviceProvider.GetService(decoratedRepositoryType);
                    if (decoratedRepository == null)
                    {
                        throw new InvalidOperationException(
                            $"The repository of type {decoratedRepositoryType.Name} has not been registered."
                        );
                    }

                    // Create the decorator instance
                    var decoratorType = typeof(EventAddingRepositoryDecorator<>).MakeGenericType(
                        typeof(T)
                    );
                    return Activator.CreateInstance(decoratorType, decoratedRepository);
                }
            );

            return services;
        }
    }
}
