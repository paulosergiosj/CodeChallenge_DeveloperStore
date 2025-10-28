using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

/// <summary>
/// Validator for DeleteUserCommand
/// </summary>
public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    /// <summary>
    /// Initializes validation rules for DeleteUserCommand
    /// </summary>
    public DeleteUserValidator()
    {
        RuleFor(x => x.UserNumber)
            .GreaterThan(0)
            .WithMessage("User number must be greater than 0");
    }
}
