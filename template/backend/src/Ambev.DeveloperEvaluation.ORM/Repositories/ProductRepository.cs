using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class ProductRepository(DbContext context) : BaseORMRepository<Product>(context), IProductRepository
{
    /// <summary>
    /// Checks if a product is referenced in any order items
    /// </summary>
    /// <param name="productNumber">The product number to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the product is referenced in order items, false otherwise</returns>
    public async Task<bool> IsReferencedInOrderItemsAsync(int productNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Set<OrderItem>()
            .AnyAsync(oi => oi.ProductRefNumber == productNumber, cancellationToken);
    }
}
