using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="DeleteProductHandler"/> class.
/// </summary>
public class DeleteProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public DeleteProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.Products.Returns(_productRepository);
        _handler = new DeleteProductHandler(_unitOfWork);
    }

    /// <summary>
    /// Tests that a valid product deletion request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product deletion When deleting product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = ProductHandlerTestData.GenerateValidDeleteCommand();
        var product = Product.Create(
            "Test Product",
            "Test Description",
            99.99m,
            "Electronics",
            "https://example.com/image.jpg",
            4.5m,
            100
        );

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);

        // When
        var deleteProductResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        deleteProductResult.Should().NotBeNull();
        deleteProductResult.Message.Should().Be("Product deleted successfully");
        
        await _productRepository.Received(1).FirstOrDefaultAsync(
            Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
            Arg.Any<CancellationToken>());
        _productRepository.Received(1).Remove(product);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid product deletion request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product deletion When deleting product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = ProductHandlerTestData.GenerateInvalidDeleteCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that deleting a non-existent product throws KeyNotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent product When deleting product Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentProduct_ThrowsKeyNotFoundException()
    {
        // Given
        var command = ProductHandlerTestData.GenerateValidDeleteCommand();

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product with number {command.ProductNumber} not found");
    }

    /// <summary>
    /// Tests that the repository is queried with the correct product number.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then queries repository with correct product number")]
    public async Task Handle_ValidRequest_QueriesRepositoryWithCorrectProductNumber()
    {
        // Given
        var command = ProductHandlerTestData.GenerateValidDeleteCommand();
        var product = Product.Create(
            "Test Product",
            "Test Description",
            99.99m,
            "Electronics",
            "https://example.com/image.jpg",
            4.5m,
            100
        );

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _productRepository.Received(1).FirstOrDefaultAsync(
            Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the product is removed from the repository.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then removes product from repository")]
    public async Task Handle_ValidRequest_RemovesProductFromRepository()
    {
        // Given
        var command = ProductHandlerTestData.GenerateValidDeleteCommand();
        var product = Product.Create(
            "Test Product",
            "Test Description",
            99.99m,
            "Electronics",
            "https://example.com/image.jpg",
            4.5m,
            100
        );

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _productRepository.Received(1).Remove(product);
    }

    /// <summary>
    /// Tests that the unit of work is committed after product deletion.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then commits unit of work")]
    public async Task Handle_ValidRequest_CommitsUnitOfWork()
    {
        // Given
        var command = ProductHandlerTestData.GenerateValidDeleteCommand();
        var product = Product.Create(
            "Test Product",
            "Test Description",
            99.99m,
            "Electronics",
            "https://example.com/image.jpg",
            4.5m,
            100
        );

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the result message is correct.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then returns correct success message")]
    public async Task Handle_ValidRequest_ReturnsCorrectSuccessMessage()
    {
        // Given
        var command = ProductHandlerTestData.GenerateValidDeleteCommand();
        var product = Product.Create(
            "Test Product",
            "Test Description",
            99.99m,
            "Electronics",
            "https://example.com/image.jpg",
            4.5m,
            100
        );

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Message.Should().Be("Product deleted successfully");
    }
}
