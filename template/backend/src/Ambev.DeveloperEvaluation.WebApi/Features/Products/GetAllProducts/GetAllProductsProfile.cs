using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;
using Ambev.DeveloperEvaluation.Common.Pagination;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProducts;

/// <summary>
/// Profile for mapping GetAllProducts requests and responses
/// </summary>
public class GetAllProductsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetAllProducts
    /// </summary>
    public GetAllProductsProfile()
    {
        CreateMap<GetAllProductsRequest, GetAllProductsQuery>()
            .ForMember(dest => dest.PagingParameters, opt => opt.MapFrom(src => new PagingParameters
            {
                Page = src.Page,
                PageSize = src.PageSize
            }))
            .ForMember(dest => dest.TitleFilter, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.CategoryFilter, opt => opt.MapFrom(src => src.Category));

        CreateMap<GetAllProductsResult, GetAllProductsResponse>();
    }
}






