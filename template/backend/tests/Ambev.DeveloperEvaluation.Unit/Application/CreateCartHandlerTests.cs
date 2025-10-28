using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateCartHandlerTests
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly CreateCartHandler _handler;

    public CreateCartHandlerTests()
    {
        _cartRepository = Substitute.For<ICartRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();

        _unitOfWork.Users.Returns(_userRepository);
        _unitOfWork.Products.Returns(_productRepository);

        _handler = new CreateCartHandler(_unitOfWork, _mapper, _cartRepository);
    }

    [Fact(DisplayName = "Given valid command When creating cart Then returns success response")]
    public async Task Handle_ValidCommand_ReturnsSuccessResponse()
    {
        // Arrange
        var command = CartHandlerTestData.GenerateValidCreateCommand();
        var user = User.Create("testuser", "test@example.com", "+5511987654321", "Password123!", Ambev.DeveloperEvaluation.Domain.Enums.UserRole.Customer);
        typeof(User).GetProperty("UserNumber")?.SetValue(user, command.UserNumber); // Set UserNumber via reflection

        var products = new List<Product>();
        foreach (var item in command.Items)
        {
            var product = Product.Create($"Test Product {item.ProductNumber}", "Description", 10.0m, "Category", "http://image.com", 4.0m, 10);
            typeof(Product).GetProperty("ProductNumber")?.SetValue(product, item.ProductNumber);
            products.Add(product);
        }

        var cart = Cart.Create(user.Id);
        var createCartResult = new CreateCartResult { CartId = cart.Id };

        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns(user);
        _productRepository.GetManyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(products);
        _cartRepository.GetActiveCartByUserIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns((Cart?)null); // No existing cart
        _cartRepository.AddAsync(Arg.Any<Cart>()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CartId.Should().Be(cart.Id);
        await _userRepository.Received(1).GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>());
        await _productRepository.Received(1).GetManyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).AddAsync(Arg.Any<Cart>());
    }

    [Fact(DisplayName = "Given invalid command When creating cart Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = CartHandlerTestData.GenerateInvalidCreateCommand();

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        await _userRepository.DidNotReceive().GetByUserNumberAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        await _cartRepository.DidNotReceive().AddAsync(Arg.Any<Cart>());
    }

    [Fact(DisplayName = "Given non-existent user When creating cart Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = CartHandlerTestData.GenerateValidCreateCommand();
        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns((User?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with number {command.UserNumber} not found");
        await _cartRepository.DidNotReceive().AddAsync(Arg.Any<Cart>());
    }

    [Fact(DisplayName = "Given non-existent product When creating cart Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentProduct_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = CartHandlerTestData.GenerateValidCreateCommand();
        var user = User.Create("testuser", "test@example.com", "+5511987654321", "Password123!", Ambev.DeveloperEvaluation.Domain.Enums.UserRole.Customer);
        typeof(User).GetProperty("UserNumber")?.SetValue(user, command.UserNumber);

        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns(user);
        _productRepository.GetManyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Product>()); // No products found

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("One or more products not found");
        await _cartRepository.DidNotReceive().AddAsync(Arg.Any<Cart>());
    }

    [Fact(DisplayName = "Given existing active cart When creating cart Then throws InvalidOperationException")]
    public async Task Handle_ExistingActiveCart_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = CartHandlerTestData.GenerateValidCreateCommand();
        var user = User.Create("testuser", "test@example.com", "+5511987654321", "Password123!", Ambev.DeveloperEvaluation.Domain.Enums.UserRole.Customer);
        typeof(User).GetProperty("UserNumber")?.SetValue(user, command.UserNumber);

        var existingCart = Cart.Create(user.Id);

        var products = new List<Product>();
        foreach (var item in command.Items)
        {
            var product = Product.Create($"Test Product {item.ProductNumber}", "Description", 10.0m, "Category", "http://image.com", 4.0m, 10);
            typeof(Product).GetProperty("ProductNumber")?.SetValue(product, item.ProductNumber);
            products.Add(product);
        }

        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns(user);
        _productRepository.GetManyAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(products);
        _cartRepository.GetActiveCartByUserIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(existingCart); // Existing active cart

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User already has an active cart. Please complete the existing cart before creating a new one.");
        await _cartRepository.DidNotReceive().AddAsync(Arg.Any<Cart>());
    }

    [Fact(DisplayName = "Given non-customer user When creating cart Then throws InvalidOperationException")]
    public async Task Handle_NonCustomerUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = CartHandlerTestData.GenerateValidCreateCommand();
        var user = User.Create("admin", "admin@example.com", "+5511987654321", "Password123!", Ambev.DeveloperEvaluation.Domain.Enums.UserRole.Admin);
        typeof(User).GetProperty("UserNumber")?.SetValue(user, command.UserNumber);

        _userRepository.GetByUserNumberAsync(command.UserNumber, Arg.Any<CancellationToken>())
            .Returns(user);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Only customers can create carts");
        await _cartRepository.DidNotReceive().AddAsync(Arg.Any<Cart>());
    }
}