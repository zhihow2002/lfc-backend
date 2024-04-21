namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;

public interface IDatabaseContext
{
    bool IsSeedOperation { get; }
    public Task StartSeedOperationAsync(Func<Task> action);
}
