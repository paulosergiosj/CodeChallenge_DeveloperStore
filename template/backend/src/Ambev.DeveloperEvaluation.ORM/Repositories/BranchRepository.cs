using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IBranchRepository using Entity Framework Core
/// </summary>
public class BranchRepository : BaseORMRepository<Branch>, IBranchRepository
{
    public BranchRepository(DbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets the first available branch from the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The first available branch if found, null otherwise</returns>
    public async Task<Branch?> GetFirstAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .OrderBy(b => b.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Checks if a branch is referenced in any orders
    /// </summary>
    /// <param name="branchId">The branch ID to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the branch is referenced in orders, false otherwise</returns>
    public async Task<bool> IsReferencedInOrdersAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Order>()
            .AnyAsync(o => o.BranchRefId == branchId, cancellationToken);
    }
}
