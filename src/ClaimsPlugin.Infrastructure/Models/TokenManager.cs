using Newtonsoft.Json;

namespace ClaimsPlugin.Infrastructure.Models
{
    public class TokenManager
    {
       
        public Guid Id { get; set; }
        public string? TokenId { get; set; }
        public string? TokenKey { get; set; }
        public DateTime IssuedOn { get; set; }
        public string? UserId { get; set; }
        public string? MemberType { get; set; }
        public string? Recsts { get; set; }
        public string? Status { get; set; }
        public string? InternalKey { get; set; }
        public string? AccessToken { get; set; }
        public int AccessTokenExpiry { get; set; }
        public string? RefreshToken { get; set; }
        public string? LoginType { get; set; }
        public int RefreshTokenValidity { get; set; }
        public string? InsertBy { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
