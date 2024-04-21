using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

public abstract class BaseDataTransferObject : IDataTransferObject
{
    public string CreatedBy { get; set; } = default!;
}
