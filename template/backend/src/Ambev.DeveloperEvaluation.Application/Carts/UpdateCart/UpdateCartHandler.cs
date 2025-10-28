using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

/// <summary>
/// Handler for processing UpdateCartCommand requests
/// </summary>
public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateCartHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository instance</param>
    /// <param name="unitOfWork">The unit of work instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateCartHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateCartCommand request
    /// </summary>
    /// <param name="request">The UpdateCart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated cart details</returns>
    public async Task<UpdateCartResult> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateCartValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var cart = await _cartRepository.GetByIdAsync(request.CartId, cancellationToken)
            ?? throw new KeyNotFoundException($"Cart with ID {request.CartId} not found");

        var user = await _unitOfWork.Users.GetByIdAsync(cart.UserRefId, cancellationToken)
            ?? throw new KeyNotFoundException($"User with ID {cart.UserRefId} not found");

        if (!cart.CanUpdate())
            throw new InvalidOperationException($"Only Active Carts can be updated.");

        var uniqueProductNumbers = request.Items
            .GroupBy(i => i.ProductNumber)
            .Select(g => new { ProductNumber = g.Key, TotalQuantity = g.Sum(i => i.Quantity) })
            .ToList();

        var productNumbers = uniqueProductNumbers.Select(p => p.ProductNumber).ToList();
        var products = await _unitOfWork.Products.GetManyAsync(p => productNumbers.Contains(p.ProductNumber), cancellationToken);

        if (products.Count() != uniqueProductNumbers.Count)
            throw new KeyNotFoundException("One or more products not found");

        cart.Clear();

        foreach (var item in uniqueProductNumbers)
        {
            var product = products.FirstOrDefault(p => p!.ProductNumber == item.ProductNumber)
                ?? throw new KeyNotFoundException($"Product with number {item.ProductNumber} not found");

            cart.AddItem(product.Id, product.ProductNumber, product.Price, item.TotalQuantity);
        }

        await _cartRepository.UpdateAsync(cart);

        cart = await _cartRepository.GetByIdAsync(cart.Id);

        // Use mapper to convert Cart to UpdateCartResult
        var result = _mapper.Map<UpdateCartResult>(cart);
        result.UserNumber = user.UserNumber;
        result.TotalAmount = cart!.GetTotalAmount();

        return result;
    }
}

