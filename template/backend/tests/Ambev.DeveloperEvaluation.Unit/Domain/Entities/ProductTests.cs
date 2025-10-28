using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Product entity class.
/// Tests cover creation, validation, and update scenarios.
/// </summary>
public class ProductTests
{
    /// <summary>
    /// Tests that a valid product can be created successfully.
    /// </summary>
    [Fact(DisplayName = "Product should be created successfully with valid data")]
    public void Given_ValidProductData_When_CreatingProduct_Then_ShouldCreateSuccessfully()
    {
        // Arrange
        var title = ProductTestData.GenerateValidTitle();
        var description = ProductTestData.GenerateValidDescription();
        var price = ProductTestData.GenerateValidPrice();
        var category = ProductTestData.GenerateValidCategory();
        var imageUrl = ProductTestData.GenerateValidImageUrl();
        var rate = ProductTestData.GenerateValidRate();
        var rateCount = ProductTestData.GenerateValidRateCount();

        // Act
        var product = Product.Create(title, description, price, category, imageUrl, rate, rateCount);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(title, product.Title);
        Assert.Equal(description, product.Description);
        Assert.Equal(price, product.Price);
        Assert.Equal(category, product.Category);
        Assert.Equal(imageUrl, product.ImageUrl);
        Assert.Equal(rate, product.Rating.Rate);
        Assert.Equal(rateCount, product.Rating.Count);
        Assert.NotEqual(DateTime.MinValue, product.CreatedAt);
    }

    /// <summary>
    /// Tests that product creation fails when price is zero or negative.
    /// </summary>
    [Fact(DisplayName = "Product creation should fail when price is zero or negative")]
    public void Given_InvalidPrice_When_CreatingProduct_Then_ShouldThrowException()
    {
        // Arrange
        var title = ProductTestData.GenerateValidTitle();
        var description = ProductTestData.GenerateValidDescription();
        var invalidPrice = ProductTestData.GenerateInvalidPrice();
        var category = ProductTestData.GenerateValidCategory();
        var imageUrl = ProductTestData.GenerateValidImageUrl();
        var rate = ProductTestData.GenerateValidRate();
        var rateCount = ProductTestData.GenerateValidRateCount();

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Product.Create(title, description, invalidPrice, category, imageUrl, rate, rateCount);
        });
    }

    /// <summary>
    /// Tests that product can be updated successfully.
    /// </summary>
    [Fact(DisplayName = "Product should be updated successfully with valid data")]
    public void Given_ValidProduct_When_UpdatingProduct_Then_ShouldUpdateSuccessfully()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var newTitle = ProductTestData.GenerateValidTitle();
        var newDescription = ProductTestData.GenerateValidDescription();
        var newPrice = ProductTestData.GenerateValidPrice();
        var newCategory = ProductTestData.GenerateValidCategory();
        var newImageUrl = ProductTestData.GenerateValidImageUrl();
        var newRate = ProductTestData.GenerateValidRate();
        var newRateCount = ProductTestData.GenerateValidRateCount();

        // Act
        product.Update(newTitle, newDescription, newPrice, newCategory, newImageUrl, newRate, newRateCount);

        // Assert
        Assert.Equal(newTitle, product.Title);
        Assert.Equal(newDescription, product.Description);
        Assert.Equal(newPrice, product.Price);
        Assert.Equal(newCategory, product.Category);
        Assert.Equal(newImageUrl, product.ImageUrl);
        Assert.Equal(newRate, product.Rating.Rate);
        Assert.Equal(newRateCount, product.Rating.Count);
        Assert.NotNull(product.UpdatedAt);
    }

    /// <summary>
    /// Tests that product properties are correctly set during creation.
    /// </summary>
    [Fact(DisplayName = "Product properties should be correctly set during creation")]
    public void Given_ProductCreation_When_SettingProperties_Then_PropertiesShouldBeCorrect()
    {
        // Arrange
        var title = "Test Product";
        var description = "Test Description";
        var price = 99.99m;
        var category = "Electronics";
        var imageUrl = "https://example.com/image.jpg";
        var rate = 4.5m;
        var rateCount = 100;

        // Act
        var product = Product.Create(title, description, price, category, imageUrl, rate, rateCount);

        // Assert
        Assert.Equal(title, product.Title);
        Assert.Equal(description, product.Description);
        Assert.Equal(price, product.Price);
        Assert.Equal(category, product.Category);
        Assert.Equal(imageUrl, product.ImageUrl);
        Assert.Equal(rate, product.Rating.Rate);
        Assert.Equal(rateCount, product.Rating.Count);
        // Note: Id is generated by EF Core, not in the domain constructor
        Assert.NotEqual(DateTime.MinValue, product.CreatedAt);
    }

    /// <summary>
    /// Tests that product rating is correctly initialized.
    /// </summary>
    [Fact(DisplayName = "Product rating should be correctly initialized")]
    public void Given_ProductCreation_When_InitializingRating_Then_RatingShouldBeCorrect()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var expectedRate = 3.5m;
        var expectedCount = 50;

        // Act
        product.Update(
            product.Title,
            product.Description,
            product.Price,
            product.Category,
            product.ImageUrl,
            expectedRate,
            expectedCount
        );

        // Assert
        Assert.Equal(expectedRate, product.Rating.Rate);
        Assert.Equal(expectedCount, product.Rating.Count);
    }

    /// <summary>
    /// Tests that product creation with zero price throws exception.
    /// </summary>
    [Fact(DisplayName = "Product creation with zero price should throw exception")]
    public void Given_ZeroPrice_When_CreatingProduct_Then_ShouldThrowException()
    {
        // Arrange
        var title = ProductTestData.GenerateValidTitle();
        var description = ProductTestData.GenerateValidDescription();
        var category = ProductTestData.GenerateValidCategory();
        var imageUrl = ProductTestData.GenerateValidImageUrl();
        var rate = ProductTestData.GenerateValidRate();
        var rateCount = ProductTestData.GenerateValidRateCount();

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Product.Create(title, description, 0m, category, imageUrl, rate, rateCount);
        });

        Assert.Contains("Price must be greater than zero", exception.Message);
    }

    /// <summary>
    /// Tests that product creation with negative price throws exception.
    /// </summary>
    [Fact(DisplayName = "Product creation with negative price should throw exception")]
    public void Given_NegativePrice_When_CreatingProduct_Then_ShouldThrowException()
    {
        // Arrange
        var title = ProductTestData.GenerateValidTitle();
        var description = ProductTestData.GenerateValidDescription();
        var category = ProductTestData.GenerateValidCategory();
        var imageUrl = ProductTestData.GenerateValidImageUrl();
        var rate = ProductTestData.GenerateValidRate();
        var rateCount = ProductTestData.GenerateValidRateCount();

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Product.Create(title, description, -10m, category, imageUrl, rate, rateCount);
        });

        Assert.Contains("Price must be greater than zero", exception.Message);
    }

    /// <summary>
    /// Tests that product update modifies UpdatedAt timestamp.
    /// </summary>
    [Fact(DisplayName = "Product update should modify UpdatedAt timestamp")]
    public void Given_ProductUpdate_When_UpdatingProduct_Then_UpdatedAtShouldBeModified()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        var originalUpdatedAt = product.UpdatedAt;

        // Act
        product.Update(
            ProductTestData.GenerateValidTitle(),
            ProductTestData.GenerateValidDescription(),
            ProductTestData.GenerateValidPrice(),
            ProductTestData.GenerateValidCategory(),
            ProductTestData.GenerateValidImageUrl(),
            ProductTestData.GenerateValidRate(),
            ProductTestData.GenerateValidRateCount()
        );

        // Assert
        Assert.NotNull(product.UpdatedAt);
        Assert.NotEqual(originalUpdatedAt, product.UpdatedAt);
    }
}
