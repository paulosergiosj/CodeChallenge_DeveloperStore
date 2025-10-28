using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
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

/// <summary>
/// Unit tests for UpdateProductHandler.
/// </summary>
public class UpdateProductHandlerTests
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly UpdateProductHandler _handler;

    public UpdateProductHandlerTests()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();

        _unitOfWork.Products.Returns(_productRepository);

        _handler = new UpdateProductHandler(_unitOfWork, _mapper);
    }

    [Fact(DisplayName = "Given valid command When updating product Then returns success response")]
    public async Task Handle_ValidCommand_ReturnsSuccessResponse()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.GenerateValidProduct(command.ProductNumber);

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        
        // Mock the mapper to return a result based on the updated product
        _mapper.Map<UpdateProductResult>(Arg.Any<Product>())
            .Returns(callInfo =>
            {
                var updatedProduct = callInfo.Arg<Product>();
                return new UpdateProductResult
                {
                    Id = updatedProduct.ProductNumber,
                    Title = updatedProduct.Title,
                    Description = updatedProduct.Description,
                    Price = updatedProduct.Price,
                    Category = updatedProduct.Category,
                    Image = updatedProduct.ImageUrl,
                    Rating = new Ambev.DeveloperEvaluation.Domain.ValueObjects.ProductRating
                    {
                        Rate = updatedProduct.Rating.Rate,
                        Count = updatedProduct.Rating.Count
                    }
                };
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(product.ProductNumber);
        result.Title.Should().Be(command.Title); // Should match the command values after update
        result.Description.Should().Be(command.Description);
        result.Price.Should().Be(command.Price);
        result.Category.Should().Be(command.Category);
        result.Image.Should().Be(command.ImageUrl);
        result.Rating.Rate.Should().Be(command.Rate);
        result.Rating.Count.Should().Be(command.Count);
    }

    [Fact(DisplayName = "Given invalid command When updating product Then throws ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateInvalidCommand();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Should().NotBeNull();
        exception.Errors.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Given non-existent product When updating product Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentProduct_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be($"Product with number {command.ProductNumber} not found");
    }

    [Fact(DisplayName = "Given valid command When updating product Then calls FirstOrDefaultAsync")]
    public async Task Handle_ValidCommand_CallsFirstOrDefaultAsync()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.GenerateValidProduct(command.ProductNumber);
        var expectedResult = UpdateProductHandlerTestData.GenerateValidResult(product);

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        _mapper.Map<UpdateProductResult>(product)
            .Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _productRepository.Received(1).FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command When updating product Then calls Update method")]
    public async Task Handle_ValidCommand_CallsUpdateMethod()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.GenerateValidProduct(command.ProductNumber);
        var expectedResult = UpdateProductHandlerTestData.GenerateValidResult(product);

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        _mapper.Map<UpdateProductResult>(product)
            .Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        // Verify that the product's Update method was called with the correct parameters
        // This is verified by checking that the product properties match the command values
        product.Title.Should().Be(command.Title);
        product.Description.Should().Be(command.Description);
        product.Price.Should().Be(command.Price);
        product.Category.Should().Be(command.Category);
        product.ImageUrl.Should().Be(command.ImageUrl);
        product.Rating.Rate.Should().Be(command.Rate);
        product.Rating.Count.Should().Be(command.Count);
    }

    [Fact(DisplayName = "Given valid command When updating product Then commits unit of work")]
    public async Task Handle_ValidCommand_CommitsUnitOfWork()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.GenerateValidProduct(command.ProductNumber);
        var expectedResult = UpdateProductHandlerTestData.GenerateValidResult(product);

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        _mapper.Map<UpdateProductResult>(product)
            .Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given valid command When updating product Then calls Map")]
    public async Task Handle_ValidCommand_CallsMap()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.GenerateValidProduct(command.ProductNumber);
        var expectedResult = UpdateProductHandlerTestData.GenerateValidResult(product);

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        _mapper.Map<UpdateProductResult>(product)
            .Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<UpdateProductResult>(product);
    }

    [Fact(DisplayName = "Given valid command When updating product Then returns correct result structure")]
    public async Task Handle_ValidCommand_ReturnsCorrectResultStructure()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.GenerateValidProduct(command.ProductNumber);
        var expectedResult = UpdateProductHandlerTestData.GenerateValidResult(product);

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        _mapper.Map<UpdateProductResult>(product)
            .Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<UpdateProductResult>();
        result.Id.Should().BeGreaterThan(0);
        result.Title.Should().NotBeNullOrEmpty();
        result.Description.Should().NotBeNullOrEmpty();
        result.Price.Should().BeGreaterThan(0);
        result.Category.Should().NotBeNullOrEmpty();
        result.Image.Should().NotBeNullOrEmpty();
        result.Rating.Should().NotBeNull();
        result.Rating.Rate.Should().BeInRange(0, 5);
        result.Rating.Count.Should().BeGreaterOrEqualTo(0);
    }

    [Fact(DisplayName = "Given command with updated values When updating product Then product properties are updated")]
    public async Task Handle_CommandWithUpdatedValues_ProductPropertiesAreUpdated()
    {
        // Arrange
        var command = UpdateProductHandlerTestData.GenerateValidCommand();
        var product = UpdateProductHandlerTestData.GenerateValidProduct(command.ProductNumber);
        var expectedResult = UpdateProductHandlerTestData.GenerateValidResult(product);

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(1);
        _mapper.Map<UpdateProductResult>(product)
            .Returns(expectedResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        product.Title.Should().Be(command.Title);
        product.Description.Should().Be(command.Description);
        product.Price.Should().Be(command.Price);
        product.Category.Should().Be(command.Category);
        product.ImageUrl.Should().Be(command.ImageUrl);
        product.Rating.Rate.Should().Be(command.Rate);
        product.Rating.Count.Should().Be(command.Count);
    }
}
