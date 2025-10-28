using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

/// <summary>
/// Command for deleting a user
/// </summary>
public record DeleteUserCommand : IRequest<DeleteUserResponse>
{
    /// <summary>
    /// The user number of the user to delete
    /// </summary>
    public int UserNumber { get; }

    /// <summary>
    /// Initializes a new instance of DeleteUserCommand
    /// </summary>
    /// <param name="userNumber">The user number of the user to delete</param>
    public DeleteUserCommand(int userNumber)
    {
        UserNumber = userNumber;
    }
}
