using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Validator for DeleteProductCommand
/// </summary>
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    /// <summary>
    /// Initializes validation rules for DeleteProductCommand
    /// </summary>
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.ProductNumber)
            .GreaterThan(0)
            .WithMessage("Product number must be greater than zero");
    }
}

