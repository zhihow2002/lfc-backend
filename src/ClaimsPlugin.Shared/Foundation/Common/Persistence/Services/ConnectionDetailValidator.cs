using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Services;
internal class ConnectionDetailValidator : IConnectionDetailValidator
{
    private readonly DatabaseSettings _dbSettings;
    private readonly ILogger<ConnectionDetailValidator> _logger;
    public ConnectionDetailValidator(IOptions<DatabaseSettings> dbSettings, ILogger<ConnectionDetailValidator> logger)
    {
        _dbSettings = dbSettings.Value;
        _logger = logger;
    }
    public bool TryValidate(string connectionDetail)
    {
        try
        {
            return new SqlConnectionStringBuilder(connectionDetail) is
            {
            } builder;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Connection String Validation Exception : {ex.Message}");
            return false;
        }
    }
}
