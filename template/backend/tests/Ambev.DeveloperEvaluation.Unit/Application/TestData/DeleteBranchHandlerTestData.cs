using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for DeleteBranchHandler tests.
/// </summary>
public static class DeleteBranchHandlerTestData
{
    /// <summary>
    /// Generates a valid DeleteBranchCommand with randomized data.
    /// </summary>
    /// <returns>A valid DeleteBranchCommand with randomly generated data.</returns>
    public static DeleteBranchCommand GenerateValidCommand()
    {
        var faker = new Faker();
        
        return new DeleteBranchCommand(faker.Random.Guid());
    }

    /// <summary>
    /// Generates a valid DeleteBranchCommand with specific branch ID.
    /// </summary>
    /// <param name="branchId">The branch ID to use</param>
    /// <returns>A valid DeleteBranchCommand with the specified branch ID.</returns>
    public static DeleteBranchCommand GenerateValidCommand(Guid branchId)
    {
        return new DeleteBranchCommand(branchId);
    }

    /// <summary>
    /// Generates an invalid DeleteBranchCommand for testing negative scenarios.
    /// </summary>
    /// <returns>An invalid DeleteBranchCommand for testing validation errors.</returns>
    public static DeleteBranchCommand GenerateInvalidCommand()
    {
        return new DeleteBranchCommand(Guid.Empty); // Invalid empty GUID
    }

    /// <summary>
    /// Generates a valid Branch entity for testing.
    /// </summary>
    /// <param name="id">The ID to use</param>
    /// <param name="name">The name to use</param>
    /// <returns>A valid Branch entity.</returns>
    public static Branch GenerateValidBranch(Guid? id = null, string? name = null)
    {
        var faker = new Faker();
        
        return new Branch(
            id: id ?? faker.Random.Guid(),
            name: name ?? faker.Company.CompanyName()
        );
    }

    /// <summary>
    /// Generates a valid DeleteBranchResult for testing.
    /// </summary>
    /// <param name="success">The success status to use</param>
    /// <param name="message">The message to use</param>
    /// <returns>A valid DeleteBranchResult.</returns>
    public static DeleteBranchResult GenerateValidResult(bool success = true, string? message = null)
    {
        return new DeleteBranchResult
        {
            Success = success,
            Message = message ?? (success ? "Branch deleted successfully" : "Failed to delete branch")
        };
    }
}
