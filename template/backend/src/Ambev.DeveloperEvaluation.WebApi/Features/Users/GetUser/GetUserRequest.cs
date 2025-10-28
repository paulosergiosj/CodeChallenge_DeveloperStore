namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

/// <summary>
/// Request model for getting a user by ID
/// </summary>
public class GetUserRequest
{
    /// <summary>
    /// The user number of the user to retrieve
    /// </summary>
    public int Id { get; set; }
}
