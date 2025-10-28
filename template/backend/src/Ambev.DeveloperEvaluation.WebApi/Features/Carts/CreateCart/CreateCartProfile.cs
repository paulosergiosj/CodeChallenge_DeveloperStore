using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// Profile for mapping CreateCart requests and responses
/// </summary>
public class CreateCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateCart
    /// </summary>
    public CreateCartProfile()
    {
        // Map request to command
        CreateMap<CreateCartRequest, CreateCartCommand>()
            .ForMember(dest => dest.UserNumber, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Products));

        CreateMap<CartProductRequest, CreateCartItemCommand>()
            .ForMember(dest => dest.ProductNumber, opt => opt.MapFrom(src => src.ProductId));

        // Map result to response
        CreateMap<CreateCartResult, CreateCartResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CartId.ToString()))
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Will be set from request
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreatedAt.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));
    }
}

/// <summary>
/// Profile for mapping cart items to products
/// </summary>
public class CartItemToProductProfile : Profile
{
    public CartItemToProductProfile()
    {
        CreateMap<CreateCartItemResult, CreateCartProductResponse>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductNumber));
    }
}

