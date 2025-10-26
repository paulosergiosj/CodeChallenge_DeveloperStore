using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Common.Sorting;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;

/// <summary>
/// Handler for processing GetProductsByCategoryQuery requests
/// </summary>
public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, PaginatedList<GetProductsByCategoryResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    // Property mapping for sorting
    private static readonly Dictionary<string, Expression<Func<Domain.Entities.Product, object>>> _sortingPropertyMap = new()
    {
        { "title", p => p.Title },
        { "price", p => p.Price },
        { "category", p => p.Category },
        { "description", p => p.Description }
    };

    /// <summary>
    /// Initializes a new instance of GetProductsByCategoryHandler
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetProductsByCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetProductsByCategoryQuery request
    /// </summary>
    /// <param name="query">The GetProductsByCategory query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of products</returns>
    public async Task<PaginatedList<GetProductsByCategoryResult>> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var productsQuery = _unitOfWork.Products
            .Query()
            .Where(p => p.Category == query.Category);

        // Apply additional filters
        productsQuery = ApplyFilters(productsQuery, query);

        productsQuery = productsQuery.ApplySorting(query.SortString, _sortingPropertyMap);

        var paginatedList = await PaginatedList<Domain.Entities.Product>.CreateAsync(
            productsQuery,
            query.PagingParameters.Page,
            query.PagingParameters.PageSize
        );

        var results = _mapper.Map<List<GetProductsByCategoryResult>>(paginatedList);

        return new PaginatedList<GetProductsByCategoryResult>(
            results,
            paginatedList.TotalCount,
            paginatedList.CurrentPage,
            paginatedList.PageSize
        );
    }

    /// <summary>
    /// Applies filters to the query
    /// </summary>
    private IQueryable<Domain.Entities.Product> ApplyFilters(
        IQueryable<Domain.Entities.Product> query,
        GetProductsByCategoryQuery filterParams)
    {
        // Title filter with wildcard support
        if (!string.IsNullOrWhiteSpace(filterParams.TitleFilter))
        {
            var titleFilter = filterParams.TitleFilter.Trim();
            
            if (titleFilter.StartsWith('*') && titleFilter.EndsWith('*'))
            {
                // Contains: *value*
                var value = titleFilter.Trim('*');
                query = query.Where(p => p.Title.Contains(value));
            }
            else if (titleFilter.StartsWith('*'))
            {
                // Ends with: *value
                var value = titleFilter.TrimStart('*');
                query = query.Where(p => p.Title.EndsWith(value));
            }
            else if (titleFilter.EndsWith('*'))
            {
                // Starts with: value*
                var value = titleFilter.TrimEnd('*');
                query = query.Where(p => p.Title.StartsWith(value));
            }
            else
            {
                // Exact match
                query = query.Where(p => p.Title == titleFilter);
            }
        }

        // Price range filters
        if (filterParams.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filterParams.MinPrice.Value);
        }

        if (filterParams.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filterParams.MaxPrice.Value);
        }

        return query;
    }
}
