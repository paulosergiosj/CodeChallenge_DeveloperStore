using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

/// <summary>
/// Handler for processing GetCartCommand requests
/// </summary>
public class GetCartHandler : IRequestHandler<GetCartCommand, GetCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetCartHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository instance</param>
    /// <param name="unitOfWork">The unit of work instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetCartHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetCartCommand request
    /// </summary>
    /// <param name="request">The GetCart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart details if found</returns>
    public async Task<GetCartResult> Handle(GetCartCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetCartValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cart = await _cartRepository.GetByIdAsync(request.CartId, cancellationToken);
        if (cart == null)
            throw new KeyNotFoundException($"Cart with ID {request.CartId} not found");

        // Buscar o usuário para obter o UserNumber
        var user = await _unitOfWork.Users.GetByIdAsync(cart.UserRefId, cancellationToken);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {cart.UserRefId} not found");

        var result = _mapper.Map<GetCartResult>(cart);
        result.UserNumber = user.UserNumber;
        result.TotalAmount = cart.GetTotalAmount();

        return result;
    }
}

