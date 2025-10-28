namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetAllProducts;

/// <summary>
/// Request for getting all products with optional filtering
/// </summary>
public class GetAllProductsRequest
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
    /// Gets or sets the sort string (e.g., "price desc, title asc")
    /// </summary>
    public string? SortString { get; set; }

    /// <summary>
    /// Gets or sets the title filter (supports wildcards: *value or value*)
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the category filter (supports wildcards: *value or value*)
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the minimum price filter
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Gets or sets the maximum price filter
    /// </summary>
    public decimal? MaxPrice { get; set; }
}






