namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Constants;

public static class SchemaNames
{
    public const string Auditing = nameof(Auditing);
    public const string Identity = nameof(Identity);
    public const string MultiTenancy = nameof(MultiTenancy);

    // Name all the schema for the common table here.
    // It will be generated in database like this "YourSchemaName.YourTableName" instead of using default "dbo.YourTableName".
}
