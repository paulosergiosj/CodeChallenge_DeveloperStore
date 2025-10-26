using Ambev.DeveloperEvaluation.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class BaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(DbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity)
        => await _dbSet.AddAsync(entity);

    public virtual void Remove(TEntity entity)
        => _dbSet.Remove(entity);

    public virtual async Task<TEntity?> GetByIdAsync(params object[] keys)
        => await _dbSet.FindAsync(keys);

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
           => await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);

    public IQueryable<TEntity> Query()
        => _dbSet.AsQueryable();
}
