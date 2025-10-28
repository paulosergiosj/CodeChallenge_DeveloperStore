using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Validator for DeleteBranchCommand
/// </summary>
public class DeleteBranchCommandValidator : AbstractValidator<DeleteBranchCommand>
{
    /// <summary>
    /// Initializes a new instance of DeleteBranchCommandValidator
    /// </summary>
    public DeleteBranchCommandValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required");
    }
}
