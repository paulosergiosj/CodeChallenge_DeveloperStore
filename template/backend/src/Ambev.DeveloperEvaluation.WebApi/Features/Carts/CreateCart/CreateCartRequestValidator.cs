using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// Validator for CreateCartRequest
/// </summary>
public class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
{
    /// <summary>
    /// Initializes a new instance of CreateCartRequestValidator
    /// </summary>
    public CreateCartRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than 0");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("At least one product is required")
            .Must(products => products.Count > 0).WithMessage("Cart must contain at least one product");

        RuleForEach(x => x.Products)
            .SetValidator(new CartProductRequestValidator());
    }
}

/// <summary>
/// Validator for CartProductRequest
/// </summary>
public class CartProductRequestValidator : AbstractValidator<CartProductRequest>
{
    /// <summary>
    /// Initializes a new instance of CartProductRequestValidator
    /// </summary>
    public CartProductRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("Product ID must be greater than 0");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20).WithMessage("Quantity cannot exceed 20 units per item");
    }
}

