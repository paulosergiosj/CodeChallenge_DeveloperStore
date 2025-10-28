using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        /// <summary>
        /// Checks if a product is referenced in any order items
        /// </summary>
        /// <param name="productNumber">The product number to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the product is referenced in order items, false otherwise</returns>
        Task<bool> IsReferencedInOrderItemsAsync(int productNumber, CancellationToken cancellationToken = default);
    }
}
