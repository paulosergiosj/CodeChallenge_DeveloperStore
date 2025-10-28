namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

/// <summary>
/// Result for UpdateCart operation
/// </summary>
public class UpdateCartResult
{
    /// <summary>
    /// The cart ID
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// The user ID who owns the cart
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The user number who owns the cart
    /// </summary>
    public int UserNumber { get; set; }

    /// <summary>
    /// The date when the cart was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// List of items in the cart
    /// </summary>
    public List<UpdateCartItemResult> Items { get; set; } = new();

    /// <summary>
    /// The total amount of the cart
    /// </summary>
    public decimal TotalAmount { get; set; }
}

/// <summary>
/// Result for a cart item
/// </summary>
public class UpdateCartItemResult
{
    /// <summary>
    /// The cart item ID
    /// </summary>
    public Guid CartItemId { get; set; }

    /// <summary>
    /// The product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The product number
    /// </summary>
    public int ProductNumber { get; set; }

    /// <summary>
    /// The unit price
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The discount amount
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// The total amount for this item
    /// </summary>
    public decimal TotalAmount { get; set; }
}



