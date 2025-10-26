using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(p => p.Price)
            .GreaterThan(0);

        RuleFor(p => p.Category)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(p => p.Image)
            .NotEmpty()
            .Must(BeAValidUrl)
            .WithMessage("Image must be a valid URL");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
