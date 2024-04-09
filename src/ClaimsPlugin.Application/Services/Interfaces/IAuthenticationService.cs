
using ClaimsPlugin.Domain.Models;

namespace ClaimsPlugin.Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateAsync(string username, string password);
        string GenerateJwtToken(User user);
    }
}
