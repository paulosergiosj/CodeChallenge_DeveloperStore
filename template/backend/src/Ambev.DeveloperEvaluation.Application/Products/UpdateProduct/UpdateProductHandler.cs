using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Handler for processing UpdateProductCommand requests
/// </summary>
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of UpdateProductHandler
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateProductHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the UpdateProductCommand request
    /// </summary>
    /// <param name="command">The UpdateProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated product details</returns>
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingProduct = await _unitOfWork.Products.FirstOrDefaultAsync(
            p => p.ProductNumber == command.ProductNumber, cancellationToken)
            ?? throw new KeyNotFoundException($"Product with number {command.ProductNumber} not found"); ;

        existingProduct.Update(
            command.Title,
            command.Description,
            command.Price,
            command.Category,
            command.ImageUrl,
            command.Rate,
            command.Count);

        await _unitOfWork.CommitAsync(cancellationToken);

        var result = _mapper.Map<UpdateProductResult>(existingProduct);
        return result;
    }
}
