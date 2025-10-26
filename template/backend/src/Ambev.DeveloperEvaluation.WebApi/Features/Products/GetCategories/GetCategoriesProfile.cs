using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.GetCategories;
using Ambev.DeveloperEvaluation.Common.Pagination;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetCategories;

/// <summary>
/// Profile for mapping GetCategories requests and responses
/// </summary>
public class GetCategoriesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetCategories
    /// </summary>
    public GetCategoriesProfile()
    {
        CreateMap<GetCategoriesRequest, GetCategoriesQuery>()
            .ForMember(dest => dest.PagingParameters, opt => opt.MapFrom(src => new PagingParameters
            {
                Page = src.Page,
                PageSize = src.PageSize
            }))
            .ForMember(dest => dest.NameFilter, opt => opt.MapFrom(src => src.Name));

        CreateMap<GetCategoriesResult, GetCategoriesResponse>();
    }
}
