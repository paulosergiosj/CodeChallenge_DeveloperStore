using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
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
/// Contains unit tests for the <see cref="GetProductHandler"/> class.
/// </summary>
public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly GetProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.Products.Returns(_productRepository);
        _handler = new GetProductHandler(_unitOfWork, _mapper);
    }

    /// <summary>
    /// Tests that a valid product query returns the product successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid product query When getting product Then returns product")]
    public async Task Handle_ValidRequest_ReturnsProduct()
    {
        // Given
        var query = ProductHandlerTestData.GenerateValidGetQuery();
        var product = Product.Create(
            "Test Product",
            "Test Description",
            99.99m,
            "Electronics",
            "https://example.com/image.jpg",
            4.5m,
            100
        );

        var result = new GetProductResult
        {
            Id = product.ProductNumber,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            Image = product.ImageUrl,
            Rating = new Ambev.DeveloperEvaluation.Domain.ValueObjects.ProductRating
            {
                Rate = product.Rating.Rate,
                Count = product.Rating.Count
            }
        };

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(result);

        // When
        var getProductResult = await _handler.Handle(query, CancellationToken.None);

        // Then
        getProductResult.Should().NotBeNull();
        getProductResult.Id.Should().Be(product.ProductNumber);
        getProductResult.Title.Should().Be(product.Title);
        getProductResult.Description.Should().Be(product.Description);
        getProductResult.Price.Should().Be(product.Price);
        getProductResult.Category.Should().Be(product.Category);
        getProductResult.Image.Should().Be(product.ImageUrl);
        getProductResult.Rating.Rate.Should().Be(product.Rating.Rate);
        getProductResult.Rating.Count.Should().Be(product.Rating.Count);
        
        await _productRepository.Received(1).FirstOrDefaultAsync(
            Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
            Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetProductResult>(product);
    }

    /// <summary>
    /// Tests that querying a non-existent product throws KeyNotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent product When getting product Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentProduct_ThrowsKeyNotFoundException()
    {
        // Given
        var query = ProductHandlerTestData.GenerateValidGetQuery();

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        // When
        var act = () => _handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product with number {query.ProductNumber} not found");
    }

    /// <summary>
    /// Tests that the repository is queried with the correct product number.
    /// </summary>
    [Fact(DisplayName = "Given valid query When handling Then queries repository with correct product number")]
    public async Task Handle_ValidRequest_QueriesRepositoryWithCorrectProductNumber()
    {
        // Given
        var query = ProductHandlerTestData.GenerateValidGetQuery();
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
        _mapper.Map<GetProductResult>(product).Returns(new GetProductResult { Id = product.ProductNumber });

        // When
        await _handler.Handle(query, CancellationToken.None);

        // Then
        await _productRepository.Received(1).FirstOrDefaultAsync(
            Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called with the correct product.
    /// </summary>
    [Fact(DisplayName = "Given valid query When handling Then maps product to result")]
    public async Task Handle_ValidRequest_MapsProductToResult()
    {
        // Given
        var query = ProductHandlerTestData.GenerateValidGetQuery();
        var product = Product.Create(
            "Test Product",
            "Test Description",
            99.99m,
            "Electronics",
            "https://example.com/image.jpg",
            4.5m,
            100
        );

        var expectedResult = new GetProductResult
        {
            Id = product.ProductNumber,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            Image = product.ImageUrl,
            Rating = new Ambev.DeveloperEvaluation.Domain.ValueObjects.ProductRating
            {
                Rate = product.Rating.Rate,
                Count = product.Rating.Count
            }
        };

        _productRepository.FirstOrDefaultAsync(Arg.Any<System.Linq.Expressions.Expression<Func<Product, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(expectedResult);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedResult.Id);
        _mapper.Received(1).Map<GetProductResult>(product);
    }
}
