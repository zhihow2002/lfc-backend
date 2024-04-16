using ClaimsPlugin.Shared.Common.Models;

namespace ClaimsPlugin.Domain.Models
{
    public class User : BaseAuditableEntity
    {
        public Guid Id { get; set; } = Guid.Empty!;
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public virtual ICollection<TokenManager> Tokens { get; set; } = new List<TokenManager>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
