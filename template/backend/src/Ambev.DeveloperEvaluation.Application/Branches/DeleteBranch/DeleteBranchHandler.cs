using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Handler for processing DeleteBranchCommand requests
/// </summary>
public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, DeleteBranchResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of DeleteBranchHandler
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance</param>
    public DeleteBranchHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the DeleteBranchCommand request
    /// </summary>
    /// <param name="request">The DeleteBranch command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    public async Task<DeleteBranchResult> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteBranchCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var branch = await _unitOfWork.Branches.FirstOrDefaultAsync(
            b => b.Id == request.BranchId, cancellationToken);

        if (branch == null)
            throw new KeyNotFoundException($"Branch with ID {request.BranchId} not found");

        // Check if branch is referenced in any orders
        var isReferenced = await _unitOfWork.Branches.IsReferencedInOrdersAsync(request.BranchId, cancellationToken);
        if (isReferenced)
            throw new InvalidOperationException($"Cannot delete branch {branch.Name} because it is referenced in existing orders");

        _unitOfWork.Branches.Remove(branch);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new DeleteBranchResult 
        { 
            Success = true,
            Message = "Branch deleted successfully" 
        };
    }
}
