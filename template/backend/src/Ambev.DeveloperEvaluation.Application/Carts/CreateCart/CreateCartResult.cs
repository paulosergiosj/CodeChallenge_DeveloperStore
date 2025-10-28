namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Result for creating a cart
/// </summary>
public class CreateCartResult
{
    /// <summary>
    /// Gets or sets the cart ID
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the user ID who owns the cart
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the list of items in the cart
    /// </summary>
    public List<CreateCartItemResult> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the total amount of the cart
    /// </summary>
    public decimal TotalAmount { get; set; }
}

/// <summary>
/// Result for a cart item
/// </summary>
public class CreateCartItemResult
{
    /// <summary>
    /// Gets or sets the cart item ID
    /// </summary>
    public Guid CartItemId { get; set; }

    /// <summary>
    /// Gets or sets the product ID
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product number
    /// </summary>
    public int ProductNumber { get; set; }

    /// <summary>
    /// Gets or sets the unit price
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the discount amount
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item (with discount)
    /// </summary>
    public decimal TotalAmount { get; set; }
}





