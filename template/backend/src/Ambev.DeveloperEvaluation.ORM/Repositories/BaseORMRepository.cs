using Ambev.DeveloperEvaluation.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class BaseORMRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly DbContext _context;

    public BaseORMRepository(DbContext context)
    {
        _context = context;
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

    public virtual async Task<IEnumerable<TEntity?>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
       => await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public virtual async Task UpdateAsync(TEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        _dbSet.Update(entity);
        await Task.CompletedTask;
    }

    public IQueryable<TEntity> Query()
        => _dbSet.AsQueryable();

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.AsNoTracking().AnyAsync(predicate, cancellationToken);
}
