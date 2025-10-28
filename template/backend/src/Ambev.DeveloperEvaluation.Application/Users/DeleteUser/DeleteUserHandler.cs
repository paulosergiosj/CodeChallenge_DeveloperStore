using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

/// <summary>
/// Handler for processing DeleteUserCommand requests
/// </summary>
public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of DeleteUserHandler
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance</param>
    public DeleteUserHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the DeleteUserCommand request
    /// </summary>
    /// <param name="request">The DeleteUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteUserValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var user = await _unitOfWork.Users.GetByUserNumberAsync(request.UserNumber, cancellationToken);
        if (user == null)
            throw new KeyNotFoundException($"User with number {request.UserNumber} not found");

        // Check if user is referenced in any orders
        var isReferenced = await _unitOfWork.Users.IsReferencedInOrdersAsync(user.Id, cancellationToken);
        if (isReferenced)
            throw new InvalidOperationException($"Cannot delete user {request.UserNumber} because they have existing orders");

        await _unitOfWork.Users.DeleteAsync(user.Id, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new DeleteUserResponse { Success = true };
    }
}
