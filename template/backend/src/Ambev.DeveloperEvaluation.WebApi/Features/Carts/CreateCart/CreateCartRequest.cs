namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// Request for creating a new cart
/// </summary>
public class CreateCartRequest
{
    /// <summary>
    /// Gets or sets the user ID who owns the cart
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the cart date
    /// </summary>
    public string? Date { get; set; }

    /// <summary>
    /// Gets or sets the list of products to add to the cart
    /// </summary>
    public List<CartProductRequest> Products { get; set; } = new();
}

/// <summary>
/// Request for a cart product
/// </summary>
public class CartProductRequest
{
    /// <summary>
    /// Gets or sets the product ID (productNumber)
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product
    /// </summary>
    public int Quantity { get; set; }
}

