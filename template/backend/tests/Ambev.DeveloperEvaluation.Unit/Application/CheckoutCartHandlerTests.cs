using Ambev.DeveloperEvaluation.Application.Carts.CheckoutCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Rebus.Bus;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Unit tests for CheckoutCartHandler.
/// </summary>
public class CheckoutCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IBus _bus;
    private readonly CheckoutCartHandler _handler;

    public CheckoutCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _bus = Substitute.For<IBus>();

        _handler = new CheckoutCartHandler(_cartRepository, _bus);
    }

    [Fact(DisplayName = "Given valid command When checking out cart Then returns success response")]
    public async Task Handle_ValidCommand_ReturnsSuccessResponse()
    {
        // Arrange
        var command = CheckoutCartHandlerTestData.GenerateValidCommand();
        var cart = CheckoutCartHandlerTestData.GenerateActiveCartWithItems(command.CartId);
        var expectedResult = CheckoutCartHandlerTestData.GenerateValidResult(command.CartId);

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns(cart);
        _cartRepository.UpdateAsync(Arg.Any<Cart>())
            .Returns(Task.CompletedTask);
        _bus.Send(Arg.Any<CartCheckedOutEventMessage>())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Cart checked out successfully");
        result.CartId.Should().Be(command.CartId);
    }

    [Fact(DisplayName = "Given non-existent cart When checking out cart Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentCart_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = CheckoutCartHandlerTestData.GenerateValidCommand();

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns((Cart?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be($"Cart with ID {command.CartId} not found");
    }

    [Fact(DisplayName = "Given empty cart When checking out cart Then throws InvalidOperationException")]
    public async Task Handle_EmptyCart_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = CheckoutCartHandlerTestData.GenerateValidCommand();
        var cart = CheckoutCartHandlerTestData.GenerateEmptyCart(command.CartId);

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns(cart);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Only active carts with items can be checked out");
    }

    [Fact(DisplayName = "Given checked out cart When checking out cart Then throws InvalidOperationException")]
    public async Task Handle_CheckedOutCart_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = CheckoutCartHandlerTestData.GenerateValidCommand();
        var cart = CheckoutCartHandlerTestData.GenerateCheckedOutCart(command.CartId);

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns(cart);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Only active carts with items can be checked out");
    }

    [Fact(DisplayName = "Given finalized cart When checking out cart Then throws InvalidOperationException")]
    public async Task Handle_FinalizedCart_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = CheckoutCartHandlerTestData.GenerateValidCommand();
        var cart = CheckoutCartHandlerTestData.GenerateFinalizedCart(command.CartId);

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns(cart);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Only active carts with items can be checked out");
    }

    [Fact(DisplayName = "Given valid command When checking out cart Then calls GetByIdAsync")]
    public async Task Handle_ValidCommand_CallsGetByIdAsync()
    {
        // Arrange
        var command = CheckoutCartHandlerTestData.GenerateValidCommand();
        var cart = CheckoutCartHandlerTestData.GenerateActiveCartWithItems(command.CartId);

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns(cart);
        _cartRepository.UpdateAsync(Arg.Any<Cart>())
            .Returns(Task.CompletedTask);
        _bus.Send(Arg.Any<CartCheckedOutEventMessage>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _cartRepository.Received(1).GetByIdAsync(command.CartId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command When checking out cart Then calls UpdateAsync")]
    public async Task Handle_ValidCommand_CallsUpdateAsync()
    {
        // Arrange
        var command = CheckoutCartHandlerTestData.GenerateValidCommand();
        var cart = CheckoutCartHandlerTestData.GenerateActiveCartWithItems(command.CartId);

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns(cart);
        _cartRepository.UpdateAsync(Arg.Any<Cart>())
            .Returns(Task.CompletedTask);
        _bus.Send(Arg.Any<CartCheckedOutEventMessage>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _cartRepository.Received(1).UpdateAsync(Arg.Is<Cart>(c => c.Status == CartStatus.CheckedOut));
    }

    [Fact(DisplayName = "Given valid command When checking out cart Then sends CartCheckedOutEventMessage")]
    public async Task Handle_ValidCommand_SendsCartCheckedOutEventMessage()
    {
        // Arrange
        var command = CheckoutCartHandlerTestData.GenerateValidCommand();
        var cart = CheckoutCartHandlerTestData.GenerateActiveCartWithItems(command.CartId);

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns(cart);
        _cartRepository.UpdateAsync(Arg.Any<Cart>())
            .Returns(Task.CompletedTask);
        _bus.Send(Arg.Any<CartCheckedOutEventMessage>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _bus.Received(1).Send(Arg.Is<CartCheckedOutEventMessage>(m => m.CartId == command.CartId));
    }

    [Fact(DisplayName = "Given valid command When checking out cart Then sets cart status to CheckedOut")]
    public async Task Handle_ValidCommand_SetsCartStatusToCheckedOut()
    {
        // Arrange
        var command = CheckoutCartHandlerTestData.GenerateValidCommand();
        var cart = CheckoutCartHandlerTestData.GenerateActiveCartWithItems(command.CartId);

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns(cart);
        _cartRepository.UpdateAsync(Arg.Any<Cart>())
            .Returns(Task.CompletedTask);
        _bus.Send(Arg.Any<CartCheckedOutEventMessage>())
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        cart.Status.Should().Be(CartStatus.CheckedOut);
    }

    [Fact(DisplayName = "Given valid command When checking out cart Then returns correct result structure")]
    public async Task Handle_ValidCommand_ReturnsCorrectResultStructure()
    {
        // Arrange
        var command = CheckoutCartHandlerTestData.GenerateValidCommand();
        var cart = CheckoutCartHandlerTestData.GenerateActiveCartWithItems(command.CartId);

        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>())
            .Returns(cart);
        _cartRepository.UpdateAsync(Arg.Any<Cart>())
            .Returns(Task.CompletedTask);
        _bus.Send(Arg.Any<CartCheckedOutEventMessage>())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CheckoutCartResult>();
        result.Success.Should().BeTrue();
        result.Message.Should().NotBeNullOrEmpty();
        result.CartId.Should().NotBe(Guid.Empty);
    }
}
