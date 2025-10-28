using Ambev.DeveloperEvaluation.Application.Carts.CheckoutCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CheckoutCartHandler tests.
/// </summary>
public static class CheckoutCartHandlerTestData
{
    /// <summary>
    /// Generates a valid CheckoutCartCommand with randomized data.
    /// </summary>
    /// <returns>A valid CheckoutCartCommand with randomly generated data.</returns>
    public static CheckoutCartCommand GenerateValidCommand()
    {
        var faker = new Faker();
        
        return new CheckoutCartCommand(faker.Random.Guid());
    }

    /// <summary>
    /// Generates a valid CheckoutCartCommand with specific cart ID.
    /// </summary>
    /// <param name="cartId">The cart ID to use</param>
    /// <returns>A valid CheckoutCartCommand with the specified cart ID.</returns>
    public static CheckoutCartCommand GenerateValidCommand(Guid cartId)
    {
        return new CheckoutCartCommand(cartId);
    }

    /// <summary>
    /// Generates an invalid CheckoutCartCommand for testing negative scenarios.
    /// </summary>
    /// <returns>An invalid CheckoutCartCommand for testing validation errors.</returns>
    public static CheckoutCartCommand GenerateInvalidCommand()
    {
        return new CheckoutCartCommand(Guid.Empty); // Invalid empty GUID
    }

    /// <summary>
    /// Generates a valid Cart entity for testing checkout scenarios.
    /// </summary>
    /// <param name="id">The ID to use</param>
    /// <param name="userId">The user ID to use</param>
    /// <param name="status">The status to use</param>
    /// <param name="itemCount">The number of items to add</param>
    /// <returns>A valid Cart entity.</returns>
    public static Cart GenerateValidCart(Guid? id = null, Guid? userId = null, CartStatus? status = null, int itemCount = 3)
    {
        var faker = new Faker();
        var cartUserId = userId ?? faker.Random.Guid();
        
        var cart = Cart.Create(cartUserId);
        
        // Set ID using reflection if provided
        if (id.HasValue)
        {
            typeof(Cart).GetProperty("Id")?.SetValue(cart, id.Value);
        }

        // Add items to cart
        for (int i = 0; i < itemCount; i++)
        {
            cart.AddItem(
                productRefId: faker.Random.Guid(),
                productRefNumber: faker.Random.Int(1, 1000),
                unitPrice: faker.Random.Decimal(1, 100),
                quantity: faker.Random.Int(1, 5)
            );
        }

        // Set status using reflection if different from default
        if (status.HasValue)
        {
            typeof(Cart).GetProperty("Status")?.SetValue(cart, status.Value);
        }

        return cart;
    }

    /// <summary>
    /// Generates an active cart with items for testing checkout scenarios.
    /// </summary>
    /// <param name="id">The ID to use</param>
    /// <param name="userId">The user ID to use</param>
    /// <param name="itemCount">The number of items to add</param>
    /// <returns>An active Cart entity with items.</returns>
    public static Cart GenerateActiveCartWithItems(Guid? id = null, Guid? userId = null, int itemCount = 3)
    {
        return GenerateValidCart(id, userId, CartStatus.Active, itemCount);
    }

    /// <summary>
    /// Generates an empty cart for testing negative scenarios.
    /// </summary>
    /// <param name="id">The ID to use</param>
    /// <param name="userId">The user ID to use</param>
    /// <returns>An empty Cart entity.</returns>
    public static Cart GenerateEmptyCart(Guid? id = null, Guid? userId = null)
    {
        return GenerateValidCart(id, userId, CartStatus.Active, 0);
    }

    /// <summary>
    /// Generates a checked out cart for testing negative scenarios.
    /// </summary>
    /// <param name="id">The ID to use</param>
    /// <param name="userId">The user ID to use</param>
    /// <param name="itemCount">The number of items to add</param>
    /// <returns>A checked out Cart entity.</returns>
    public static Cart GenerateCheckedOutCart(Guid? id = null, Guid? userId = null, int itemCount = 3)
    {
        return GenerateValidCart(id, userId, CartStatus.CheckedOut, itemCount);
    }

    /// <summary>
    /// Generates a finalized cart for testing negative scenarios.
    /// </summary>
    /// <param name="id">The ID to use</param>
    /// <param name="userId">The user ID to use</param>
    /// <param name="itemCount">The number of items to add</param>
    /// <returns>A finalized Cart entity.</returns>
    public static Cart GenerateFinalizedCart(Guid? id = null, Guid? userId = null, int itemCount = 3)
    {
        return GenerateValidCart(id, userId, CartStatus.Finalized, itemCount);
    }

    /// <summary>
    /// Generates a valid CheckoutCartResult for testing.
    /// </summary>
    /// <param name="cartId">The cart ID to use</param>
    /// <param name="success">The success status to use</param>
    /// <param name="message">The message to use</param>
    /// <returns>A valid CheckoutCartResult.</returns>
    public static CheckoutCartResult GenerateValidResult(Guid? cartId = null, bool success = true, string? message = null)
    {
        var faker = new Faker();
        
        return new CheckoutCartResult
        {
            Success = success,
            Message = message ?? (success ? "Cart checked out successfully" : "Failed to checkout cart"),
            CartId = cartId ?? faker.Random.Guid()
        };
    }
}
