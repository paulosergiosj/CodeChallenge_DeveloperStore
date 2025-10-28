using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Handler for processing DeleteProductCommand requests
/// </summary>
public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of DeleteProductHandler
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance</param>
    public DeleteProductHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the DeleteProductCommand request
    /// </summary>
    /// <param name="command">The DeleteProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new DeleteProductCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingProduct = await _unitOfWork.Products.FirstOrDefaultAsync(
            p => p.ProductNumber == command.ProductNumber, cancellationToken)
            ?? throw new KeyNotFoundException($"Product with number {command.ProductNumber} not found");

        var isReferenced = await _unitOfWork.Products.IsReferencedInOrderItemsAsync(command.ProductNumber, cancellationToken);
        if (isReferenced)
            throw new InvalidOperationException($"Cannot delete product {command.ProductNumber} because it is referenced in existing orders");

        _unitOfWork.Products.Remove(existingProduct);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new DeleteProductResult
        {
            Message = "Product deleted successfully"
        };
    }
}






