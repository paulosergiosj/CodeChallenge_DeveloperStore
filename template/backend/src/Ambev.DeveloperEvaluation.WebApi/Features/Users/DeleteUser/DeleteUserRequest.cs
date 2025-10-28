namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;

/// <summary>
/// Request model for deleting a user
/// </summary>
public class DeleteUserRequest
{
    /// <summary>
    /// The user number of the user to delete
    /// </summary>
    public int Id { get; set; }
}
