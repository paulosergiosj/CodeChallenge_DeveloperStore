using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Handler for processing GetProductQuery requests
/// </summary>
public class GetProductHandler : IRequestHandler<GetProductQuery, GetProductResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetProductHandler
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetProductHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetProductQuery request
    /// </summary>
    /// <param name="query">The GetProduct query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product details</returns>
    public async Task<GetProductResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.FirstOrDefaultAsync(p => p.ProductNumber == query.ProductNumber, cancellationToken);

        if (product == null)
            throw new KeyNotFoundException($"Product with number {query.ProductNumber} not found");

        var result = _mapper.Map<GetProductResult>(product);
        return result;
    }
}

