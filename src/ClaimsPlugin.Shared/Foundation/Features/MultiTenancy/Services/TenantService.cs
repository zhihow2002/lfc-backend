using Foundation.Common.Persistence.Interfaces;
using Foundation.Features.Caching.Extensions;
using Foundation.Features.Caching.Interfaces;
using Foundation.Features.ExceptionHandling.Exceptions;
using Foundation.Features.Identity.Configurations;
using Foundation.Features.MultiTenancy.DataTransferObjects;
using Foundation.Features.MultiTenancy.Entities;
using Foundation.Features.MultiTenancy.Interfaces;
using Foundation.Features.MultiTenancy.Models;
using Foundation.Features.MultiTenancy.Persistence;
using Foundation.Features.Notifications.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Foundation.Features.MultiTenancy.Services;

internal class TenantService : ITenantService
{
    private readonly ICacheKeyService _cacheKeyService;
    private readonly ICacheService _cacheService;
    private readonly IStringLocalizer<TenantService> _localizer;
    private readonly INotificationSender _notificationSender;
    private readonly TenantDatabaseContext _context;

    public TenantService(
        IStringLocalizer<TenantService> localizer,
        INotificationSender notificationSender,
        ICacheService cacheService,
        ICacheKeyService cacheKeyService,
        TenantDatabaseContext context
    )
    {
        _localizer = localizer;
        _notificationSender = notificationSender;
        _cacheService = cacheService;
        _cacheKeyService = cacheKeyService;
        _context = context;
    }

    public async Task<List<TenantDataTransferObject>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        List<TenantDataTransferObject>? tenants = await _cacheService.GetOrSetAsync(
            _cacheKeyService.GetCacheKey(IdentityConfiguration.IdentityClaimTypes.Tenant, "list", false),
            async () => (await _context.Tenants.ToListAsync(cancellationToken: cancellationToken)).Adapt<List<TenantDataTransferObject>>(),
            null,
            DateTimeOffset.Now.AddMonths(1),
            cancellationToken
        );

        return tenants ?? new List<TenantDataTransferObject>();
    }

    public async Task<TenantDataTransferObject> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return (await GetTenantInfoAsync(id, cancellationToken)).Adapt<TenantDataTransferObject>();
    }

    public async Task<TenantDataTransferObject> AddAsync(
        string id,
        string name,
        string? issuer,
        CancellationToken cancellationToken = default
    )
    {
        if (await ExistsWithIdAsync(id))
        {
            throw new DomainException(_localizer["tenant.idAlreadyExists"]);
        }

        Tenant tenant = new(id, name, issuer);

        await _context.AddAsync(tenant, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        await _notificationSender.BroadcastAsync(
            new TenantNotification(id, name, issuer),
            cancellationToken
        );

        await InvalidateTenantListCacheAsync(cancellationToken);

        return new TenantDataTransferObject
        {
            Id = tenant.Id,
            Name = tenant.Name,
            IsActive = tenant.IsActive,
            ValidUpto = tenant.ValidUpto,
            Issuer = tenant.Issuer
        };
    }

    public async Task ActivateAsync(string id, CancellationToken cancellationToken = default)
    {
        Tenant tenantResult = await GetTenantInfoAsync(id, cancellationToken);

        if (tenantResult.IsActive)
        {
            throw new DomainException("Tenant is already activated.");
        }

        tenantResult.Activate();

        _context.Tenants.Update(tenantResult);

        await _context.SaveChangesAsync(cancellationToken);

        await InvalidateTenantListCacheAsync(cancellationToken);
    }

    public async Task DeactivateAsync(string id, CancellationToken cancellationToken = default)
    {
        Tenant tenantResult = await GetTenantInfoAsync(id, cancellationToken);

        if (!tenantResult.IsActive)
        {
            throw new DomainException("Tenant is already deactivated.");
        }

        tenantResult.Deactivate();

        _context.Tenants.Update(tenantResult);

        await _context.SaveChangesAsync(cancellationToken);

        await InvalidateTenantListCacheAsync(cancellationToken);
    }

    public async Task UpdateSubscriptionAsync(string id, DateTime extendedExpiryDate, CancellationToken cancellationToken = default)
    {
        Tenant tenantResult = await GetTenantInfoAsync(id, cancellationToken);

        try
        {
            tenantResult.SetValidity(extendedExpiryDate);
        }
        catch (Exception ex)
        {
            throw new InternalServerException(ex.Message);
        }

        _context.Tenants.Update(tenantResult);

        await _context.SaveChangesAsync(cancellationToken);
        
        await InvalidateTenantListCacheAsync(cancellationToken);
    }

    public async Task<bool> ExistsWithIdAsync(string id)
    {
        return await _context.Tenants.FirstOrDefaultAsync(x => x.Id == id) is not null;
    }

    public async Task InvalidateTenantListCacheAsync(CancellationToken cancellationToken = default)
    {
        await _cacheService.RemoveAsync(_cacheKeyService.GetCacheKey(IdentityConfiguration.IdentityClaimTypes.Tenant, "list", false), cancellationToken);
    }

    private async Task<Tenant> GetTenantInfoAsync(string id, CancellationToken cancellationToken = default)
    {
        Tenant? tenant = await _context.Tenants.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (tenant is null)
        {
            throw new DomainException(_localizer["record.notFoundWithGivenValueAndIn", nameof(Tenant), id]);
        }

        return tenant;
    }
}
