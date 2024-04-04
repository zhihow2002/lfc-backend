using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Infrastructure.Repositories
{
    internal abstract class BaseRepository<E> : IRepository<E>
        where E : AggregateRoot
    {
        private readonly BackendDbContext _context;
        private readonly DbSet<E> _dbset;

        public IUnitOfWork UnitOfWork => _context;

        public BaseRepository(BackendDbContext context)
        {
            _context = context;
            _dbset = _context.Set<E>();
        }

        public E Add(E entity)
        {
            return _dbset.Add(entity).Entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<E, bool>> expression)
        {
            return await _dbset.AnyAsync(expression);
        }

        public void Delete(E entity)
        {
            _dbset.Remove(entity);
        }

        public async Task<List<E>> GetAllAsync(List<Func<IQueryable<E>, IQueryable<E>>> includes, CancellationToken cancellationToken = default)
        {
            IQueryable<E> query = _dbset;

            foreach (var include in includes)
            {
                query = query.AppendQuery(include);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<E>> GetAsync(Expression<Func<E, bool>> expression, List<Func<IQueryable<E>, IQueryable<E>>> includes, CancellationToken cancellationToken = default)
        {
            IQueryable<E> query = _dbset.Where(expression);
            foreach (var include in includes)
            {
                query = query.AppendQuery(include);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<E> GetByIdAsync(Guid id, List<Func<IQueryable<E>, IQueryable<E>>> includes, CancellationToken cancellationToken = default)
        {
            IQueryable<E> query = _dbset;
            foreach (var include in includes)
            {
                query = query.AppendQuery(include);
            }
            return await query.FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        }

    }
}