using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.CheckoutCart;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// </summary>
public static class CartHandlerTestData
{
    /// <summary>
    /// Generates a valid CreateCartCommand with randomized data.
    /// The generated command will have all properties populated with valid values.
    /// </summary>
    /// <returns>A valid CreateCartCommand with randomly generated data.</returns>
    public static CreateCartCommand GenerateValidCreateCommand()
    {
        var faker = new Faker();
        
        return new CreateCartCommand
        {
            UserNumber = faker.Random.Int(1, 1000),
            Items = GenerateValidCartItems(faker.Random.Int(1, 5))
        };
    }

    /// <summary>
    /// Generates a valid UpdateCartCommand with randomized data.
    /// The generated command will have all properties populated with valid values
    /// including a valid CartId and a list of items.
    /// </summary>
    /// <returns>A valid UpdateCartCommand with randomly generated data.</returns>
    public static UpdateCartCommand GenerateValidUpdateCommand()
    {
        var faker = new Faker();
        
        return new UpdateCartCommand
        {
            CartId = Guid.NewGuid(),
            Items = GenerateValidUpdateCartItems(faker.Random.Int(1, 5))
        };
    }

    /// <summary>
    /// Generates a valid DeleteCartCommand with randomized data.
    /// The generated command will have a valid cart ID.
    /// </summary>
    /// <returns>A valid DeleteCartCommand with randomly generated data.</returns>
    public static DeleteCartCommand GenerateValidDeleteCommand()
    {
        return new DeleteCartCommand(Guid.NewGuid());
    }

    /// <summary>
    /// Generates a valid GetCartCommand with randomized data.
    /// The generated command will have a valid cart ID.
    /// </summary>
    /// <returns>A valid GetCartCommand with randomly generated data.</returns>
    public static GetCartCommand GenerateValidGetCommand()
    {
        return new GetCartCommand(Guid.NewGuid());
    }

    /// <summary>
    /// Generates a valid CheckoutCartCommand with randomized data.
    /// The generated command will have a valid cart ID.
    /// </summary>
    /// <returns>A valid CheckoutCartCommand with randomly generated data.</returns>
    public static CheckoutCartCommand GenerateValidCheckoutCommand()
    {
        return new CheckoutCartCommand(Guid.NewGuid());
    }

    /// <summary>
    /// Generates a valid cart items collection.
    /// </summary>
    /// <param name="count">Number of cart items to generate</param>
    /// <returns>A collection of valid cart items.</returns>
    public static List<CreateCartItemCommand> GenerateValidCartItems(int count = 3)
    {
        var faker = new Faker();
        var items = new List<CreateCartItemCommand>();

        for (int i = 0; i < count; i++)
        {
            items.Add(new CreateCartItemCommand
            {
                ProductNumber = faker.Random.Int(1, 1000),
                Quantity = faker.Random.Int(1, 10)
            });
        }

        return items;
    }

    /// <summary>
    /// Generates a valid update cart items collection.
    /// </summary>
    /// <param name="count">Number of cart items to generate</param>
    /// <returns>A collection of valid update cart items.</returns>
    public static List<UpdateCartItemCommand> GenerateValidUpdateCartItems(int count = 3)
    {
        var faker = new Faker();
        var items = new List<UpdateCartItemCommand>();

        for (int i = 0; i < count; i++)
        {
            items.Add(new UpdateCartItemCommand
            {
                ProductNumber = faker.Random.Int(1, 1000),
                Quantity = faker.Random.Int(1, 10)
            });
        }

        return items;
    }

    /// <summary>
    /// Generates an invalid CreateCartCommand for testing negative scenarios.
    /// The generated command will have invalid values that should fail validation.
    /// </summary>
    /// <returns>An invalid CreateCartCommand for testing validation errors.</returns>
    public static CreateCartCommand GenerateInvalidCreateCommand()
    {
        var faker = new Faker();
        
        return new CreateCartCommand
        {
            UserNumber = faker.Random.Int(-100, 0), // Invalid user ID
            Items = new List<CreateCartItemCommand>() // Empty items list
        };
    }

    /// <summary>
    /// Generates an invalid UpdateCartCommand for testing negative scenarios.
    /// The generated command will have invalid values that should fail validation.
    /// </summary>
    /// <returns>An invalid UpdateCartCommand for testing validation errors.</returns>
    public static UpdateCartCommand GenerateInvalidUpdateCommand()
    {
        var faker = new Faker();
        
        return new UpdateCartCommand
        {
            CartId = Guid.Empty, // Invalid cart ID
            Items = new List<UpdateCartItemCommand>() // Empty items list
        };
    }

    /// <summary>
    /// Generates an invalid DeleteCartCommand for testing negative scenarios.
    /// The generated command will have an invalid cart ID.
    /// </summary>
    /// <returns>An invalid DeleteCartCommand for testing validation errors.</returns>
    public static DeleteCartCommand GenerateInvalidDeleteCommand()
    {
        return new DeleteCartCommand(Guid.Empty); // Invalid cart ID
    }

    /// <summary>
    /// Generates an invalid GetCartCommand for testing negative scenarios.
    /// The generated command will have an invalid cart ID.
    /// </summary>
    /// <returns>An invalid GetCartCommand for testing validation errors.</returns>
    public static GetCartCommand GenerateInvalidGetCommand()
    {
        return new GetCartCommand(Guid.Empty); // Invalid cart ID
    }

    /// <summary>
    /// Generates an invalid CheckoutCartCommand for testing negative scenarios.
    /// The generated command will have an invalid cart ID.
    /// </summary>
    /// <returns>An invalid CheckoutCartCommand for testing validation errors.</returns>
    public static CheckoutCartCommand GenerateInvalidCheckoutCommand()
    {
        return new CheckoutCartCommand(Guid.Empty); // Invalid cart ID
    }

    /// <summary>
    /// Generates a cart with maximum allowed products for testing edge cases.
    /// </summary>
    /// <returns>A cart with maximum products.</returns>
    public static CreateCartCommand GenerateMaxProductsCart()
    {
        return new CreateCartCommand
        {
            UserNumber = new Faker().Random.Int(1, 1000),
            Items = GenerateValidCartItems(20) // Maximum allowed items
        };
    }

    /// <summary>
    /// Generates a cart with a single product for testing edge cases.
    /// </summary>
    /// <returns>A cart with a single product.</returns>
    public static CreateCartCommand GenerateSingleProductCart()
    {
        return new CreateCartCommand
        {
            UserNumber = new Faker().Random.Int(1, 1000),
            Items = GenerateValidCartItems(1)
        };
    }
}