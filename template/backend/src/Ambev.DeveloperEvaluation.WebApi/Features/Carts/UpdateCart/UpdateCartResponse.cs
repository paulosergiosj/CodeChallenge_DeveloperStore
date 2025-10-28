namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

/// <summary>
/// Response for updating a cart
/// </summary>
public class UpdateCartResponse
{
    /// <summary>
    /// The cart ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The user number who owns the cart
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// The date when the cart was created
    /// </summary>
    public string Date { get; set; } = string.Empty;

    /// <summary>
    /// List of products in the cart
    /// </summary>
    public List<UpdateCartProductResponse> Products { get; set; } = new();
}

/// <summary>
/// Response for a cart product
/// </summary>
public class UpdateCartProductResponse
{
    /// <summary>
    /// The product ID (ProductNumber)
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// The quantity
    /// </summary>
    public int Quantity { get; set; }
}



