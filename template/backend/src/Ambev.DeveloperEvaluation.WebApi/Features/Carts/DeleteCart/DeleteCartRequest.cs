namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;

/// <summary>
/// Request for deleting a cart
/// </summary>
public class DeleteCartRequest
{
    /// <summary>
    /// The cart ID to delete
    /// </summary>
    public Guid Id { get; set; }
}



