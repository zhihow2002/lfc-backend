using ClaimsPlugin.Shared.Foundation.Features.DependencyInjection.Interfaces;
using Foundation.Features.MultiTenancy.DataTransferObjects;

namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;

public interface ITenantService : ITransientService
{
    Task<List<TenantDataTransferObject>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsWithIdAsync(string id);
    Task<TenantDataTransferObject> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<TenantDataTransferObject> AddAsync(
        string id,
        string name,
        string? issuer,
        CancellationToken cancellationToken = default
    );

    Task ActivateAsync(string id, CancellationToken cancellationToken = default);
    Task DeactivateAsync(string id, CancellationToken cancellationToken = default);
    Task UpdateSubscriptionAsync(string id, DateTime extendedExpiryDate, CancellationToken cancellationToken = default);
    Task InvalidateTenantListCacheAsync(CancellationToken cancellationToken = default);
}
