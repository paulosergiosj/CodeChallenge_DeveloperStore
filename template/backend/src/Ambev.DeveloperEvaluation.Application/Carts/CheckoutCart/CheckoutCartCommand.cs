using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CheckoutCart;

/// <summary>
/// Command for checking out a cart
/// </summary>
public record CheckoutCartCommand : IRequest<CheckoutCartResult>
{
    /// <summary>
    /// The cart ID to checkout
    /// </summary>
    public Guid CartId { get; }

    /// <summary>
    /// Initializes a new instance of CheckoutCartCommand
    /// </summary>
    /// <param name="cartId">The cart ID to checkout</param>
    public CheckoutCartCommand(Guid cartId)
    {
        CartId = cartId;
    }
}

/// <summary>
/// Result for CheckoutCart operation
/// </summary>
public class CheckoutCartResult
{
    /// <summary>
    /// Gets or sets whether the checkout was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the cart ID
    /// </summary>
    public Guid CartId { get; set; }
}



