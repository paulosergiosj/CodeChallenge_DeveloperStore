using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;

/// <summary>
/// Profile for mapping Product to GetAllProductsResult
/// </summary>
public class GetAllProductsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetAllProducts
    /// </summary>
    public GetAllProductsProfile()
    {
        CreateMap<Product, GetAllProductsResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductNumber))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => new ProductRating
            {
                Rate = src.Rating.Rate,
                Count = src.Rating.Count
            }));
    }
}

