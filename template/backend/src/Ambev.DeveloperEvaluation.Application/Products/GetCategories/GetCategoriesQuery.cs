using MediatR;
using Ambev.DeveloperEvaluation.Common.Pagination;

namespace Ambev.DeveloperEvaluation.Application.Products.GetCategories;

/// <summary>
/// Query for retrieving categories with pagination, sorting, and filtering
/// </summary>
public class GetCategoriesQuery : IRequest<PaginatedList<GetCategoriesResult>>
{
    /// <summary>
    /// Gets or sets the pagination parameters
    /// </summary>
    public PagingParameters PagingParameters { get; set; } = new();

    /// <summary>
    /// Gets or sets the sort string (e.g., "name asc")
    /// </summary>
    public string? SortString { get; set; }

    /// <summary>
    /// Gets or sets the category name filter (supports wildcards: *value or value*)
    /// </summary>
    public string? NameFilter { get; set; }
}

