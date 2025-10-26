using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Query for retrieving a product by its unique identifier.
/// </summary>
public class GetProductQuery : IRequest<GetProductResult>
{
    /// <summary>
    /// Gets or sets the product number (unique identifier).
    /// </summary>
    public int ProductNumber { get; set; }
}

