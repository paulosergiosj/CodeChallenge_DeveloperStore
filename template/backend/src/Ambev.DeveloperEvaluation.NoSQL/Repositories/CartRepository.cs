using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.NoSQL.Repositories;
public class CartRepository : BaseNoSQLRepository<Cart>, ICartRepository
{
    public CartRepository(IMongoDatabase database) : base(database, "carts")
    {
    }

    public async Task<Cart?> GetActiveCartByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cart>.Filter.And(
        Builders<Cart>.Filter.Eq(c => c.UserRefId, userId),
        Builders<Cart>.Filter.Eq(c => c.Status, CartStatus.Active)
    );
        var cart = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return cart;
    }

    /// <summary>
    /// Gets paginated carts with sorting
    /// </summary>
    public async Task<(IEnumerable<Cart> Items, long TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? sortString = null,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cart>.Filter.Empty;
        var findOptions = _collection.Find(filter);

        if (!string.IsNullOrWhiteSpace(sortString))
        {
            findOptions = ApplySorting(findOptions, sortString);
        }

        var totalCount = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var carts = await findOptions
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return (carts, totalCount);
    }

    private static IFindFluent<Cart, Cart> ApplySorting(IFindFluent<Cart, Cart> findOptions, string sortString)
    {
        var sortDefinitions = new List<SortDefinition<Cart>>();

        var parts = sortString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var part in parts)
        {
            var segments = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length < 1) continue;

            var field = segments[0].ToLower();
            var direction = segments.Length > 1 && segments[1].ToLower() == "desc" 
                ? SortDirection.Descending 
                : SortDirection.Ascending;

            SortDefinition<Cart> sortDef = field switch
            {
                "id" => direction == SortDirection.Ascending ? Builders<Cart>.Sort.Ascending("_id") : Builders<Cart>.Sort.Descending("_id"),
                "userid" => direction == SortDirection.Ascending ? Builders<Cart>.Sort.Ascending("UserRefId") : Builders<Cart>.Sort.Descending("UserRefId"),
                "createdat" => direction == SortDirection.Ascending ? Builders<Cart>.Sort.Ascending("CreatedAt") : Builders<Cart>.Sort.Descending("CreatedAt"),
                "status" => direction == SortDirection.Ascending ? Builders<Cart>.Sort.Ascending("Status") : Builders<Cart>.Sort.Descending("Status"),
                _ => direction == SortDirection.Ascending ? Builders<Cart>.Sort.Ascending(field) : Builders<Cart>.Sort.Descending(field)
            };

            sortDefinitions.Add(sortDef);
        }

        return sortDefinitions.Any() 
            ? findOptions.Sort(Builders<Cart>.Sort.Combine(sortDefinitions))
            : findOptions;
    }
}
