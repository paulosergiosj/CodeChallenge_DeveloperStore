using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Profile for mapping between Product entity and UpdateProductResult
/// </summary>
public class UpdateProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateProduct operation
    /// </summary>
    public UpdateProductProfile()
    {
        CreateMap<Product, UpdateProductResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductNumber))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRating
            {
                Rate = src.Rating.Rate,
                Count = src.Rating.Count
            }));
    }
}

