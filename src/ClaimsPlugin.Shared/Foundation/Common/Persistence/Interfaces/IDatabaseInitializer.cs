using ClaimsPlugin.Shared.Foundation.Common.Persistence.Constants;
namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
internal interface IDatabaseInitializer
{
    internal Task InitializeAsync(DatabaseInitializeMode mode, CancellationToken cancellationToken);
}
