using ClaimsPlugin.Shared.Foundation.Features.Auditing.Dtos;

namespace ClaimsPlugin.Shared.Foundation.Features.Auditing.Interfaces;

public interface IAuditService
{
    Task<List<AuditDto>> GetUserTrailsAsync(string userId);
}
