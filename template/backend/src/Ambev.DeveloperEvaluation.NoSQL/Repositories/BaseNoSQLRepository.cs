using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.NoSQL.Repositories;

public class BaseNoSQLRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IMongoCollection<TEntity> _collection;

    public BaseNoSQLRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<TEntity>(collectionName);
    }

    public virtual async Task AddAsync(TEntity entity)
        => await _collection.InsertOneAsync(entity);

    public virtual void Remove(TEntity entity)
        => _collection.DeleteOne(Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id));

    public virtual async Task<TEntity?> GetByIdAsync(params object[] keys)
    {
        if (keys == null || keys.Length == 0 || !(keys[0] is Guid id))
            return null;

        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await _collection.AsQueryable().Where(predicate).FirstOrDefaultAsync(cancellationToken);

    public virtual async Task<IEnumerable<TEntity?>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await _collection.AsQueryable().Where(predicate).ToListAsync(cancellationToken);

    public virtual async Task UpdateAsync(TEntity entity)
    {
        if(entity is null)
            throw new ArgumentNullException(nameof(entity));

        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
        await _collection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = false });
    }

    public IQueryable<TEntity> Query()
        => _collection.AsQueryable();

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await _collection.AsQueryable().AnyAsync(predicate, cancellationToken);
}





