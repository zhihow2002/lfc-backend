namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Extensions;

internal static class DatabaseConnectionStringExtension
{
    internal static string GetString(string connectionDetail, string databaseName, string? tenantId = null)
    {
        return $"{connectionDetail};Database={databaseName}{(!string.IsNullOrWhiteSpace(tenantId) ? $"-{tenantId}" : string.Empty)}";
    }
}
