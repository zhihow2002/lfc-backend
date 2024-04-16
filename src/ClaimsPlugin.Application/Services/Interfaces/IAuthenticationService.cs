using System.Security.Claims;
using System.Threading.Tasks;
using ClaimsPlugin.Domain.Models;

namespace ClaimsPlugin.Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        string GenerateJwtToken(User user);
        string GenerateRefreshToken(User user);
      //  Task<bool> ValidateToken(string token, out ClaimsPrincipal principal);
        bool ValidateToken(string token, out ClaimsPrincipal principal);
    }
}
