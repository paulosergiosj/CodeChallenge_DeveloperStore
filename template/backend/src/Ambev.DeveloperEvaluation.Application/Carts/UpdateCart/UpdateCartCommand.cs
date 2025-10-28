using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

/// <summary>
/// Command for updating a cart
/// </summary>
public class UpdateCartCommand : IRequest<UpdateCartResult>
{
    /// <summary>
    /// Gets or sets the cart ID
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Gets or sets the list of items to update in the cart
    /// </summary>
    public List<UpdateCartItemCommand> Items { get; set; } = new();
}

/// <summary>
/// Command for updating a cart item
/// </summary>
public class UpdateCartItemCommand
{
    /// <summary>
    /// Gets or sets the product number
    /// </summary>
    public int ProductNumber { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the product
    /// </summary>
    public int Quantity { get; set; }
}



