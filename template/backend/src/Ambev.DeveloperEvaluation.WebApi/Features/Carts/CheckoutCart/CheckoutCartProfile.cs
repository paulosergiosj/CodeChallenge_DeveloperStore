using Ambev.DeveloperEvaluation.Application.Carts.CheckoutCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CheckoutCart;

/// <summary>
/// Profile for mapping CheckoutCart requests
/// </summary>
public class CheckoutCartProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CheckoutCart
    /// </summary>
    public CheckoutCartProfile()
    {
        CreateMap<CheckoutCartRequest, CheckoutCartCommand>()
            .ConstructUsing(req => new CheckoutCartCommand(req.Id));
    }
}



