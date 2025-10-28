using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

/// <summary>
/// Profile for mapping between GetProductResult and GetProductResponse
/// </summary>
public class GetProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetProduct operation
    /// </summary>
    public GetProductProfile()
    {
        CreateMap<GetProductResult, GetProductResponse>();
    }
}






