using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ClaimsPlugin.Infrastructure.Extensions
{
    static class RepositoryExtensions
    {
        public static void AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            var repositoryInterfaceType = typeof(IRepository<>);
            var repositoryTypes = assembly.GetTypes()
                .Where(type => type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == repositoryInterfaceType));

            foreach (var repositoryType in repositoryTypes)
            {
                var interfaceType = repositoryType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == repositoryInterfaceType);
                services.AddScoped(interfaceType, repositoryType);
            }
        }
    }
}
