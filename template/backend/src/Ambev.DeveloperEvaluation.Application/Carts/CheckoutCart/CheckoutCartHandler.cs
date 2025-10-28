using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.Application.Carts.CheckoutCart;

/// <summary>
/// Handler for processing CheckoutCartCommand requests
/// </summary>
public class CheckoutCartHandler : IRequestHandler<CheckoutCartCommand, CheckoutCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IBus _bus;

    /// <summary>
    /// Initializes a new instance of CheckoutCartHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository instance</param>
    public CheckoutCartHandler(
        ICartRepository cartRepository,
        IBus bus)
    {
        _cartRepository = cartRepository;
        _bus = bus;
    }

    /// <summary>
    /// Handles the CheckoutCartCommand request
    /// </summary>
    /// <param name="request">The CheckoutCart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the checkout operation</returns>
    public async Task<CheckoutCartResult> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.CartId, cancellationToken)
            ?? throw new KeyNotFoundException($"Cart with ID {request.CartId} not found");

        if (!cart.CanBeCheckedOut())
            throw new InvalidOperationException("Only active carts with items can be checked out");

        cart.SetCheckedOut();

        await _cartRepository.UpdateAsync(cart);

        await _bus.Send(new Domain.Events.CartCheckedOutEventMessage(cart.Id));

        return new CheckoutCartResult
        {
            Success = true,
            Message = "Cart checked out successfully",
            CartId = cart.Id
        };
    }
}



