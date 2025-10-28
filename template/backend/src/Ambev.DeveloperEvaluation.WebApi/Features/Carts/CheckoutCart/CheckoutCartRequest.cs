namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CheckoutCart;

/// <summary>
/// Request for checking out a cart
/// </summary>
public class CheckoutCartRequest
{
    /// <summary>
    /// The cart ID to checkout
    /// </summary>
    public Guid Id { get; set; }
}



