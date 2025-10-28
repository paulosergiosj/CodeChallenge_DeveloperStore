namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

/// <summary>
/// Request for updating a cart
/// </summary>
public class UpdateCartRequest
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
    /// Gets or sets the list of products to update in the cart
    /// </summary>
    public List<CartProductUpdateRequest> Products { get; set; } = new();
}

/// <summary>
/// Request for a cart product update
/// </summary>
public class CartProductUpdateRequest
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



