using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartItemTests
{
    [Fact(DisplayName = "Given valid data When creating cart item Then item should be created successfully")]
    public void Given_ValidData_When_CreatingCartItem_Then_ItemShouldBeCreatedSuccessfully()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var productRefId = Guid.NewGuid();
        var productRefNumber = 1;
        var unitPrice = 10.0m;
        var quantity = 2;

        // Act
        var cartItem = CartItem.Create(cartId, productRefId, productRefNumber, unitPrice, quantity);

        // Assert
        cartItem.Should().NotBeNull();
        cartItem.CartId.Should().Be(cartId);
        cartItem.ProductRefId.Should().Be(productRefId);
        cartItem.ProductRefNumber.Should().Be(productRefNumber);
        cartItem.UnitPrice.Should().Be(unitPrice);
        cartItem.Quantity.Should().Be(quantity);
        cartItem.PurchaseDiscount.Should().NotBeNull();
        cartItem.CreatedAt.Should().NotBe(DateTime.MinValue);
        cartItem.TotalItemWithDiscount().Should().Be(unitPrice * quantity);
    }

    [Theory(DisplayName = "Given invalid quantity When creating cart item Then should throw ArgumentException")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(21)]
    public void Given_InvalidQuantity_When_CreatingCartItem_Then_ShouldThrowArgumentException(int quantity)
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var productRefId = Guid.NewGuid();
        var productRefNumber = 1;
        var unitPrice = 10.0m;

        // Act
        var act = () => CartItem.Create(cartId, productRefId, productRefNumber, unitPrice, quantity);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory(DisplayName = "Given invalid unit price When creating cart item Then should throw ArgumentException")]
    [InlineData(0.0)]
    [InlineData(-1.0)]
    public void Given_InvalidUnitPrice_When_CreatingCartItem_Then_ShouldThrowArgumentException(decimal unitPrice)
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var productRefId = Guid.NewGuid();
        var productRefNumber = 1;
        var quantity = 2;

        // Act
        var act = () => CartItem.Create(cartId, productRefId, productRefNumber, unitPrice, quantity);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Given new quantity When updating quantity Then quantity and total should be updated")]
    public void Given_NewQuantity_When_UpdatingQuantity_Then_QuantityAndTotalShouldBeUpdated()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var productRefId = Guid.NewGuid();
        var productRefNumber = 1;
        var unitPrice = 10.0m;
        var initialQuantity = 2;
        var cartItem = CartItem.Create(cartId, productRefId, productRefNumber, unitPrice, initialQuantity);
        var newQuantity = 5;

        // Act
        cartItem.UpdateQuantity(newQuantity);

        // Assert
        cartItem.Quantity.Should().Be(newQuantity);
        cartItem.TotalItemWithDiscount().Should().Be((unitPrice * newQuantity) - cartItem.PurchaseDiscount.Value);
        cartItem.UpdatedAt.Should().NotBeNull();
    }

    [Theory(DisplayName = "Given invalid new quantity When updating quantity Then should throw ArgumentException")]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(21)]
    public void Given_InvalidNewQuantity_When_UpdatingQuantity_Then_ShouldThrowArgumentException(int newQuantity)
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var productRefId = Guid.NewGuid();
        var productRefNumber = 1;
        var unitPrice = 10.0m;
        var initialQuantity = 2;
        var cartItem = CartItem.Create(cartId, productRefId, productRefNumber, unitPrice, initialQuantity);

        // Act
        var act = () => cartItem.UpdateQuantity(newQuantity);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}