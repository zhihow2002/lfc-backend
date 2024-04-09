using ClaimsPlugin.Domain.Models;

namespace ClaimsPlugin.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsernidAsync(string userid);
        Task<User?> GetUserByUsernameAsync(string userid);
    }
}
