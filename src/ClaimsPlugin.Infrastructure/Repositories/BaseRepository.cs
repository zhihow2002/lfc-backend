using ClaimsPlugin.Application.Interfaces;
using ClaimsPlugin.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClaimsPlugin.Infrastructure.Repositories
{
    public abstract class BaseRepository<T>(DbContext context) : IRepository<T>
        where T : class
    {
        protected readonly DbContext _context = context;

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id) ?? default!;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
