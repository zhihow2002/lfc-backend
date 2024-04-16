using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<User> GetByUserIdAsync(string userid)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userid)
                ?? throw new Exception("User not found.");
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == username);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string userid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userid);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User not found.");
            }
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id)
                ?? throw new Exception("User not found.");
        }
    }
}
