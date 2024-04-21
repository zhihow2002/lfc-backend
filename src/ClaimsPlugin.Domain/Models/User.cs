using ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
using ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.Interfaces;

namespace ClaimsPlugin.Domain.Models
{
    public class User : BaseAuditableEntity, IAggregateRoot
    {
        public Guid Id { get; set; } = Guid.Empty!;
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public virtual ICollection<TokenManager> Tokens { get; set; } = new List<TokenManager>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public User(string userName, string email, string passwordHash)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
        }

        public void UpdateUser(string userName, string email, string passwordHash)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}
