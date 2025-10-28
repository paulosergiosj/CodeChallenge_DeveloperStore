using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for UpdateProductHandler tests.
/// </summary>
public static class UpdateProductHandlerTestData
{
    /// <summary>
    /// Generates a valid UpdateProductCommand with randomized data.
    /// </summary>
    /// <returns>A valid UpdateProductCommand with randomly generated data.</returns>
    public static UpdateProductCommand GenerateValidCommand()
    {
        var faker = new Faker();
        
        return new UpdateProductCommand
        {
            ProductNumber = faker.Random.Int(1, 1000),
            Title = faker.Commerce.ProductName(),
            Description = faker.Commerce.ProductDescription(),
            Price = faker.Random.Decimal(1, 1000),
            Category = faker.Commerce.Categories(1)[0],
            ImageUrl = faker.Internet.Url(),
            Rate = faker.Random.Decimal(0, 5),
            Count = faker.Random.Int(0, 100)
        };
    }

    /// <summary>
    /// Generates a valid UpdateProductCommand with specific product number.
    /// </summary>
    /// <param name="productNumber">The product number to use</param>
    /// <returns>A valid UpdateProductCommand with the specified product number.</returns>
    public static UpdateProductCommand GenerateValidCommand(int productNumber)
    {
        var faker = new Faker();
        
        return new UpdateProductCommand
        {
            ProductNumber = productNumber,
            Title = faker.Commerce.ProductName(),
            Description = faker.Commerce.ProductDescription(),
            Price = faker.Random.Decimal(1, 1000),
            Category = faker.Commerce.Categories(1)[0],
            ImageUrl = faker.Internet.Url(),
            Rate = faker.Random.Decimal(0, 5),
            Count = faker.Random.Int(0, 100)
        };
    }

    /// <summary>
    /// Generates an invalid UpdateProductCommand for testing negative scenarios.
    /// </summary>
    /// <returns>An invalid UpdateProductCommand for testing validation errors.</returns>
    public static UpdateProductCommand GenerateInvalidCommand()
    {
        return new UpdateProductCommand
        {
            ProductNumber = 0, // Invalid product number
            Title = string.Empty, // Invalid title
            Description = string.Empty, // Invalid description
            Price = 0, // Invalid price
            Category = string.Empty, // Invalid category
            ImageUrl = string.Empty, // Invalid image URL
            Rate = -1, // Invalid rate
            Count = -1 // Invalid count
        };
    }

    /// <summary>
    /// Generates a valid Product entity for testing.
    /// </summary>
    /// <param name="productNumber">The product number to use</param>
    /// <param name="title">The title to use</param>
    /// <param name="description">The description to use</param>
    /// <param name="price">The price to use</param>
    /// <param name="category">The category to use</param>
    /// <param name="imageUrl">The image URL to use</param>
    /// <param name="rate">The rate to use</param>
    /// <param name="count">The count to use</param>
    /// <returns>A valid Product entity.</returns>
    public static Product GenerateValidProduct(
        int? productNumber = null,
        string? title = null,
        string? description = null,
        decimal? price = null,
        string? category = null,
        string? imageUrl = null,
        decimal? rate = null,
        int? count = null)
    {
        var faker = new Faker();
        
        var product = Product.Create(
            title: title ?? faker.Commerce.ProductName(),
            description: description ?? faker.Commerce.ProductDescription(),
            price: price ?? faker.Random.Decimal(1, 1000),
            category: category ?? faker.Commerce.Categories(1)[0],
            imageUrl: imageUrl ?? faker.Internet.Url(),
            rate: rate ?? faker.Random.Decimal(0, 5),
            rateCount: count ?? faker.Random.Int(0, 100)
        );

        // Set product number using reflection if provided
        if (productNumber.HasValue)
        {
            typeof(Product).GetProperty("ProductNumber")?.SetValue(product, productNumber.Value);
        }

        return product;
    }

    /// <summary>
    /// Generates a valid UpdateProductResult for testing.
    /// </summary>
    /// <param name="product">The product to generate result from</param>
    /// <returns>A valid UpdateProductResult.</returns>
    public static UpdateProductResult GenerateValidResult(Product product)
    {
        return new UpdateProductResult
        {
            Id = product.ProductNumber,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            Image = product.ImageUrl,
            Rating = new ProductRating
            {
                Rate = product.Rating.Rate,
                Count = product.Rating.Count
            }
        };
    }

    /// <summary>
    /// Generates a valid UpdateProductResult with specific data for testing.
    /// </summary>
    /// <param name="id">The ID to use</param>
    /// <param name="title">The title to use</param>
    /// <param name="description">The description to use</param>
    /// <param name="price">The price to use</param>
    /// <param name="category">The category to use</param>
    /// <param name="image">The image to use</param>
    /// <param name="rate">The rate to use</param>
    /// <param name="count">The count to use</param>
    /// <returns>A valid UpdateProductResult.</returns>
    public static UpdateProductResult GenerateValidResult(
        int? id = null,
        string? title = null,
        string? description = null,
        decimal? price = null,
        string? category = null,
        string? image = null,
        decimal? rate = null,
        int? count = null)
    {
        var faker = new Faker();
        
        return new UpdateProductResult
        {
            Id = id ?? faker.Random.Int(1, 1000),
            Title = title ?? faker.Commerce.ProductName(),
            Description = description ?? faker.Commerce.ProductDescription(),
            Price = price ?? faker.Random.Decimal(1, 1000),
            Category = category ?? faker.Commerce.Categories(1)[0],
            Image = image ?? faker.Internet.Url(),
            Rating = new ProductRating
            {
                Rate = rate ?? faker.Random.Decimal(0, 5),
                Count = count ?? faker.Random.Int(0, 100)
            }
        };
    }
}
