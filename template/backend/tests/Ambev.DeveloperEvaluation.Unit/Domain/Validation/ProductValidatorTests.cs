using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the ProductValidator class.
/// Tests cover validation of all product properties including title, description,
/// price, category, imageUrl, and rating requirements.
/// </summary>
public class ProductValidatorTests
{
    private readonly ProductValidator _validator;

    public ProductValidatorTests()
    {
        _validator = new ProductValidator();
    }

    /// <summary>
    /// Tests that validation passes when all product properties are valid.
    /// This test verifies that a product with valid:
    /// - Title (3-250 characters)
    /// - Description (maximum 1000 characters)
    /// - Price (greater than zero)
    /// - Category (2-100 characters)
    /// - ImageUrl (valid URL)
    /// - Rating (rate 0-5, count >= 0)
    /// passes all validation rules without any errors.
    /// </summary>
    [Fact(DisplayName = "Valid product should pass all validation rules")]
    public void Given_ValidProduct_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails for invalid title formats.
    /// This test verifies that titles that are:
    /// - Empty strings
    /// - Too short (less than 3 characters)
    /// - Too long (more than 250 characters)
    /// fail validation with appropriate error messages.
    /// </summary>
    [Fact(DisplayName = "Invalid title should fail validation")]
    public void Given_InvalidTitle_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        
        // Use reflection to set invalid title since properties are private set
        var titleProperty = typeof(Product).GetProperty("Title");
        titleProperty?.SetValue(product, ProductTestData.GenerateInvalidTitle());

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    /// <summary>
    /// Tests that validation fails for invalid description formats.
    /// This test verifies that descriptions that are:
    /// - Empty strings
    /// - Too long (more than 1000 characters)
    /// fail validation with appropriate error messages.
    /// </summary>
    [Fact(DisplayName = "Invalid description should fail validation")]
    public void Given_InvalidDescription_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        
        // Use reflection to set invalid description since properties are private set
        var descriptionProperty = typeof(Product).GetProperty("Description");
        descriptionProperty?.SetValue(product, ProductTestData.GenerateInvalidDescription());

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    /// <summary>
    /// Tests that validation fails for invalid price values.
    /// This test verifies that prices that are:
    /// - Zero
    /// - Negative values
    /// fail validation with appropriate error messages.
    /// </summary>
    [Fact(DisplayName = "Invalid price should fail validation")]
    public void Given_InvalidPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        
        // Use reflection to set invalid price since properties are private set
        var priceProperty = typeof(Product).GetProperty("Price");
        priceProperty?.SetValue(product, ProductTestData.GenerateInvalidPrice());

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    /// <summary>
    /// Tests that validation fails for invalid category formats.
    /// This test verifies that categories that are:
    /// - Empty strings
    /// - Too short (less than 2 characters)
    /// - Too long (more than 100 characters)
    /// fail validation with appropriate error messages.
    /// </summary>
    [Fact(DisplayName = "Invalid category should fail validation")]
    public void Given_InvalidCategory_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        
        // Use reflection to set invalid category since properties are private set
        var categoryProperty = typeof(Product).GetProperty("Category");
        categoryProperty?.SetValue(product, ProductTestData.GenerateInvalidCategory());

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Category);
    }

    /// <summary>
    /// Tests that validation fails for invalid image URL formats.
    /// This test verifies that image URLs that are:
    /// - Empty strings
    /// - Invalid URL formats
    /// - Malformed URLs
    /// fail validation with appropriate error messages.
    /// </summary>
    [Fact(DisplayName = "Invalid image URL should fail validation")]
    public void Given_InvalidImageUrl_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        
        // Use reflection to set invalid image URL since properties are private set
        var imageUrlProperty = typeof(Product).GetProperty("ImageUrl");
        imageUrlProperty?.SetValue(product, ProductTestData.GenerateInvalidImageUrl());

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageUrl);
    }

    /// <summary>
    /// Tests that validation fails for invalid rating values.
    /// This test verifies that ratings that are:
    /// - Rate less than 0
    /// - Rate greater than 5
    /// - Count less than 0
    /// fail validation with appropriate error messages.
    /// </summary>
    [Fact(DisplayName = "Invalid rating should fail validation")]
    public void Given_InvalidRating_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        
        // Use reflection to set invalid rating since properties are private set
        var ratingProperty = typeof(Product).GetProperty("Rating");
        var invalidRating = new Rating(ProductTestData.GenerateInvalidRate(), ProductTestData.GenerateInvalidRateCount());
        ratingProperty?.SetValue(product, invalidRating);

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Rating.Rate);
        result.ShouldHaveValidationErrorFor(x => x.Rating.Count);
    }

    /// <summary>
    /// Tests that validation fails when rating is null.
    /// This test verifies that products with null ratings
    /// fail validation with appropriate error messages.
    /// </summary>
    [Fact(DisplayName = "Null rating should fail validation")]
    public void Given_NullRating_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();
        
        // Use reflection to set null rating since properties are private set
        var ratingProperty = typeof(Product).GetProperty("Rating");
        ratingProperty?.SetValue(product, null);

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Rating);
    }

    /// <summary>
    /// Tests that validation passes for edge case values.
    /// This test verifies that products with edge case values
    /// (minimum valid lengths, maximum valid values) pass validation.
    /// </summary>
    [Fact(DisplayName = "Edge case values should pass validation")]
    public void Given_EdgeCaseValues_When_Validated_Then_ShouldPassValidation()
    {
        // Arrange
        var product = Product.Create(
            title: "ABC", // Minimum length
            description: "A", // Minimum length
            price: 0.01m, // Minimum valid price
            category: "AB", // Minimum length
            imageUrl: "https://example.com", // Valid URL
            rate: 0m, // Minimum rate
            rateCount: 0 // Minimum count
        );

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation passes for maximum valid values.
    /// This test verifies that products with maximum valid values
    /// pass validation without errors.
    /// </summary>
    [Fact(DisplayName = "Maximum valid values should pass validation")]
    public void Given_MaximumValidValues_When_Validated_Then_ShouldPassValidation()
    {
        // Arrange
        var product = Product.Create(
            title: new string('A', 250), // Maximum length
            description: new string('A', 1000), // Maximum length
            price: 999999.99m, // Large price
            category: new string('A', 100), // Maximum length
            imageUrl: "https://example.com/very/long/url/path", // Valid URL
            rate: 5m, // Maximum rate
            rateCount: int.MaxValue // Maximum count
        );

        // Act
        var result = _validator.TestValidate(product);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
