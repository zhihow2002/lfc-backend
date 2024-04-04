using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimsPlugin.Domain;
using ClaimsPlugin.Domain.SharedKernel;

namespace ClaimsPlugin.Repository
{
    public interface IRepository<E> where E : AggregateRoot
    {
         IUnitOfWork UnitOfWork { get; }

        E Add(E entity);
        Task<List<E>> GetAllAsync(List<Func<IQueryable<E>, IQueryable<E>>> includes, CancellationToken cancellationToken = default);
        Task<List<E>> GetAsync(Expression<Func<E, bool>> expression, List<Func<IQueryable<E>, IQueryable<E>>> includes, CancellationToken cancellationToken = default);
        Task<E> GetByIdAsync(Guid id, List<Func<IQueryable<E>, IQueryable<E>>> includes, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<E, bool>> expression);
        void Delete(E entity);
    }
}