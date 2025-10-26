using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Validator for UpdateProductCommand that defines validation rules for product update command.
/// </summary>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    /// <summary>
    /// Initializes a new instance of the UpdateProductCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - ProductNumber: Must be greater than zero
    /// - Title: Required, must be between 3 and 250 characters
    /// - Description: Required, maximum 1000 characters
    /// - Price: Must be greater than zero
    /// - Category: Required, must be between 2 and 100 characters
    /// - ImageUrl: Required, must be a valid URL
    /// - Rate: Must be between 0 and 5 (inclusive)
    /// - Count: Must be greater than or equal to zero
    /// </remarks>
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.ProductNumber)
            .GreaterThan(0).WithMessage("Product number must be greater than zero");

        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(3, 250).WithMessage("Title must be between 3 and 250 characters");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");

        RuleFor(p => p.Category)
            .NotEmpty().WithMessage("Category is required")
            .Length(2, 100).WithMessage("Category must be between 2 and 100 characters");

        RuleFor(p => p.ImageUrl)
            .NotEmpty().WithMessage("Image URL is required")
            .Must(BeAValidUrl).WithMessage("ImageUrl must be a valid URL");

        RuleFor(p => p.Rate)
            .InclusiveBetween(0, 5).WithMessage("Rate must be between 0 and 5");

        RuleFor(p => p.Count)
            .GreaterThanOrEqualTo(0).WithMessage("Count must be greater than or equal to zero");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}

