//using Ardalis.Specification;
//using Ardalis.Specification.EntityFrameworkCore;
//using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;
//using ClaimsPlugin.Shared.Foundation.Features.DomainDrivenDesign.Interfaces;
//using Mapster;
//namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;
//public class BaseRepository<TDbContext, T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
//    where TDbContext : BaseDatabaseContext, IDatabaseContext
//    where T : class, IAggregateRoot
//{
//    private readonly BaseDatabaseContext _dbContext;
//    public BaseRepository(TDbContext dbContext)
//        : base(dbContext, new BaseSpecificationEvaluator())
//    {
//        _dbContext = dbContext;
//    }
//    public override async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
//    {
//        _dbContext.Set<T>().Add(entity);
//        await _dbContext.SaveChangesAsync(cancellationToken);
//        return entity;
//    }
//    public override async Task<IEnumerable<T>> AddRangeAsync(
//        IEnumerable<T> entities,
//        CancellationToken cancellationToken = default
//    )
//    {
//        IEnumerable<T> aggregateRoots = entities.ToList();
//        _dbContext.Set<T>().AddRange(aggregateRoots);
//        await _dbContext.SaveChangesAsync(cancellationToken);
//        return aggregateRoots;
//    }
//    public override async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
//    {
//        _dbContext.Set<T>().Remove(entity);
//        await _dbContext.SaveChangesAsync(cancellationToken);
//    }
//    public override async Task DeleteRangeAsync(
//        IEnumerable<T> entities,
//        CancellationToken cancellationToken = new()
//    )
//    {
//        IEnumerable<T> aggregateRoots = entities.ToList();
//        _dbContext.Set<T>().RemoveRange(aggregateRoots);
//        await _dbContext.SaveChangesAsync(cancellationToken);
//    }
//    public override async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
//    {
//        _dbContext.Set<T>().Update(entity);
//        await _dbContext.SaveChangesAsync(cancellationToken);
//    }
//    public override async Task<IEnumerable<T>> UpdateRangeAsync(
//        IEnumerable<T> entities,
//        CancellationToken cancellationToken = default
//    )
//    {
//        IEnumerable<T> aggregateRoots = entities.ToList();
//        _dbContext.Set<T>().UpdateRange(aggregateRoots);
//        await _dbContext.SaveChangesAsync(cancellationToken);
//        return aggregateRoots;
//    }
//    protected override IQueryable<TResult> ApplySpecification<TResult>(
//        ISpecification<T, TResult> specification
//    )
//    {
//        return base.ApplySpecification(specification, false).Select(x => x.Adapt<TResult>());
//    }
//}
