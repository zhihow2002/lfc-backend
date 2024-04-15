using ClaimsPlugin.Domain.Interfaces;
using ClaimsPlugin.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimsPlugin.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernidAsync(string userid)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userid)
                ?? throw new Exception("User not found.");
        }

        public async Task<User?> GetUserByUsernameAsync(string userid)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userid);
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return null;
        }

        public Task CreateUserAsync(User user)
        {
            return null;
        }

        public void UpdateUser(User user) { }

        public Task DeleteUserAsync(int id) { 
            return null;
        }

        public Task<User> GetUserByIdAsync(int id)
        {
            return null;
        }
    }
}
