using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

/// <summary>
/// Handler for processing DeleteCartCommand requests
/// </summary>
public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, DeleteCartResult>
{
    private readonly ICartRepository _cartRepository;

    /// <summary>
    /// Initializes a new instance of DeleteCartHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository instance</param>
    public DeleteCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    /// <summary>
    /// Handles the DeleteCartCommand request
    /// </summary>
    /// <param name="request">The DeleteCart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    public async Task<DeleteCartResult> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.CartId, cancellationToken)
            ?? throw new KeyNotFoundException($"Cart with ID {request.CartId} not found");

        if (!cart.CanDelete())
            throw new InvalidOperationException("Only active carts can be deleted");

        _cartRepository.Remove(cart);

        return new DeleteCartResult
        {
            Success = true,
            Message = "Cart deleted successfully"
        };
    }
}

