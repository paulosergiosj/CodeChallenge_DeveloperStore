using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Common.Sorting;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;

/// <summary>
/// Handler for processing GetAllProductsQuery requests
/// </summary>
public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, PaginatedList<GetAllProductsResult>>
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
    /// Initializes a new instance of GetAllProductsHandler
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetAllProductsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetAllProductsQuery request
    /// </summary>
    /// <param name="query">The GetAllProducts query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of products</returns>
    public async Task<PaginatedList<GetAllProductsResult>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        // Start with base query
        var productsQuery = _unitOfWork.Products.Query();

        // Apply filters
        productsQuery = ApplyFilters(productsQuery, query);

        // Apply sorting
        productsQuery = productsQuery.ApplySorting(query.SortString, _sortingPropertyMap);

        // Create paginated list
        var paginatedList = await PaginatedList<Domain.Entities.Product>.CreateAsync(
            productsQuery,
            query.PagingParameters.Page,
            query.PagingParameters.PageSize
        );

        // Map to result DTOs
        var results = _mapper.Map<List<GetAllProductsResult>>(paginatedList);

        // Return new paginated list with mapped results
        return new PaginatedList<GetAllProductsResult>(
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
        GetAllProductsQuery filterParams)
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

        // Category filter with wildcard support
        if (!string.IsNullOrWhiteSpace(filterParams.CategoryFilter))
        {
            var categoryFilter = filterParams.CategoryFilter.Trim();
            
            if (categoryFilter.StartsWith('*') && categoryFilter.EndsWith('*'))
            {
                // Contains: *value*
                var value = categoryFilter.Trim('*');
                query = query.Where(p => p.Category.Contains(value));
            }
            else if (categoryFilter.StartsWith('*'))
            {
                // Ends with: *value
                var value = categoryFilter.TrimStart('*');
                query = query.Where(p => p.Category.EndsWith(value));
            }
            else if (categoryFilter.EndsWith('*'))
            {
                // Starts with: value*
                var value = categoryFilter.TrimEnd('*');
                query = query.Where(p => p.Category.StartsWith(value));
            }
            else
            {
                // Exact match
                query = query.Where(p => p.Category == categoryFilter);
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

