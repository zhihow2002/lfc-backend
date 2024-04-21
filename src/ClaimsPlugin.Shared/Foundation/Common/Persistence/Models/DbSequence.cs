namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

public class DbSequence
{
    public string SequenceName { get; private init; }

    public DbSequence(string sequenceName)
    {
        SequenceName = sequenceName;
    }
}
