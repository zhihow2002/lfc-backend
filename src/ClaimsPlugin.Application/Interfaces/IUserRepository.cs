using ClaimsPlugin.Domain.Models;

namespace ClaimsPlugin.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUserIdAsync(string userid);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string userid);
        Task<User> GetUserByIdAsync(int id);
    }
}
