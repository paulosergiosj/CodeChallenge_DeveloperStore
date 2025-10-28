using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

/// <summary>
/// Profile for mapping UpdateCart requests and responses
/// </summary>
public class UpdateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateCart
    /// </summary>
    public UpdateCartProfile()
    {
        // Map request to command
        CreateMap<UpdateCartRequest, UpdateCartCommand>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Products));

        CreateMap<CartProductUpdateRequest, UpdateCartItemCommand>()
            .ForMember(dest => dest.ProductNumber, opt => opt.MapFrom(src => src.ProductId));

        // Map Cart entity to UpdateCartResult
        CreateMap<Domain.Entities.Cart, UpdateCartResult>()
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserRefId))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        // Map CartItem to UpdateCartItemResult
        CreateMap<Domain.Entities.CartItem, UpdateCartItemResult>()
            .ForMember(dest => dest.CartItemId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductRefId))
            .ForMember(dest => dest.ProductNumber, opt => opt.MapFrom(src => src.ProductRefNumber))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalItemWithDiscount()))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.PurchaseDiscount.Value));

        // Map UpdateCartResult to UpdateCartResponse
        CreateMap<UpdateCartResult, UpdateCartResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CartId.ToString()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserNumber))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));

        // Map UpdateCartItemResult to UpdateCartProductResponse
        CreateMap<UpdateCartItemResult, UpdateCartProductResponse>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductNumber));
    }
}

