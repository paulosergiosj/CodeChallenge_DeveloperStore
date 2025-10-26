using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Common.Sorting;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Products.GetCategories;

/// <summary>
/// Handler for processing GetCategoriesQuery requests
/// </summary>
public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, PaginatedList<GetCategoriesResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetCategoriesHandler
    /// </summary>
    /// <param name="unitOfWork">The unit of work instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetCategoriesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetCategoriesQuery request
    /// </summary>
    /// <param name="request">The GetCategories query</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of categories</returns>
    public async Task<PaginatedList<GetCategoriesResult>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        // Get distinct categories with product counts
        var categoriesQuery = _unitOfWork.Products
            .Query()
            .GroupBy(p => p.Category)
            .Select(g => new GetCategoriesResult
            {
                Name = g.Key,
                ProductCount = g.Count()
            });

        // Apply filters
        categoriesQuery = ApplyFilters(categoriesQuery, request);

        // Apply sorting
        categoriesQuery = ApplySorting(categoriesQuery, request.SortString);

        // Create paginated list
        var paginatedList = await PaginatedList<GetCategoriesResult>.CreateAsync(
            categoriesQuery,
            request.PagingParameters.Page,
            request.PagingParameters.PageSize
        );

        return paginatedList;
    }

    /// <summary>
    /// Applies filters to the query
    /// </summary>
    private IQueryable<GetCategoriesResult> ApplyFilters(
        IQueryable<GetCategoriesResult> query,
        GetCategoriesQuery filterParams)
    {
        // Name filter with wildcard support
        if (!string.IsNullOrWhiteSpace(filterParams.NameFilter))
        {
            var nameFilter = filterParams.NameFilter.Trim();
            
            if (nameFilter.StartsWith('*') && nameFilter.EndsWith('*'))
            {
                // Contains: *value*
                var value = nameFilter.Trim('*');
                query = query.Where(c => c.Name.Contains(value));
            }
            else if (nameFilter.StartsWith('*'))
            {
                // Ends with: *value
                var value = nameFilter.TrimStart('*');
                query = query.Where(c => c.Name.EndsWith(value));
            }
            else if (nameFilter.EndsWith('*'))
            {
                // Starts with: value*
                var value = nameFilter.TrimEnd('*');
                query = query.Where(c => c.Name.StartsWith(value));
            }
            else
            {
                // Exact match
                query = query.Where(c => c.Name == nameFilter);
            }
        }

        return query;
    }

    /// <summary>
    /// Applies sorting to the query
    /// </summary>
    private IQueryable<GetCategoriesResult> ApplySorting(
        IQueryable<GetCategoriesResult> query,
        string? sortString)
    {
        if (string.IsNullOrWhiteSpace(sortString))
            return query.OrderBy(c => c.Name);

        var sortParts = sortString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        IOrderedQueryable<GetCategoriesResult>? orderedQuery = null;

        foreach (var sortPart in sortParts)
        {
            var parts = sortPart.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            if (parts.Length == 0)
                continue;

            var propertyName = parts[0].ToLower();
            var isDescending = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            if (orderedQuery == null)
            {
                orderedQuery = propertyName switch
                {
                    "name" or "category" => isDescending 
                        ? query.OrderByDescending(c => c.Name)
                        : query.OrderBy(c => c.Name),
                    "count" or "productcount" => isDescending
                        ? query.OrderByDescending(c => c.ProductCount)
                        : query.OrderBy(c => c.ProductCount),
                    _ => query.OrderBy(c => c.Name)
                };
            }
            else
            {
                orderedQuery = propertyName switch
                {
                    "name" or "category" => isDescending
                        ? orderedQuery.ThenByDescending(c => c.Name)
                        : orderedQuery.ThenBy(c => c.Name),
                    "count" or "productcount" => isDescending
                        ? orderedQuery.ThenByDescending(c => c.ProductCount)
                        : orderedQuery.ThenBy(c => c.ProductCount),
                    _ => orderedQuery.ThenBy(c => c.Name)
                };
            }
        }

        return orderedQuery ?? query.OrderBy(c => c.Name);
    }
}
