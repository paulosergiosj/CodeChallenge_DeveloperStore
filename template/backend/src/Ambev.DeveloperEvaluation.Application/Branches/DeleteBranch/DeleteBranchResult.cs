namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Result of deleting a branch
/// </summary>
public class DeleteBranchResult
{
    /// <summary>
    /// Gets or sets the success status
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
