using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

/// <summary>
/// Validator for UpdateCartCommand
/// </summary>
public class UpdateCartValidator : AbstractValidator<UpdateCartCommand>
{
    /// <summary>
    /// Initializes validation rules for UpdateCartCommand
    /// </summary>
    public UpdateCartValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("Cart ID is required");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required")
            .Must(items => items.Count > 0).WithMessage("Cart must contain at least one item");

        RuleForEach(x => x.Items)
            .SetValidator(new UpdateCartItemCommandValidator());
    }
}

/// <summary>
/// Validator for UpdateCartItemCommand
/// </summary>
public class UpdateCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand>
{
    /// <summary>
    /// Initializes a new instance of UpdateCartItemCommandValidator
    /// </summary>
    public UpdateCartItemCommandValidator()
    {
        RuleFor(x => x.ProductNumber)
            .GreaterThan(0).WithMessage("Product number must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20 units per item");
    }
}



