using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICartRepository : IBaseRepository<Cart>
{
    /// <summary>
    /// Gets the active cart for a specific user
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The active cart if found, null otherwise</returns>
    Task<Cart?> GetActiveCartByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated carts with sorting
    /// </summary>
    /// <param name="pageNumber">The page number</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="sortString">The sort string (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Tuple containing the carts and total count</returns>
    Task<(IEnumerable<Cart> Items, long TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? sortString = null,
        CancellationToken cancellationToken = default);
}
