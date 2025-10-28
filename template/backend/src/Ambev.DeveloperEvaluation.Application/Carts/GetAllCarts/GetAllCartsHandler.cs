using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Common.Sorting;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetAllCarts;

/// <summary>
/// Handler for processing GetAllCartsQuery requests
/// </summary>
public class GetAllCartsHandler : IRequestHandler<GetAllCartsQuery, PaginatedList<GetAllCartsResult>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    // Property mapping for sorting
    private static readonly Dictionary<string, Expression<Func<Domain.Entities.Cart, object>>> _sortingPropertyMap = new()
    {
        { "id", c => c.Id },
        { "userid", c => c.UserRefId },
        { "createdat", c => c.CreatedAt },
        { "status", c => c.Status }
    };

    /// <summary>
    /// Initializes a new instance of GetAllCartsHandler
    /// </summary>
    /// <param name="cartRepository">The cart repository instance</param>
    /// <param name="unitOfWork">The unit of work instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetAllCartsHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetAllCartsQuery request
    /// </summary>
    /// <param name="query">The GetAllCarts query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of carts</returns>
    public async Task<PaginatedList<GetAllCartsResult>> Handle(GetAllCartsQuery query, CancellationToken cancellationToken)
    {
        // Get paginated carts directly from MongoDB
        var (carts, totalCount) = await _cartRepository.GetPagedAsync(
            query.PagingParameters.Page,
            query.PagingParameters.PageSize,
            query.SortString,
            cancellationToken);

        // Map to result DTOs with UserNumber
        var results = new List<GetAllCartsResult>();

        foreach (var cart in carts)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(cart.UserRefId, cancellationToken);
            
            var result = _mapper.Map<GetAllCartsResult>(cart);
            result.UserNumber = user?.UserNumber ?? 0;
            result.TotalAmount = cart.GetTotalAmount();
            
            results.Add(result);
        }

        // Return new paginated list with mapped results
        return new PaginatedList<GetAllCartsResult>(
            results,
            (int)totalCount,
            query.PagingParameters.Page,
            query.PagingParameters.PageSize
        );
    }
}

