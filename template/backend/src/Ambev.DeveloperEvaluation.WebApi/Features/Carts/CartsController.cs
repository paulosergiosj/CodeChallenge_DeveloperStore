using Ambev.DeveloperEvaluation.Application.Carts.CheckoutCart;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetAllCarts;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CheckoutCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetAllCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

/// <summary>
/// Controller for managing cart operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CartsController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new cart for a customer
    /// </summary>
    /// <param name="request">The cart creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateCartResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateCartCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        var createCartResponse = _mapper.Map<CreateCartResponse>(response);
        createCartResponse.UserId = request.UserId; // Preserve the userId from request
        createCartResponse.Id = response.CartId.ToString(); // Convert Guid to string

        return Created(string.Empty, new ApiResponseWithData<CreateCartResponse>
        {
            Success = true,
            Message = "Cart created successfully",
            Data = createCartResponse
        });
    }

    /// <summary>
    /// Retrieves a specific cart by ID
    /// </summary>
    /// <param name="id">The cart ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart details if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCartRequest { Id = id };
        var validator = new GetCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<GetCartCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        var getCartResponse = _mapper.Map<GetCartResponse>(response);

        return Ok(new ApiResponseWithData<GetCartResponse>
        {
            Success = true,
            Message = "Cart retrieved successfully",
            Data = getCartResponse
        });
    }

    /// <summary>
    /// Retrieves a list of all carts with pagination and sorting
    /// </summary>
    /// <param name="request">The request with pagination and sorting parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of carts</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<GetAllCartsResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCarts(
        [FromQuery] GetAllCartsRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetAllCartsQuery>(request);
        var result = await _mediator.Send(query, cancellationToken);

        return OkPaginated(_mapper.Map<PaginatedList<GetAllCartsResponse>>(result));
    }

    /// <summary>
    /// Updates a specific cart by ID
    /// </summary>
    /// <param name="id">The cart ID</param>
    /// <param name="request">The cart update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated cart details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCart([FromRoute] Guid id, [FromBody] UpdateCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new UpdateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<UpdateCartCommand>(request);
        command.CartId = id;

        var response = await _mediator.Send(command, cancellationToken);

        var updateCartResponse = _mapper.Map<UpdateCartResponse>(response);

        return Ok(new ApiResponseWithData<UpdateCartResponse>
        {
            Success = true,
            Message = "Cart updated successfully",
            Data = updateCartResponse
        });
    }

    /// <summary>
    /// Checks out a specific cart by ID
    /// </summary>
    /// <param name="id">The cart ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the cart was checked out</returns>
    [HttpPost("{id}/checkout")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckoutCart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new CheckoutCartRequest { Id = id };
        var command = _mapper.Map<CheckoutCartCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = response.Message
        });
    }

    /// <summary>
    /// Deletes a specific cart by ID
    /// </summary>
    /// <param name="id">The cart ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the cart was deleted</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCart([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCartCommand(id);
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = response.Message
        });
    }
}

