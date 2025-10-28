using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Command for creating a new cart
/// </summary>
public class CreateCartCommand : IRequest<CreateCartResult>
{
    /// <summary>
    /// Gets or sets the user number who owns the cart
    /// </summary>
    public int UserNumber { get; set; }

    /// <summary>
    /// Gets or sets the list of items to add to the cart
    /// </summary>
    public List<CreateCartItemCommand> Items { get; set; } = new();
}

/// <summary>
/// Command for creating a cart item
/// </summary>
public class CreateCartItemCommand
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



