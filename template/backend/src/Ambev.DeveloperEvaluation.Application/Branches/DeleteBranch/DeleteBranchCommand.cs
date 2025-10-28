using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Command for deleting a branch
/// </summary>
public record DeleteBranchCommand : IRequest<DeleteBranchResult>
{
    /// <summary>
    /// The ID of the branch to delete
    /// </summary>
    public Guid BranchId { get; }

    /// <summary>
    /// Initializes a new instance of DeleteBranchCommand
    /// </summary>
    /// <param name="branchId">The ID of the branch to delete</param>
    public DeleteBranchCommand(Guid branchId)
    {
        BranchId = branchId;
    }
}
