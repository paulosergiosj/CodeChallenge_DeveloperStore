using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartTests
{
    [Fact(DisplayName = "Given valid user ID When creating cart Then cart should be created with active status")]
    public void Given_ValidUserId_When_CreatingCart_Then_CartShouldBeCreatedWithActiveStatus()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var cart = Cart.Create(userId);

        // Assert
        cart.Should().NotBeNull();
        cart.UserRefId.Should().Be(userId);
        cart.Status.Should().Be(CartStatus.Active);
        cart.Items.Should().BeEmpty();
        cart.CreatedAt.Should().NotBe(DateTime.MinValue);
    }

    [Fact(DisplayName = "Given valid item When adding item to cart Then item should be added")]
    public void Given_ValidItem_When_AddingItemToCart_Then_ItemShouldBeAdded()
    {
        // Arrange
        var cart = CartTestData.GenerateEmptyCart();
        var productRefId = Guid.NewGuid();
        var productRefNumber = 1;
        var unitPrice = 10.0m;
        var quantity = 2;

        // Act
        cart.AddItem(productRefId, productRefNumber, unitPrice, quantity);

        // Assert
        cart.Items.Should().ContainSingle();
        var addedItem = cart.Items.First();
        addedItem.ProductRefId.Should().Be(productRefId);
        addedItem.ProductRefNumber.Should().Be(productRefNumber);
        addedItem.UnitPrice.Should().Be(unitPrice);
        addedItem.Quantity.Should().Be(quantity);
        cart.GetTotalAmount().Should().Be(addedItem.TotalItemWithDiscount());
    }

    [Fact(DisplayName = "Given existing item When adding same item to cart Then quantity should be updated")]
    public void Given_ExistingItem_When_AddingSameItemToCart_Then_QuantityShouldBeUpdated()
    {
        // Arrange
        var cart = CartTestData.GenerateEmptyCart();
        var productRefId = Guid.NewGuid();
        var productRefNumber = 1;
        var unitPrice = 10.0m;
        var initialQuantity = 2;
        cart.AddItem(productRefId, productRefNumber, unitPrice, initialQuantity);

        var additionalQuantity = 3;

        // Act
        cart.AddItem(productRefId, productRefNumber, unitPrice, additionalQuantity);

        // Assert
        cart.Items.Should().ContainSingle();
        var updatedItem = cart.Items.First();
        updatedItem.Quantity.Should().Be(initialQuantity + additionalQuantity);
        cart.GetTotalAmount().Should().Be(updatedItem.TotalItemWithDiscount());
    }

    [Fact(DisplayName = "Given item in cart When removing item by product number Then item should be removed")]
    public void Given_ItemInCart_When_RemovingItemByProductNumber_Then_ItemShouldBeRemoved()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart(itemCount: 2);
        var itemToRemove = cart.Items.First();
        var initialItemCount = cart.Items.Count;

        // Act
        cart.RemoveItem(itemToRemove.ProductRefNumber);

        // Assert
        cart.Items.Should().HaveCount(initialItemCount - 1);
        cart.Items.Should().NotContain(itemToRemove);
    }

    [Fact(DisplayName = "Given item in cart When updating item quantity Then quantity should be updated")]
    public void Given_ItemInCart_When_UpdatingItemQuantity_Then_QuantityShouldBeUpdated()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart(itemCount: 1);
        var itemToUpdate = cart.Items.First();
        var newQuantity = 5;

        // Act
        cart.UpdateItemQuantity(itemToUpdate.ProductRefNumber, newQuantity);

        // Assert
        itemToUpdate.Quantity.Should().Be(newQuantity);
        cart.GetTotalAmount().Should().Be(itemToUpdate.TotalItemWithDiscount());
    }

    [Fact(DisplayName = "Given cart with items When getting total amount Then should return correct total")]
    public void Given_CartWithItems_When_GettingTotalAmount_Then_ShouldReturnCorrectTotal()
    {
        // Arrange
        var cart = CartTestData.GenerateEmptyCart();
        cart.AddItem(Guid.NewGuid(), 1, 10.0m, 2); // 20.0
        cart.AddItem(Guid.NewGuid(), 2, 5.0m, 3);  // 15.0
        var expectedTotal = cart.Items.Sum(item => item.TotalItemWithDiscount());

        // Act
        var actualTotal = cart.GetTotalAmount();

        // Assert
        actualTotal.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "Given checked out cart When setting finalized Then status should be Finalized")]
    public void Given_CheckedOutCart_When_SettingFinalized_Then_StatusShouldBeFinalized()
    {
        // Arrange
        var cart = CartTestData.GenerateCartWithSpecificStatus(CartStatus.CheckedOut);

        // Act
        cart.SetFinalized();

        // Assert
        cart.Status.Should().Be(CartStatus.Finalized);
        cart.UpdatedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Given checked out cart When checking if can be finalized Then should return true")]
    public void Given_CheckedOutCart_When_CheckingIfCanBeFinalized_Then_ShouldReturnTrue()
    {
        // Arrange
        var cart = CartTestData.GenerateCartWithSpecificStatus(CartStatus.CheckedOut);

        // Act
        var canBeFinalized = cart.CanBeFinalized();

        // Assert
        canBeFinalized.Should().BeTrue();
    }

    [Fact(DisplayName = "Given finalized cart When checking if can be finalized Then should return false")]
    public void Given_FinalizedCart_When_CheckingIfCanBeFinalized_Then_ShouldReturnFalse()
    {
        // Arrange
        var cart = CartTestData.GenerateCartWithSpecificStatus(CartStatus.Finalized);

        // Act
        var canBeFinalized = cart.CanBeFinalized();

        // Assert
        canBeFinalized.Should().BeFalse();
    }

    [Fact(DisplayName = "Given active cart When checking if can delete Then should return true")]
    public void Given_ActiveCart_When_CheckingIfCanDelete_Then_ShouldReturnTrue()
    {
        // Arrange
        var cart = CartTestData.GenerateValidCart();

        // Act
        var canDelete = cart.CanDelete();

        // Assert
        canDelete.Should().BeTrue();
    }

    [Fact(DisplayName = "Given finalized cart When checking if can delete Then should return false")]
    public void Given_FinalizedCart_When_CheckingIfCanDelete_Then_ShouldReturnFalse()
    {
        // Arrange
        var cart = CartTestData.GenerateCartWithSpecificStatus(CartStatus.Finalized);

        // Act
        var canDelete = cart.CanDelete();

        // Assert
        canDelete.Should().BeFalse();
    }
}