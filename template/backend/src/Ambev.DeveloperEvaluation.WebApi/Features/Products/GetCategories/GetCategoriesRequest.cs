namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetCategories;

/// <summary>
/// Request for getting categories with pagination, sorting, and filtering
/// </summary>
public class GetCategoriesRequest
{
    /// <summary>
    /// Gets or sets the page number (default: 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size (default: 10)
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the sort string (e.g., "name asc")
    /// </summary>
    public string? SortString { get; set; }

    /// <summary>
    /// Gets or sets the category name filter (supports wildcards: *value or value*)
    /// </summary>
    public string? Name { get; set; }
}
