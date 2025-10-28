using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class ProductTestData
{
    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// The generated product will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Product entity with randomly generated data.</returns>
    public static Product GenerateValidProduct()
    {
        var faker = new Faker();
        
        return Product.Create(
            title: faker.Commerce.ProductName(),
            description: faker.Commerce.ProductDescription(),
            price: faker.Random.Decimal(1, 1000),
            category: faker.Commerce.Categories(1)[0],
            imageUrl: faker.Image.PicsumUrl(),
            rate: faker.Random.Decimal(0, 5),
            rateCount: faker.Random.Int(0, 1000)
        );
    }

    /// <summary>
    /// Generates a valid product title.
    /// The generated title will:
    /// - Be between 3 and 250 characters
    /// - Use commerce product names
    /// - Contain only valid characters
    /// </summary>
    /// <returns>A valid product title.</returns>
    public static string GenerateValidTitle()
    {
        return new Faker().Commerce.ProductName();
    }

    /// <summary>
    /// Generates a valid product description.
    /// The generated description will:
    /// - Be between 1 and 1000 characters
    /// - Use commerce product descriptions
    /// - Contain meaningful content
    /// </summary>
    /// <returns>A valid product description.</returns>
    public static string GenerateValidDescription()
    {
        return new Faker().Commerce.ProductDescription();
    }

    /// <summary>
    /// Generates a valid product price.
    /// The generated price will:
    /// - Be greater than zero
    /// - Be a reasonable decimal value
    /// - Follow commerce pricing patterns
    /// </summary>
    /// <returns>A valid product price.</returns>
    public static decimal GenerateValidPrice()
    {
        return new Faker().Random.Decimal(1, 1000);
    }

    /// <summary>
    /// Generates a valid product category.
    /// The generated category will:
    /// - Be between 2 and 100 characters
    /// - Use commerce categories
    /// - Contain only valid characters
    /// </summary>
    /// <returns>A valid product category.</returns>
    public static string GenerateValidCategory()
    {
        return new Faker().Commerce.Categories(1)[0];
    }

    /// <summary>
    /// Generates a valid image URL.
    /// The generated URL will:
    /// - Follow valid URL format
    /// - Use image service URLs
    /// - Be properly formatted
    /// </summary>
    /// <returns>A valid image URL.</returns>
    public static string GenerateValidImageUrl()
    {
        return new Faker().Image.PicsumUrl();
    }

    /// <summary>
    /// Generates a valid rating rate.
    /// The generated rate will:
    /// - Be between 0 and 5 (inclusive)
    /// - Use decimal precision
    /// - Follow rating patterns
    /// </summary>
    /// <returns>A valid rating rate.</returns>
    public static decimal GenerateValidRate()
    {
        return new Faker().Random.Decimal(0, 5);
    }

    /// <summary>
    /// Generates a valid rating count.
    /// The generated count will:
    /// - Be greater than or equal to zero
    /// - Use reasonable integer values
    /// - Follow rating count patterns
    /// </summary>
    /// <returns>A valid rating count.</returns>
    public static int GenerateValidRateCount()
    {
        return new Faker().Random.Int(0, 1000);
    }

    /// <summary>
    /// Generates an invalid product title for testing negative scenarios.
    /// The generated title will:
    /// - Be too short (less than 3 characters)
    /// - Be too long (more than 250 characters)
    /// - Be empty or null
    /// This is useful for testing title validation error cases.
    /// </summary>
    /// <returns>An invalid product title.</returns>
    public static string GenerateInvalidTitle()
    {
        var faker = new Faker();
        return faker.PickRandom(
            "", // Empty
            "AB", // Too short
            faker.Random.String2(251) // Too long
        );
    }

    /// <summary>
    /// Generates an invalid product description for testing negative scenarios.
    /// The generated description will:
    /// - Be too long (more than 1000 characters)
    /// - Be empty
    /// This is useful for testing description validation error cases.
    /// </summary>
    /// <returns>An invalid product description.</returns>
    public static string GenerateInvalidDescription()
    {
        var faker = new Faker();
        return faker.PickRandom(
            "", // Empty
            faker.Random.String2(1001) // Too long
        );
    }

    /// <summary>
    /// Generates an invalid product price for testing negative scenarios.
    /// The generated price will:
    /// - Be zero or negative
    /// This is useful for testing price validation error cases.
    /// </summary>
    /// <returns>An invalid product price.</returns>
    public static decimal GenerateInvalidPrice()
    {
        var faker = new Faker();
        return faker.PickRandom(
            0m, // Zero
            faker.Random.Decimal(-1000, -1) // Negative
        );
    }

    /// <summary>
    /// Generates an invalid product category for testing negative scenarios.
    /// The generated category will:
    /// - Be too short (less than 2 characters)
    /// - Be too long (more than 100 characters)
    /// - Be empty
    /// This is useful for testing category validation error cases.
    /// </summary>
    /// <returns>An invalid product category.</returns>
    public static string GenerateInvalidCategory()
    {
        var faker = new Faker();
        return faker.PickRandom(
            "", // Empty
            "A", // Too short
            faker.Random.String2(101) // Too long
        );
    }

    /// <summary>
    /// Generates an invalid image URL for testing negative scenarios.
    /// The generated URL will:
    /// - Not follow valid URL format
    /// - Be empty
    /// - Be malformed
    /// This is useful for testing image URL validation error cases.
    /// </summary>
    /// <returns>An invalid image URL.</returns>
    public static string GenerateInvalidImageUrl()
    {
        var faker = new Faker();
        return faker.PickRandom(
            "", // Empty
            "not-a-url", // Invalid format
            faker.Lorem.Word() // Random word
        );
    }

    /// <summary>
    /// Generates an invalid rating rate for testing negative scenarios.
    /// The generated rate will:
    /// - Be less than 0
    /// - Be greater than 5
    /// This is useful for testing rating validation error cases.
    /// </summary>
    /// <returns>An invalid rating rate.</returns>
    public static decimal GenerateInvalidRate()
    {
        var faker = new Faker();
        return faker.PickRandom(
            faker.Random.Decimal(-10, -1), // Negative
            faker.Random.Decimal(6, 10) // Greater than 5
        );
    }

    /// <summary>
    /// Generates an invalid rating count for testing negative scenarios.
    /// The generated count will:
    /// - Be negative
    /// This is useful for testing rating count validation error cases.
    /// </summary>
    /// <returns>An invalid rating count.</returns>
    public static int GenerateInvalidRateCount()
    {
        return new Faker().Random.Int(-100, -1);
    }
}
