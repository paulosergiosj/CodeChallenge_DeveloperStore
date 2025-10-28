using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

/// <summary>
/// Command for deleting a cart
/// </summary>
public record DeleteCartCommand : IRequest<DeleteCartResult>
{
    /// <summary>
    /// The cart ID to delete
    /// </summary>
    public Guid CartId { get; }

    /// <summary>
    /// Initializes a new instance of DeleteCartCommand
    /// </summary>
    /// <param name="cartId">The cart ID to delete</param>
    public DeleteCartCommand(Guid cartId)
    {
        CartId = cartId;
    }
}

/// <summary>
/// Result for DeleteCart operation
/// </summary>
public class DeleteCartResult
{
    /// <summary>
    /// Gets or sets whether the deletion was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}



