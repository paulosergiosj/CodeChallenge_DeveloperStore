using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Handler for processing CreateCartCommand requests
/// </summary>
public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICartRepository _cartRepository;

    /// <summary>
    /// Initializes a new instance of CreateCartHandler
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CreateCartHandler(IUnitOfWork unitOfWork, IMapper mapper, ICartRepository cartRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cartRepository = cartRepository;
    }

    /// <summary>
    /// Handles the CreateCartCommand request
    /// </summary>
    /// <param name="command">The CreateCart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart details</returns>
    public async Task<CreateCartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateCartCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await _unitOfWork.Users.GetByUserNumberAsync(command.UserNumber, cancellationToken)
            ?? throw new KeyNotFoundException($"User with number {command.UserNumber} not found");

        if (user.Role != UserRole.Customer)
            throw new InvalidOperationException("Only customers can create carts");

        var productNumbers = command.Items.Select(i => i.ProductNumber).ToList();

        var products = await _unitOfWork.Products.GetManyAsync(p => productNumbers.Contains(p.ProductNumber), cancellationToken);

        if (products.Count() != command.Items.Count)
            throw new KeyNotFoundException("One or more products not found");

        var existingCart = await _cartRepository.GetActiveCartByUserIdAsync(user.Id, cancellationToken);
        
        if (existingCart != null)
        {
            throw new InvalidOperationException("User already has an active cart. Please complete the existing cart before creating a new one.");
        }

        var cart = Cart.Create(user.Id);

        var cartItems = new List<CreateCartItemResult>();

        foreach (var itemCommand in command.Items)
        {
            var product = products.FirstOrDefault(p => p!.ProductNumber == itemCommand.ProductNumber)
                ?? throw new KeyNotFoundException($"Product with number {itemCommand.ProductNumber} not found");

            var cartItem = cart.AddItem(product.Id, product.ProductNumber, product.Price, itemCommand.Quantity);

            cartItems.Add(new CreateCartItemResult
            {
                CartItemId = cartItem.Id,
                ProductId = product.Id,
                ProductNumber = product.ProductNumber,
                UnitPrice = product.Price,
                Quantity = cartItem.Quantity,
                DiscountAmount = cartItem.PurchaseDiscount.Value,
                TotalAmount = cartItem.TotalItemWithDiscount()
            });
        }

        await _cartRepository.AddAsync(cart);

        // Return the result
        var result = new CreateCartResult
        {
            CartId = cart.Id,
            UserId = cart.UserRefId,
            CreatedAt = cart.CreatedAt,
            Items = cartItems,
            TotalAmount = cart.GetTotalAmount()
        };

        return result;
    }
}
