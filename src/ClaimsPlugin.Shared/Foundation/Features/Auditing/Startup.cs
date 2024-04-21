using ClaimsPlugin.Shared.Foundation.Features.Auditing.Interfaces;
using ClaimsPlugin.Shared.Foundation.Features.Auditing.Services;
using Microsoft.EntityFrameworkCore;
namespace Foundation.Features.Auditing;
internal static class Startup
{
    internal static IServiceCollection AddAuditing<TDbContext>(this IServiceCollection services) where TDbContext : DbContext, IAuditableDbContext
    {
        return services.AddTransient(typeof(IAuditService), typeof(AuditService<TDbContext>));
    }
}
