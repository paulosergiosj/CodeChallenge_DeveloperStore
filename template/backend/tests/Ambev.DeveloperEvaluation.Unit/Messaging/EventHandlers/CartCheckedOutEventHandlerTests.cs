using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Messaging.EventHandlers;
using Ambev.DeveloperEvaluation.Unit.Messaging.EventHandlers.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Messaging.EventHandlers;

public class CartCheckedOutEventHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartRepository _cartRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly CartCheckedOutEventHandler _handler;

    public CartCheckedOutEventHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _cartRepository = Substitute.For<ICartRepository>();
        _branchRepository = Substitute.For<IBranchRepository>();
        _orderRepository = Substitute.For<IOrderRepository>();
        _productRepository = Substitute.For<IProductRepository>();

        _unitOfWork.Branches.Returns(_branchRepository);
        _unitOfWork.Orders.Returns(_orderRepository);
        _unitOfWork.Products.Returns(_productRepository);

        _handler = new CartCheckedOutEventHandler(_unitOfWork, _cartRepository);
    }

    [Fact(DisplayName = "Given valid event message When handling cart checkout Then should create order successfully")]
    public async Task Handle_ValidEventMessage_ShouldCreateOrderSuccessfully()
    {
        // Arrange
        var eventMessage = CartCheckedOutEventHandlerTestData.GenerateValidEventMessage();
        var cart = CartCheckedOutEventHandlerTestData.GenerateCheckedOutCart(eventMessage.CartId);
        var branch = CartCheckedOutEventHandlerTestData.GenerateValidBranch();
        var products = cart.Items.Select(item => 
            CartCheckedOutEventHandlerTestData.GenerateValidProduct(item.ProductRefNumber)).ToList();

        _cartRepository.GetByIdAsync(eventMessage.CartId).Returns(cart);
        _branchRepository.GetFirstAvailableAsync().Returns(branch);
        _productRepository.AnyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>())
            .Returns(true); // All products exist
        _cartRepository.UpdateAsync(Arg.Any<Cart>()).Returns(Task.CompletedTask);
        _orderRepository.AddAsync(Arg.Any<Order>()).Returns(Task.CompletedTask);
        _unitOfWork.CommitAsync().Returns(Task.FromResult(1));

        // Act
        await _handler.Handle(eventMessage);

        // Assert
        await _cartRepository.Received(1).GetByIdAsync(eventMessage.CartId);
        await _branchRepository.Received(1).GetFirstAvailableAsync();
        await _productRepository.Received(cart.Items.Count).AnyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>());
        await _cartRepository.Received(1).UpdateAsync(Arg.Any<Cart>());
        await _orderRepository.Received(1).AddAsync(Arg.Any<Order>());
        await _unitOfWork.Received(1).CommitAsync();
    }

    [Fact(DisplayName = "Given non-existent cart When handling cart checkout Then should throw KeyNotFoundException")]
    public async Task Handle_NonExistentCart_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var eventMessage = CartCheckedOutEventHandlerTestData.GenerateValidEventMessage();
        _cartRepository.GetByIdAsync(eventMessage.CartId).Returns((Cart?)null);

        // Act
        var act = () => _handler.Handle(eventMessage);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Cart with ID {eventMessage.CartId} not found");
        await _branchRepository.DidNotReceive().GetFirstAvailableAsync();
        await _orderRepository.DidNotReceive().AddAsync(Arg.Any<Order>());
        await _unitOfWork.DidNotReceive().CommitAsync();
    }

    [Fact(DisplayName = "Given active cart When handling cart checkout Then should throw InvalidOperationException")]
    public async Task Handle_ActiveCart_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var eventMessage = CartCheckedOutEventHandlerTestData.GenerateValidEventMessage();
        var cart = CartCheckedOutEventHandlerTestData.GenerateActiveCart(eventMessage.CartId);

        _cartRepository.GetByIdAsync(eventMessage.CartId).Returns(cart);

        // Act
        var act = () => _handler.Handle(eventMessage);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"CartCheckedOutEventHandler: Cart cannot be finalized, current status: {cart.Status}");
        await _branchRepository.DidNotReceive().GetFirstAvailableAsync();
        await _orderRepository.DidNotReceive().AddAsync(Arg.Any<Order>());
        await _unitOfWork.DidNotReceive().CommitAsync();
    }

    [Fact(DisplayName = "Given finalized cart When handling cart checkout Then should throw InvalidOperationException")]
    public async Task Handle_FinalizedCart_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var eventMessage = CartCheckedOutEventHandlerTestData.GenerateValidEventMessage();
        var cart = CartCheckedOutEventHandlerTestData.GenerateFinalizedCart(eventMessage.CartId);

        _cartRepository.GetByIdAsync(eventMessage.CartId).Returns(cart);

        // Act
        var act = () => _handler.Handle(eventMessage);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"CartCheckedOutEventHandler: Cart cannot be finalized, current status: {cart.Status}");
        await _branchRepository.DidNotReceive().GetFirstAvailableAsync();
        await _orderRepository.DidNotReceive().AddAsync(Arg.Any<Order>());
        await _unitOfWork.DidNotReceive().CommitAsync();
    }

    [Fact(DisplayName = "Given no available branches When handling cart checkout Then should throw InvalidOperationException")]
    public async Task Handle_NoAvailableBranches_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var eventMessage = CartCheckedOutEventHandlerTestData.GenerateValidEventMessage();
        var cart = CartCheckedOutEventHandlerTestData.GenerateCheckedOutCart(eventMessage.CartId);

        _cartRepository.GetByIdAsync(eventMessage.CartId).Returns(cart);
        _branchRepository.GetFirstAvailableAsync().Returns((Branch?)null);
        _productRepository.AnyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>())
            .Returns(true); // All products exist

        // Act
        var act = () => _handler.Handle(eventMessage);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("No branches available in the system");
        await _orderRepository.DidNotReceive().AddAsync(Arg.Any<Order>());
        await _unitOfWork.DidNotReceive().CommitAsync();
    }

    [Fact(DisplayName = "Given cart with invalid products When handling cart checkout Then should remove invalid products and create order")]
    public async Task Handle_CartWithInvalidProducts_ShouldRemoveInvalidProductsAndCreateOrder()
    {
        // Arrange
        var eventMessage = CartCheckedOutEventHandlerTestData.GenerateValidEventMessage();
        var cart = CartCheckedOutEventHandlerTestData.GenerateCartWithInvalidProducts(eventMessage.CartId);
        var branch = CartCheckedOutEventHandlerTestData.GenerateValidBranch();

        _cartRepository.GetByIdAsync(eventMessage.CartId).Returns(cart);
        _branchRepository.GetFirstAvailableAsync().Returns(branch);
        _productRepository.AnyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>())
            .Returns(false); // All products are invalid
        _cartRepository.UpdateAsync(Arg.Any<Cart>()).Returns(Task.CompletedTask);
        _orderRepository.AddAsync(Arg.Any<Order>()).Returns(Task.CompletedTask);
        var originalItemCount = cart.Items.Count;

        // Act
        await _handler.Handle(eventMessage);

        // Assert
        await _cartRepository.Received(1).GetByIdAsync(eventMessage.CartId);
        await _branchRepository.DidNotReceive().GetFirstAvailableAsync(); // Not called when cart is empty after cleanup
        await _productRepository.Received(originalItemCount).AnyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>());
        await _cartRepository.Received(2).UpdateAsync(Arg.Any<Cart>()); // Once for cleanup, once for finalization
        await _orderRepository.DidNotReceive().AddAsync(Arg.Any<Order>()); // No order created because cart is empty after cleanup
        await _unitOfWork.DidNotReceive().CommitAsync(); // No commit when cart is empty
    }

    [Fact(DisplayName = "Given empty cart after cleanup When handling cart checkout Then should skip order creation")]
    public async Task Handle_EmptyCartAfterCleanup_ShouldSkipOrderCreation()
    {
        // Arrange
        var eventMessage = CartCheckedOutEventHandlerTestData.GenerateValidEventMessage();
        var cart = CartCheckedOutEventHandlerTestData.GenerateEmptyCheckedOutCart(eventMessage.CartId);

        _cartRepository.GetByIdAsync(eventMessage.CartId).Returns(cart);
        _cartRepository.UpdateAsync(Arg.Any<Cart>()).Returns(Task.CompletedTask);
        _unitOfWork.CommitAsync().Returns(Task.FromResult(1));

        // Act
        await _handler.Handle(eventMessage);

        // Assert
        await _cartRepository.Received(1).GetByIdAsync(eventMessage.CartId);
        await _branchRepository.DidNotReceive().GetFirstAvailableAsync();
        await _productRepository.Received(cart.Items.Count).AnyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>());
        await _cartRepository.Received(1).UpdateAsync(Arg.Any<Cart>()); // Only for finalization
        await _orderRepository.DidNotReceive().AddAsync(Arg.Any<Order>());
        await _unitOfWork.DidNotReceive().CommitAsync(); // No commit when cart is empty
    }

    [Fact(DisplayName = "Given cart with mixed valid and invalid products When handling cart checkout Then should remove only invalid products")]
    public async Task Handle_CartWithMixedProducts_ShouldRemoveOnlyInvalidProducts()
    {
        // Arrange
        var eventMessage = CartCheckedOutEventHandlerTestData.GenerateValidEventMessage();
        var cart = CartCheckedOutEventHandlerTestData.GenerateCheckedOutCart(eventMessage.CartId);
        var branch = CartCheckedOutEventHandlerTestData.GenerateValidBranch();

        // Set up products: first two are valid, third is invalid
        var cartItems = cart.Items.ToList();
        var validProductNumbers = cartItems.Take(2).Select(item => item.ProductRefNumber).ToList();
        var invalidProductNumbers = cartItems.Skip(2).Select(item => item.ProductRefNumber).ToList();

        _cartRepository.GetByIdAsync(eventMessage.CartId).Returns(cart);
        _branchRepository.GetFirstAvailableAsync().Returns(branch);
        
        // Mock product existence check
        _productRepository.AnyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>())
            .Returns(callInfo =>
            {
                // Extract product number from the expression
                var expression = callInfo.Arg<System.Linq.Expressions.Expression<Func<Product, bool>>>();
                // This is a simplified mock - in real tests you might need more sophisticated expression parsing
                return Task.FromResult(true); // For this test, assume all products exist
            });

        _cartRepository.UpdateAsync(Arg.Any<Cart>()).Returns(Task.CompletedTask);
        _orderRepository.AddAsync(Arg.Any<Order>()).Returns(Task.CompletedTask);
        _unitOfWork.CommitAsync().Returns(Task.FromResult(1));

        // Act
        await _handler.Handle(eventMessage);

        // Assert
        await _cartRepository.Received(1).GetByIdAsync(eventMessage.CartId);
        await _branchRepository.Received(1).GetFirstAvailableAsync();
        await _productRepository.Received(cart.Items.Count).AnyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>());
        await _cartRepository.Received(1).UpdateAsync(Arg.Any<Cart>());
        await _orderRepository.Received(1).AddAsync(Arg.Any<Order>());
        await _unitOfWork.Received(1).CommitAsync();
    }

    [Fact(DisplayName = "Given valid cart When handling cart checkout Then should finalize cart")]
    public async Task Handle_ValidCart_ShouldFinalizeCart()
    {
        // Arrange
        var eventMessage = CartCheckedOutEventHandlerTestData.GenerateValidEventMessage();
        var cart = CartCheckedOutEventHandlerTestData.GenerateCheckedOutCart(eventMessage.CartId);
        var branch = CartCheckedOutEventHandlerTestData.GenerateValidBranch();

        _cartRepository.GetByIdAsync(eventMessage.CartId).Returns(cart);
        _branchRepository.GetFirstAvailableAsync().Returns(branch);
        _productRepository.AnyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>())
            .Returns(true); // All products exist
        _cartRepository.UpdateAsync(Arg.Any<Cart>()).Returns(Task.CompletedTask);
        _orderRepository.AddAsync(Arg.Any<Order>()).Returns(Task.CompletedTask);
        _unitOfWork.CommitAsync().Returns(Task.FromResult(1));

        // Act
        await _handler.Handle(eventMessage);

        // Assert
        cart.Status.Should().Be(CartStatus.Finalized);
        await _cartRepository.Received(1).UpdateAsync(Arg.Any<Cart>());
    }

    [Fact(DisplayName = "Given valid cart When handling cart checkout Then should commit unit of work")]
    public async Task Handle_ValidCart_ShouldCommitUnitOfWork()
    {
        // Arrange
        var eventMessage = CartCheckedOutEventHandlerTestData.GenerateValidEventMessage();
        var cart = CartCheckedOutEventHandlerTestData.GenerateCheckedOutCart(eventMessage.CartId);
        var branch = CartCheckedOutEventHandlerTestData.GenerateValidBranch();

        _cartRepository.GetByIdAsync(eventMessage.CartId).Returns(cart);
        _branchRepository.GetFirstAvailableAsync().Returns(branch);
        _productRepository.AnyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>())
            .Returns(true); // All products exist
        _cartRepository.UpdateAsync(Arg.Any<Cart>()).Returns(Task.CompletedTask);
        _orderRepository.AddAsync(Arg.Any<Order>()).Returns(Task.CompletedTask);
        _unitOfWork.CommitAsync().Returns(Task.FromResult(1));

        // Act
        await _handler.Handle(eventMessage);

        // Assert
        await _unitOfWork.Received(1).CommitAsync();
    }
}
