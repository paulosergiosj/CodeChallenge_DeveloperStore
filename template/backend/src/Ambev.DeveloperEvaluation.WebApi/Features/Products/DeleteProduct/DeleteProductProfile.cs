using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;

/// <summary>
/// Profile for mapping between DeleteProductResult and DeleteProductResponse
/// </summary>
public class DeleteProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for DeleteProduct operation
    /// </summary>
    public DeleteProductProfile()
    {
        CreateMap<DeleteProductResult, DeleteProductResponse>();
    }
}

