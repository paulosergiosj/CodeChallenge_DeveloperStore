namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

/// <summary>
/// Request for getting a cart by ID
/// </summary>
public class GetCartRequest
{
    /// <summary>
    /// The cart ID to retrieve
    /// </summary>
    public Guid Id { get; set; }
}



