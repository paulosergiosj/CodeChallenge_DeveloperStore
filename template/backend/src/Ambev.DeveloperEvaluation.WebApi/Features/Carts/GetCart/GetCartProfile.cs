using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

/// <summary>
/// Profile for mapping GetCart requests and responses
/// </summary>
public class GetCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetCart
    /// </summary>
    public GetCartProfile()
    {
        // Map request to command
        CreateMap<GetCartRequest, Application.Carts.GetCart.GetCartCommand>()
            .ConstructUsing(req => new Application.Carts.GetCart.GetCartCommand(req.Id));

        // Map Cart entity to GetCartResult
        CreateMap<Domain.Entities.Cart, GetCartResult>()
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserRefId))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        // Map CartItem to GetCartItemResult
        CreateMap<Domain.Entities.CartItem, GetCartItemResult>()
            .ForMember(dest => dest.CartItemId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductRefId))
            .ForMember(dest => dest.ProductNumber, opt => opt.MapFrom(src => src.ProductRefNumber))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalItemWithDiscount()))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.PurchaseDiscount.Value));

        // Map GetCartResult to GetCartResponse
        CreateMap<GetCartResult, GetCartResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CartId.ToString()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserNumber))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));

        // Map GetCartItemResult to GetCartProductResponse
        CreateMap<GetCartItemResult, GetCartProductResponse>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductNumber));
    }
}

