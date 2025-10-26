using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetCategories;
using Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsByCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetCategories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of ProductsController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateProductCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateProductResponse>
        {
            Success = true,
            Message = "Product created successfully",
            Data = _mapper.Map<CreateProductResponse>(response)
        });
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<GetAllProductsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts(
        [FromQuery] GetAllProductsRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetAllProductsQuery>(request);
        var result = await _mediator.Send(query, cancellationToken);

        return OkPaginated(_mapper.Map<PaginatedList<GetAllProductsResponse>>(result));
    }

    [HttpGet("categories")]
    [ProducesResponseType(typeof(PaginatedResponse<GetCategoriesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories(
        [FromQuery] GetCategoriesRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetCategoriesQuery>(request);
        var result = await _mediator.Send(query, cancellationToken);

        return OkPaginated(_mapper.Map<PaginatedList<GetCategoriesResponse>>(result));
    }

    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(PaginatedResponse<GetProductsByCategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsByCategory(
        string category,
        [FromQuery] GetProductsByCategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        request.Category = category;

        var query = _mapper.Map<GetProductsByCategoryQuery>(request);
        var result = await _mediator.Send(query, cancellationToken);

        return OkPaginated(_mapper.Map<PaginatedList<GetProductsByCategoryResponse>>(result));
    }

    [HttpGet("{productNumber:int}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(int productNumber, CancellationToken cancellationToken)
    {
        var query = new GetProductQuery { ProductNumber = productNumber };
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(new ApiResponseWithData<GetProductResponse>
        {
            Success = true,
            Message = "Product retrieved successfully",
            Data = _mapper.Map<GetProductResponse>(response)
        });
    }

    [HttpPut("{productNumber:int}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(int productNumber, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateProductCommand>(request);
        command.ProductNumber = productNumber;

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<UpdateProductResponse>
        {
            Success = true,
            Message = "Product updated successfully",
            Data = _mapper.Map<UpdateProductResponse>(response)
        });
    }

    [HttpDelete("{productNumber:int}")]
    [ProducesResponseType(typeof(DeleteProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int productNumber, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand { ProductNumber = productNumber };
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<DeleteProductResponse>(result));
    }
}
