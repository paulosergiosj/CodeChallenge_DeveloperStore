using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Command for retrieving a user by their ID
/// </summary>
public record GetUserCommand : IRequest<GetUserResult>
{
    /// <summary>
    /// The user number of the user to retrieve
    /// </summary>
    public int UserNumber { get; }

    /// <summary>
    /// Initializes a new instance of GetUserCommand
    /// </summary>
    /// <param name="userNumber">The user number of the user to retrieve</param>
    public GetUserCommand(int userNumber)
    {
        UserNumber = userNumber;
    }
}
