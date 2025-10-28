using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Branch entity operations
/// </summary>
public interface IBranchRepository : IBaseRepository<Branch>
{
    /// <summary>
    /// Gets the first available branch from the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The first available branch if found, null otherwise</returns>
    Task<Branch?> GetFirstAvailableAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a branch is referenced in any orders
    /// </summary>
    /// <param name="branchId">The branch ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the branch is referenced in orders, false otherwise</returns>
    Task<bool> IsReferencedInOrdersAsync(Guid branchId, CancellationToken cancellationToken = default);
}
