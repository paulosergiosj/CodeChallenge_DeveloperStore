using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

/// <summary>
/// Validator for UpdateCartRequest
/// </summary>
public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
{
    /// <summary>
    /// Initializes validation rules for UpdateCartRequest
    /// </summary>
    public UpdateCartRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than 0");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("At least one product is required")
            .Must(products => products.Count > 0).WithMessage("Cart must contain at least one product");

        RuleForEach(x => x.Products)
            .SetValidator(new CartProductUpdateRequestValidator());
    }
}

/// <summary>
/// Validator for CartProductUpdateRequest
/// </summary>
public class CartProductUpdateRequestValidator : AbstractValidator<CartProductUpdateRequest>
{
    /// <summary>
    /// Initializes validation rules for CartProductUpdateRequest
    /// </summary>
    public CartProductUpdateRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20 units per item");
    }
}



