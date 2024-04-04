using ClaimsPlugin.Shared.Common.Models;

namespace ClaimsPlugin.Infrastructure.Models
{
    public class User : BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
    }
}
