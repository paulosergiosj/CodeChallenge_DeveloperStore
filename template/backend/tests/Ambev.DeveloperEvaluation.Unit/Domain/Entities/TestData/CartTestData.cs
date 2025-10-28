using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class CartTestData
{
    public static Cart GenerateValidCart(Guid? userId = null, int itemCount = 3)
    {
        var faker = new Faker();
        var cart = Cart.Create(userId ?? Guid.NewGuid());

        for (int i = 0; i < itemCount; i++)
        {
            cart.AddItem(
                productRefId: Guid.NewGuid(),
                productRefNumber: faker.Random.Int(1, 1000),
                unitPrice: faker.Finance.Amount(1, 100),
                quantity: faker.Random.Int(1, 10)
            );
        }
        return cart;
    }

    public static Cart GenerateCartWithSpecificStatus(CartStatus status, Guid? userId = null, int itemCount = 3)
    {
        var cart = GenerateValidCart(userId, itemCount);
        
        // Set status using reflection for testing purposes
        typeof(Cart).GetProperty("Status")?.SetValue(cart, status);
        
        return cart;
    }

    public static Cart GenerateEmptyCart(Guid? userId = null)
    {
        return Cart.Create(userId ?? Guid.NewGuid());
    }

    public static CartItem GenerateValidCartItem(Guid? cartId = null)
    {
        var faker = new Faker();
        
        return CartItem.Create(
            cartId: cartId ?? Guid.NewGuid(),
            productRefId: Guid.NewGuid(),
            productRefNumber: faker.Random.Int(1, 1000),
            unitPrice: faker.Random.Decimal(10, 500),
            quantity: faker.Random.Int(1, 10)
        );
    }
}
