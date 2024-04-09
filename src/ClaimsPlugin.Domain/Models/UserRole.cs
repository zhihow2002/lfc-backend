using ClaimsPlugin.Shared.Common.Models;

namespace ClaimsPlugin.Domain.Models
{
    public class UserRole : BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public string? RoleName { get; set; }
    }
}
