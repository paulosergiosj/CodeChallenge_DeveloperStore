using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Messaging.EventHandlers;

public class CartCheckedOutEventHandler : IHandleMessages<CartCheckedOutEventMessage>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartRepository _cartRepository;

    public CartCheckedOutEventHandler(
        IUnitOfWork unitOfWork,
        ICartRepository cartRepository)
    {
        _unitOfWork = unitOfWork;
        _cartRepository = cartRepository;
    }

    public async Task Handle(CartCheckedOutEventMessage message)
    {
        Console.WriteLine($"CartCheckedOutEventHandler: Processing cart {message.CartId}");

        var cart = await _cartRepository.GetByIdAsync(message.CartId)
            ?? throw new KeyNotFoundException($"Cart with ID {message.CartId} not found");

        Console.WriteLine($"CartCheckedOutEventHandler: Cart found with status {cart.Status}");

        if (!cart.CanBeFinalized())
            throw new InvalidOperationException($"CartCheckedOutEventHandler: Cart cannot be finalized, current status: {cart.Status}");

        // Validate and clean up cart items - remove products that no longer exist
        await ValidateAndCleanCartItems(cart);

        // Check if cart still has items after cleanup
        if (cart.Items.Count == 0)
        {
            Console.WriteLine($"CartCheckedOutEventHandler: Cart {cart.Id} has no valid items after cleanup, skipping order creation");
            await FinalizeCart(cart);
            return;
        }

        var branch = await _unitOfWork.Branches.GetFirstAvailableAsync()
            ?? throw new InvalidOperationException("No branches available in the system");

        var order = Order.Create(cart, cart.UserRefId, branch.Id);

        await FinalizeCart(cart);

        await _unitOfWork.Orders.AddAsync(order);

        var result = await _unitOfWork.CommitAsync();

        Console.WriteLine($"Order {order.Id} created from cart {cart.Id} with total amount {order.TotalAmount} for branch {branch.Name}");
    }

    /// <summary>
    /// Validates cart items and removes products that no longer exist in the database
    /// </summary>
    /// <param name="cart">The cart to validate and clean</param>
    private async Task ValidateAndCleanCartItems(Cart cart)
    {
        var itemsToRemove = new List<CartItem>();
        
        foreach (var cartItem in cart.Items)
        {
            // Check if product still exists in PostgreSQL
            var productExists = await _unitOfWork.Products.AnyAsync(
                p => p.ProductNumber == cartItem.ProductRefNumber);
            
            if (!productExists)
            {
                Console.WriteLine($"CartCheckedOutEventHandler: Product {cartItem.ProductRefNumber} no longer exists, removing from cart");
                itemsToRemove.Add(cartItem);
            }
        }

        // Remove invalid items from cart
        foreach (var item in itemsToRemove)
        {
            cart.RemoveItem(item.ProductRefNumber);
        }

        // Update cart in repository if items were removed
        if (itemsToRemove.Count > 0)
        {
            await _cartRepository.UpdateAsync(cart);
            Console.WriteLine($"CartCheckedOutEventHandler: Removed {itemsToRemove.Count} invalid items from cart {cart.Id}");
        }
    }

    private async Task FinalizeCart(Cart cart)
    {
        cart.SetFinalized();
        await _cartRepository.UpdateAsync(cart);
    }
}
