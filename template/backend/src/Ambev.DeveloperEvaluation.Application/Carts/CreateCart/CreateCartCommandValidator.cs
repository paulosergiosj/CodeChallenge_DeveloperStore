using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

/// <summary>
/// Validator for CreateCartCommand
/// </summary>
public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    /// <summary>
    /// Initializes a new instance of CreateCartCommandValidator
    /// </summary>
    public CreateCartCommandValidator()
    {
        RuleFor(x => x.UserNumber)
            .GreaterThan(0).WithMessage("User number must be greater than 0");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required")
            .Must(items => items.Count > 0).WithMessage("Cart must contain at least one item");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateCartItemCommandValidator());
    }
}

/// <summary>
/// Validator for CreateCartItemCommand
/// </summary>
public class CreateCartItemCommandValidator : AbstractValidator<CreateCartItemCommand>
{
    /// <summary>
    /// Initializes a new instance of CreateCartItemCommandValidator
    /// </summary>
    public CreateCartItemCommandValidator()
    {
        RuleFor(x => x.ProductNumber)
            .NotEmpty().WithMessage("Product ID is required");

        RuleFor(x => x.ProductNumber)
            .GreaterThan(0).WithMessage("Product number must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20 units per item");
    }
}



