using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// Profile for mapping between UpdateProductRequest, UpdateProductCommand, UpdateProductResult and UpdateProductResponse
/// </summary>
public class UpdateProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateProduct operation
    /// </summary>
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductRequest, UpdateProductCommand>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rating.Rate))
            .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Rating.Count));

        CreateMap<UpdateProductResult, UpdateProductResponse>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
    }
}
