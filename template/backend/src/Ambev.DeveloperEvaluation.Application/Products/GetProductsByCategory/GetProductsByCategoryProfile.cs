using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;

/// <summary>
/// Profile for mapping Product to GetProductsByCategoryResult
/// </summary>
public class GetProductsByCategoryProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetProductsByCategory
    /// </summary>
    public GetProductsByCategoryProfile()
    {
        CreateMap<Product, GetProductsByCategoryResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductNumber))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRating
            {
                Rate = src.Rating.Rate,
                Count = src.Rating.Count
            }));
    }
}
