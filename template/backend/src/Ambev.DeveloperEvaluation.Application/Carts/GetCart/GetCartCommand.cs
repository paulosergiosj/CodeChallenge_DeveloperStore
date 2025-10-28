using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

/// <summary>
/// Command for retrieving a cart by ID
/// </summary>
public record GetCartCommand : IRequest<GetCartResult>
{
    /// <summary>
    /// The cart ID to retrieve
    /// </summary>
    public Guid CartId { get; }

    /// <summary>
    /// Initializes a new instance of GetCartCommand
    /// </summary>
    /// <param name="cartId">The cart ID to retrieve</param>
    public GetCartCommand(Guid cartId)
    {
        CartId = cartId;
    }
}



