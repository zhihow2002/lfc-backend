using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

namespace ClaimsPlugin.Domain.Models
{
    public class UserRole : BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public string? RoleName { get; set; }
    }
}
