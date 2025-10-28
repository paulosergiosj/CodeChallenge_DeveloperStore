using Ambev.DeveloperEvaluation.Application.Carts.GetAllCarts;
using AutoMapper;
using Ambev.DeveloperEvaluation.Common.Pagination;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetAllCarts;

/// <summary>
/// Profile for mapping GetAllCarts requests and responses
/// </summary>
public class GetAllCartsProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetAllCarts
    /// </summary>
    public GetAllCartsProfile()
    {
        // Map request to query
        CreateMap<GetAllCartsRequest, Application.Carts.GetAllCarts.GetAllCartsQuery>()
            .ForMember(dest => dest.PagingParameters, opt => opt.MapFrom(src => new PagingParameters
            {
                Page = src._page,
                PageSize = src._size
            }))
            .ForMember(dest => dest.SortString, opt => opt.MapFrom(src => src._order));

        // Map Cart entity to GetAllCartsResult
        CreateMap<Domain.Entities.Cart, GetAllCartsResult>()
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserRefId))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        // Map CartItem to GetAllCartsItemResult
        CreateMap<Domain.Entities.CartItem, GetAllCartsItemResult>()
            .ForMember(dest => dest.CartItemId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductRefId))
            .ForMember(dest => dest.ProductNumber, opt => opt.MapFrom(src => src.ProductRefNumber))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalItemWithDiscount()))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.PurchaseDiscount.Value));

        // Map GetAllCartsResult to GetAllCartsResponse
        CreateMap<GetAllCartsResult, GetAllCartsResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CartId.ToString()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserNumber))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));

        // Map GetAllCartsItemResult to GetAllCartsProductResponse
        CreateMap<GetAllCartsItemResult, GetAllCartsProductResponse>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductNumber));
    }
}

