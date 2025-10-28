using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation for Product handlers
/// to ensure consistency across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class ProductHandlerTestData
{
    /// <summary>
    /// Generates a valid CreateProductCommand with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid CreateProductCommand with randomly generated data.</returns>
    public static CreateProductCommand GenerateValidCreateCommand()
    {
        var faker = new Faker();
        
        return new CreateProductCommand
        {
            Title = faker.Commerce.ProductName(),
            Description = faker.Commerce.ProductDescription(),
            Price = faker.Random.Decimal(1, 1000),
            Category = faker.Commerce.Categories(1)[0],
            ImageUrl = faker.Image.PicsumUrl(),
            Rate = faker.Random.Decimal(0, 5),
            Count = faker.Random.Int(0, 1000)
        };
    }

    /// <summary>
    /// Generates a valid UpdateProductCommand with randomized data.
    /// The generated command will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid UpdateProductCommand with randomly generated data.</returns>
    public static UpdateProductCommand GenerateValidUpdateCommand()
    {
        var faker = new Faker();
        
        return new UpdateProductCommand
        {
            ProductNumber = faker.Random.Int(1, 1000),
            Title = faker.Commerce.ProductName(),
            Description = faker.Commerce.ProductDescription(),
            Price = faker.Random.Decimal(1, 1000),
            Category = faker.Commerce.Categories(1)[0],
            ImageUrl = faker.Image.PicsumUrl(),
            Rate = faker.Random.Decimal(0, 5),
            Count = faker.Random.Int(0, 1000)
        };
    }

    /// <summary>
    /// Generates a valid DeleteProductCommand with randomized data.
    /// The generated command will have a valid product number.
    /// </summary>
    /// <returns>A valid DeleteProductCommand with randomly generated data.</returns>
    public static DeleteProductCommand GenerateValidDeleteCommand()
    {
        var faker = new Faker();
        
        return new DeleteProductCommand
        {
            ProductNumber = faker.Random.Int(1, 1000)
        };
    }

    /// <summary>
    /// Generates a valid GetProductQuery with randomized data.
    /// The generated query will have a valid product number.
    /// </summary>
    /// <returns>A valid GetProductQuery with randomly generated data.</returns>
    public static GetProductQuery GenerateValidGetQuery()
    {
        var faker = new Faker();
        
        return new GetProductQuery
        {
            ProductNumber = faker.Random.Int(1, 1000)
        };
    }

    /// <summary>
    /// Generates a valid GetAllProductsQuery with randomized data.
    /// The generated query will have valid pagination and filtering parameters.
    /// </summary>
    /// <returns>A valid GetAllProductsQuery with randomly generated data.</returns>
    public static GetAllProductsQuery GenerateValidGetAllQuery()
    {
        var faker = new Faker();
        
        return new GetAllProductsQuery
        {
            PagingParameters = new Ambev.DeveloperEvaluation.Common.Pagination.PagingParameters
            {
                Page = faker.Random.Int(1, 10),
                PageSize = faker.Random.Int(1, 50)
            },
            SortString = faker.PickRandom("title", "price", "category", "description"),
            CategoryFilter = faker.Commerce.Categories(1)[0],
            MinPrice = faker.Random.Decimal(0, 100),
            MaxPrice = faker.Random.Decimal(100, 1000)
        };
    }

    /// <summary>
    /// Generates an invalid CreateProductCommand for testing negative scenarios.
    /// The generated command will have invalid values that should fail validation.
    /// </summary>
    /// <returns>An invalid CreateProductCommand for testing validation errors.</returns>
    public static CreateProductCommand GenerateInvalidCreateCommand()
    {
        var faker = new Faker();
        
        return new CreateProductCommand
        {
            Title = faker.PickRandom("", "AB", faker.Random.String2(251)), // Invalid title
            Description = faker.PickRandom("", faker.Random.String2(1001)), // Invalid description
            Price = faker.PickRandom(0m, faker.Random.Decimal(-1000, -1)), // Invalid price
            Category = faker.PickRandom("", "A", faker.Random.String2(101)), // Invalid category
            ImageUrl = faker.PickRandom("", "not-a-url"), // Invalid image URL
            Rate = faker.PickRandom(faker.Random.Decimal(-10, -1), faker.Random.Decimal(6, 10)), // Invalid rate
            Count = faker.Random.Int(-100, -1) // Invalid count
        };
    }

    /// <summary>
    /// Generates an invalid UpdateProductCommand for testing negative scenarios.
    /// The generated command will have invalid values that should fail validation.
    /// </summary>
    /// <returns>An invalid UpdateProductCommand for testing validation errors.</returns>
    public static UpdateProductCommand GenerateInvalidUpdateCommand()
    {
        var faker = new Faker();
        
        return new UpdateProductCommand
        {
            ProductNumber = faker.Random.Int(-100, 0), // Invalid product number
            Title = faker.PickRandom("", "AB", faker.Random.String2(251)), // Invalid title
            Description = faker.PickRandom("", faker.Random.String2(1001)), // Invalid description
            Price = faker.PickRandom(0m, faker.Random.Decimal(-1000, -1)), // Invalid price
            Category = faker.PickRandom("", "A", faker.Random.String2(101)), // Invalid category
            ImageUrl = faker.PickRandom("", "not-a-url"), // Invalid image URL
            Rate = faker.PickRandom(faker.Random.Decimal(-10, -1), faker.Random.Decimal(6, 10)), // Invalid rate
            Count = faker.Random.Int(-100, -1) // Invalid count
        };
    }

    /// <summary>
    /// Generates an invalid DeleteProductCommand for testing negative scenarios.
    /// The generated command will have an invalid product number.
    /// </summary>
    /// <returns>An invalid DeleteProductCommand for testing validation errors.</returns>
    public static DeleteProductCommand GenerateInvalidDeleteCommand()
    {
        var faker = new Faker();
        
        return new DeleteProductCommand
        {
            ProductNumber = faker.Random.Int(-100, 0) // Invalid product number
        };
    }

    /// <summary>
    /// Generates an invalid GetProductQuery for testing negative scenarios.
    /// The generated query will have an invalid product number.
    /// </summary>
    /// <returns>An invalid GetProductQuery for testing validation errors.</returns>
    public static GetProductQuery GenerateInvalidGetQuery()
    {
        var faker = new Faker();
        
        return new GetProductQuery
        {
            ProductNumber = faker.Random.Int(-100, 0) // Invalid product number
        };
    }
}
