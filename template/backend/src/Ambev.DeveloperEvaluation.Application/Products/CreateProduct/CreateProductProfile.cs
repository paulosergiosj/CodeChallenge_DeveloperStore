using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Profile for mapping between Product entity and CreateProductResult
/// </summary>
public class CreateProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateProduct operation
    /// </summary>
    public CreateProductProfile()
    {
        CreateMap<CreateProductCommand, Product>()
            .ConstructUsing(cmd => Product.Create(
                cmd.Title,
                cmd.Description,
                cmd.Price,
                cmd.Category,
                cmd.ImageUrl,
                cmd.Rate,
                cmd.Count));

        CreateMap<Product, CreateProductResult>();
    }
}

