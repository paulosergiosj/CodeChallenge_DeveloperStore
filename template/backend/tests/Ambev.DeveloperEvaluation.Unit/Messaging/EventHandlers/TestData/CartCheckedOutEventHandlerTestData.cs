using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Messaging.EventHandlers.TestData;

/// <summary>
/// Provides methods for generating test data for CartCheckedOutEventHandler tests.
/// </summary>
public static class CartCheckedOutEventHandlerTestData
{
    /// <summary>
    /// Generates a valid CartCheckedOutEventMessage with randomized data.
    /// </summary>
    /// <returns>A valid CartCheckedOutEventMessage with randomly generated data.</returns>
    public static CartCheckedOutEventMessage GenerateValidEventMessage()
    {
        return new CartCheckedOutEventMessage(Guid.NewGuid());
    }

    /// <summary>
    /// Generates a valid CartCheckedOutEventMessage with specific cart ID.
    /// </summary>
    /// <param name="cartId">The cart ID to use</param>
    /// <returns>A valid CartCheckedOutEventMessage with the specified cart ID.</returns>
    public static CartCheckedOutEventMessage GenerateValidEventMessage(Guid cartId)
    {
        return new CartCheckedOutEventMessage(cartId);
    }

    /// <summary>
    /// Generates a cart with CheckedOut status for testing.
    /// </summary>
    /// <param name="cartId">The cart ID to use</param>
    /// <param name="userId">The user ID to use</param>
    /// <param name="itemCount">Number of items to add to the cart</param>
    /// <returns>A cart with CheckedOut status.</returns>
    public static Cart GenerateCheckedOutCart(Guid? cartId = null, Guid? userId = null, int itemCount = 3)
    {
        var faker = new Faker();
        var cart = Cart.Create(userId ?? Guid.NewGuid());
        
        // Set the ID using reflection
        typeof(Cart).GetProperty("Id")?.SetValue(cart, cartId ?? Guid.NewGuid());
        
        // Add items to the cart
        for (int i = 0; i < itemCount; i++)
        {
            cart.AddItem(
                productRefId: Guid.NewGuid(),
                productRefNumber: faker.Random.Int(1, 1000),
                unitPrice: faker.Finance.Amount(1, 100),
                quantity: faker.Random.Int(1, 10)
            );
        }
        
        // Set status to CheckedOut using reflection
        typeof(Cart).GetProperty("Status")?.SetValue(cart, CartStatus.CheckedOut);
        
        return cart;
    }

    /// <summary>
    /// Generates a cart with Active status for testing negative scenarios.
    /// </summary>
    /// <param name="cartId">The cart ID to use</param>
    /// <param name="userId">The user ID to use</param>
    /// <param name="itemCount">Number of items to add to the cart</param>
    /// <returns>A cart with Active status.</returns>
    public static Cart GenerateActiveCart(Guid? cartId = null, Guid? userId = null, int itemCount = 3)
    {
        var faker = new Faker();
        var cart = Cart.Create(userId ?? Guid.NewGuid());
        
        // Set the ID using reflection
        typeof(Cart).GetProperty("Id")?.SetValue(cart, cartId ?? Guid.NewGuid());
        
        // Add items to the cart
        for (int i = 0; i < itemCount; i++)
        {
            cart.AddItem(
                productRefId: Guid.NewGuid(),
                productRefNumber: faker.Random.Int(1, 1000),
                unitPrice: faker.Finance.Amount(1, 100),
                quantity: faker.Random.Int(1, 10)
            );
        }
        
        // Status remains Active (default)
        return cart;
    }

    /// <summary>
    /// Generates a cart with Finalized status for testing negative scenarios.
    /// </summary>
    /// <param name="cartId">The cart ID to use</param>
    /// <param name="userId">The user ID to use</param>
    /// <param name="itemCount">Number of items to add to the cart</param>
    /// <returns>A cart with Finalized status.</returns>
    public static Cart GenerateFinalizedCart(Guid? cartId = null, Guid? userId = null, int itemCount = 3)
    {
        var faker = new Faker();
        var cart = Cart.Create(userId ?? Guid.NewGuid());
        
        // Set the ID using reflection
        typeof(Cart).GetProperty("Id")?.SetValue(cart, cartId ?? Guid.NewGuid());
        
        // Add items to the cart
        for (int i = 0; i < itemCount; i++)
        {
            cart.AddItem(
                productRefId: Guid.NewGuid(),
                productRefNumber: faker.Random.Int(1, 1000),
                unitPrice: faker.Finance.Amount(1, 100),
                quantity: faker.Random.Int(1, 10)
            );
        }
        
        // Set status to Finalized using reflection
        typeof(Cart).GetProperty("Status")?.SetValue(cart, CartStatus.Finalized);
        
        return cart;
    }

    /// <summary>
    /// Generates an empty cart with CheckedOut status for testing edge cases.
    /// </summary>
    /// <param name="cartId">The cart ID to use</param>
    /// <param name="userId">The user ID to use</param>
    /// <returns>An empty cart with CheckedOut status.</returns>
    public static Cart GenerateEmptyCheckedOutCart(Guid? cartId = null, Guid? userId = null)
    {
        var cart = Cart.Create(userId ?? Guid.NewGuid());
        
        // Set the ID using reflection
        typeof(Cart).GetProperty("Id")?.SetValue(cart, cartId ?? Guid.NewGuid());
        
        // Set status to CheckedOut using reflection
        typeof(Cart).GetProperty("Status")?.SetValue(cart, CartStatus.CheckedOut);
        
        return cart;
    }

    /// <summary>
    /// Generates a valid Branch entity for testing.
    /// </summary>
    /// <returns>A valid Branch entity.</returns>
    public static Branch GenerateValidBranch()
    {
        var faker = new Faker();
        return Branch.Create(faker.Company.CompanyName());
    }

    /// <summary>
    /// Generates a valid Product entity for testing.
    /// </summary>
    /// <param name="productNumber">The product number to use</param>
    /// <returns>A valid Product entity.</returns>
    public static Product GenerateValidProduct(int? productNumber = null)
    {
        var faker = new Faker();
        var product = Product.Create(
            title: faker.Commerce.ProductName(),
            description: faker.Commerce.ProductDescription(),
            price: faker.Finance.Amount(1, 1000),
            category: faker.Commerce.Categories(1)[0],
            imageUrl: faker.Image.PicsumUrl(),
            rate: faker.Random.Decimal(1, 5),
            rateCount: faker.Random.Int(0, 100)
        );
        
        if (productNumber.HasValue)
        {
            typeof(Product).GetProperty("ProductNumber")?.SetValue(product, productNumber.Value);
        }
        
        return product;
    }

    /// <summary>
    /// Generates a cart with invalid products (products that don't exist in the database).
    /// </summary>
    /// <param name="cartId">The cart ID to use</param>
    /// <param name="userId">The user ID to use</param>
    /// <param name="invalidProductNumbers">List of invalid product numbers</param>
    /// <returns>A cart with invalid products.</returns>
    public static Cart GenerateCartWithInvalidProducts(Guid? cartId = null, Guid? userId = null, List<int>? invalidProductNumbers = null)
    {
        var faker = new Faker();
        var cart = Cart.Create(userId ?? Guid.NewGuid());
        
        // Set the ID using reflection
        typeof(Cart).GetProperty("Id")?.SetValue(cart, cartId ?? Guid.NewGuid());
        
        // Add items with invalid product numbers
        var productNumbers = invalidProductNumbers ?? new List<int> { 9999, 8888, 7777 };
        
        foreach (var productNumber in productNumbers)
        {
            cart.AddItem(
                productRefId: Guid.NewGuid(),
                productRefNumber: productNumber,
                unitPrice: faker.Finance.Amount(1, 100),
                quantity: faker.Random.Int(1, 10)
            );
        }
        
        // Set status to CheckedOut using reflection
        typeof(Cart).GetProperty("Status")?.SetValue(cart, CartStatus.CheckedOut);
        
        return cart;
    }
}
