using MediatR;
using Ambev.DeveloperEvaluation.Common.Pagination;

namespace Ambev.DeveloperEvaluation.Application.Products.GetAllProducts;

/// <summary>
/// Query for retrieving all products with pagination, sorting, and filtering
/// </summary>
public class GetAllProductsQuery : IRequest<PaginatedList<GetAllProductsResult>>
{
    /// <summary>
    /// Gets or sets the pagination parameters
    /// </summary>
    public PagingParameters PagingParameters { get; set; } = new();

    /// <summary>
    /// Gets or sets the sort string (e.g., "price desc, title asc")
    /// </summary>
    public string? SortString { get; set; }

    /// <summary>
    /// Gets or sets the title filter (supports wildcards: *value or value*)
    /// </summary>
    public string? TitleFilter { get; set; }

    /// <summary>
    /// Gets or sets the category filter (supports wildcards: *value or value*)
    /// </summary>
    public string? CategoryFilter { get; set; }

    /// <summary>
    /// Gets or sets the minimum price filter
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Gets or sets the maximum price filter
    /// </summary>
    public decimal? MaxPrice { get; set; }
}

