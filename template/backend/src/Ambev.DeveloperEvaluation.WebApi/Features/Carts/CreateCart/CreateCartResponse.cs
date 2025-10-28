namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// Response for creating a cart
/// </summary>
public class CreateCartResponse
{
    /// <summary>
    /// Gets or sets the cart ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user ID who owns the cart
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the cart date
    /// </summary>
    public string Date { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of products in the cart
    /// </summary>
    public List<CreateCartProductResponse> Products { get; set; } = new();
}

/// <summary>
/// Response for a cart product
/// </summary>
public class CreateCartProductResponse
{
    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product
    /// </summary>
    public int Quantity { get; set; }
}

