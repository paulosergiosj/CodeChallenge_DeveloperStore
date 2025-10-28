using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for Product entity that defines validation rules for product domain model.
/// </summary>
public class ProductValidator : AbstractValidator<Product>
{
    /// <summary>
    /// Initializes a new instance of the ProductValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Title: Required, must be between 3 and 250 characters
    /// - Description: Required, maximum 1000 characters
    /// - Price: Must be greater than zero
    /// - Category: Required, must be between 2 and 100 characters
    /// - ImageUrl: Required, must be a valid URL
    /// - Rating: Must be valid (rate between 0-5, count >= 0)
    /// </remarks>
    public ProductValidator()
    {
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

        RuleFor(p => p.Rating)
            .NotNull().WithMessage("Rating is required");

        RuleFor(p => p.Rating.Rate)
            .InclusiveBetween(0, 5).WithMessage("Rate must be between 0 and 5")
            .When(p => p.Rating != null);

        RuleFor(p => p.Rating.Count)
            .GreaterThanOrEqualTo(0).WithMessage("Count must be greater than or equal to zero")
            .When(p => p.Rating != null);
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
