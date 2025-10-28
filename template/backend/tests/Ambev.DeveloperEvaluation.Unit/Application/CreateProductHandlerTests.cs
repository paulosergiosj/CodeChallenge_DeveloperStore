using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateProductHandler"/> class.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.Products.Returns(_productRepository);
        _handler = new CreateProductHandler(_unitOfWork, _mapper);
    }

    /// <summary>
    /// Tests that a valid product creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product data When creating product Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = ProductHandlerTestData.GenerateValidCreateCommand();
        var product = Product.Create(
            command.Title,
            command.Description,
            command.Price,
            command.Category,
            command.ImageUrl,
            command.Rate,
            command.Count
        );

        var result = new CreateProductResult
        {
            Id = product.Id
        };

        _mapper.Map<Product>(command).Returns(product);
        _productRepository.AddAsync(Arg.Any<Product>()).Returns(Task.CompletedTask);
        _mapper.Map<CreateProductResult>(product).Returns(result);

        // When
        var createProductResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createProductResult.Should().NotBeNull();
        createProductResult.Id.Should().Be(product.Id);
        
        _mapper.Received(1).Map<Product>(command);
        await _productRepository.Received(1).AddAsync(Arg.Any<Product>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<CreateProductResult>(product);
    }

    /// <summary>
    /// Tests that an invalid product creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product data When creating product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = ProductHandlerTestData.GenerateInvalidCreateCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the mapper is called with the correct command.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps command to product entity")]
    public async Task Handle_ValidRequest_MapsCommandToProduct()
    {
        // Given
        var command = ProductHandlerTestData.GenerateValidCreateCommand();
        var product = Product.Create(
            command.Title,
            command.Description,
            command.Price,
            command.Category,
            command.ImageUrl,
            command.Rate,
            command.Count
        );

        _mapper.Map<Product>(command).Returns(product);
        _productRepository.AddAsync(Arg.Any<Product>()).Returns(Task.CompletedTask);
        _mapper.Map<CreateProductResult>(product).Returns(new CreateProductResult { Id = product.Id });

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<Product>(Arg.Is<CreateProductCommand>(c =>
            c.Title == command.Title &&
            c.Description == command.Description &&
            c.Price == command.Price &&
            c.Category == command.Category &&
            c.ImageUrl == command.ImageUrl &&
            c.Rate == command.Rate &&
            c.Count == command.Count));
    }

    /// <summary>
    /// Tests that the unit of work is committed after product creation.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then commits unit of work")]
    public async Task Handle_ValidRequest_CommitsUnitOfWork()
    {
        // Given
        var command = ProductHandlerTestData.GenerateValidCreateCommand();
        var product = Product.Create(
            command.Title,
            command.Description,
            command.Price,
            command.Category,
            command.ImageUrl,
            command.Rate,
            command.Count
        );

        _mapper.Map<Product>(command).Returns(product);
        _productRepository.AddAsync(Arg.Any<Product>()).Returns(Task.CompletedTask);
        _mapper.Map<CreateProductResult>(product).Returns(new CreateProductResult { Id = product.Id });

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the product is added to the repository.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then adds product to repository")]
    public async Task Handle_ValidRequest_AddsProductToRepository()
    {
        // Given
        var command = ProductHandlerTestData.GenerateValidCreateCommand();
        var product = Product.Create(
            command.Title,
            command.Description,
            command.Price,
            command.Category,
            command.ImageUrl,
            command.Rate,
            command.Count
        );

        _mapper.Map<Product>(command).Returns(product);
        _productRepository.AddAsync(Arg.Any<Product>()).Returns(Task.CompletedTask);
        _mapper.Map<CreateProductResult>(product).Returns(new CreateProductResult { Id = product.Id });

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _productRepository.Received(1).AddAsync(Arg.Is<Product>(p =>
            p.Title == product.Title &&
            p.Description == product.Description &&
            p.Price == product.Price &&
            p.Category == product.Category &&
            p.ImageUrl == product.ImageUrl &&
            p.Rating.Rate == product.Rating.Rate &&
            p.Rating.Count == product.Rating.Count));
    }

    /// <summary>
    /// Tests that the result is mapped correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps result correctly")]
    public async Task Handle_ValidRequest_MapsResultCorrectly()
    {
        // Given
        var command = ProductHandlerTestData.GenerateValidCreateCommand();
        var product = Product.Create(
            command.Title,
            command.Description,
            command.Price,
            command.Category,
            command.ImageUrl,
            command.Rate,
            command.Count
        );

        var expectedResult = new CreateProductResult
        {
            Id = product.Id
        };

        _mapper.Map<Product>(command).Returns(product);
        _productRepository.AddAsync(Arg.Any<Product>()).Returns(Task.CompletedTask);
        _mapper.Map<CreateProductResult>(product).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedResult.Id);
        _mapper.Received(1).Map<CreateProductResult>(product);
    }
}
