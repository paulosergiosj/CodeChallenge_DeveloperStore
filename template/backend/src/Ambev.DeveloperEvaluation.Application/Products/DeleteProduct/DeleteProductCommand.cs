using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Command for deleting a product
/// </summary>
public class DeleteProductCommand : IRequest<DeleteProductResult>
{
    /// <summary>
    /// The product number (unique identifier) of the product to delete
    /// </summary>
    public int ProductNumber { get; set; }
}






