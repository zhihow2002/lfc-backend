using ClaimsPlugin.Domain.Models;

namespace ClaimsPlugin.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string userid);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
        void UpdateUser(User user);
        Task DeleteUserAsync(int id);
    }
}
