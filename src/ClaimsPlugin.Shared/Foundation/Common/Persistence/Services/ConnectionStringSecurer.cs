using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Services;
public class ConnectionStringSecurer : IConnectionStringSecurer
{
    private const string HiddenValueDefault = "*******";
    private readonly DatabaseSettings _dbSettings;
    public ConnectionStringSecurer(IOptions<DatabaseSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }
    public string? MakeSecure(string? connectionString)
    {
        if (connectionString == null || string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }
        return MakeSecureSqlConnectionString(connectionString);
    }
    private string MakeSecureSqlConnectionString(string connectionString)
    {
        SqlConnectionStringBuilder builder = new(connectionString);
        if (!string.IsNullOrEmpty(builder.Password) || !builder.IntegratedSecurity)
        {
            builder.Password = HiddenValueDefault;
        }
        if (!string.IsNullOrEmpty(builder.UserID) || !builder.IntegratedSecurity)
        {
            builder.UserID = HiddenValueDefault;
        }
        return builder.ToString();
    }
}
