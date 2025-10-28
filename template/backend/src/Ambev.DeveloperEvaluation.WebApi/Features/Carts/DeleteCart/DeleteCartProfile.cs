using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;

/// <summary>
/// Profile for mapping DeleteCart requests
/// </summary>
public class DeleteCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteCart
    /// </summary>
    public DeleteCartProfile()
    {
        CreateMap<DeleteCartRequest, Application.Carts.DeleteCart.DeleteCartCommand>()
            .ConstructUsing(req => new Application.Carts.DeleteCart.DeleteCartCommand(req.Id));
    }
}



