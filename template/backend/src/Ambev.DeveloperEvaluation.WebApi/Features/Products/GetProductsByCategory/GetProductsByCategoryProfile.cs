using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;
using Ambev.DeveloperEvaluation.Common.Pagination;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsByCategory;

/// <summary>
/// Profile for mapping GetProductsByCategory requests and responses
/// </summary>
public class GetProductsByCategoryProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetProductsByCategory
    /// </summary>
    public GetProductsByCategoryProfile()
    {
        CreateMap<GetProductsByCategoryRequest, GetProductsByCategoryQuery>()
            .ForMember(dest => dest.PagingParameters, opt => opt.MapFrom(src => new PagingParameters
            {
                Page = src.Page,
                PageSize = src.PageSize
            }))
            .ForMember(dest => dest.TitleFilter, opt => opt.MapFrom(src => src.Title));

        CreateMap<GetProductsByCategoryResult, GetProductsByCategoryResponse>();
    }
}
