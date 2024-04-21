using ClaimsPlugin.Shared.Foundation.Common.Persistence.Constants;

namespace ClaimsPlugin.Shared.Foundation.Features.MultiTenancy.Interfaces;

internal interface IMultiTenancyDatabaseInitializer
{
    internal Task InitializeAsync(DatabaseInitializeMode mode, CancellationToken cancellationToken);
}
