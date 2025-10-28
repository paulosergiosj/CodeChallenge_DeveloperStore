using Ambev.DeveloperEvaluation.Domain.Common;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);
    void Remove(TEntity entity);
    Task<TEntity?> GetByIdAsync(params object[] keys);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity?>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity);
    IQueryable<TEntity> Query();
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}
