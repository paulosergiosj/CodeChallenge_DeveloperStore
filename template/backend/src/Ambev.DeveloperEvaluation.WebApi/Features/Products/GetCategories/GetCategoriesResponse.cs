namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetCategories;

/// <summary>
/// Response for category information
/// </summary>
public class GetCategoriesResponse
{
    /// <summary>
    /// Gets or sets the category name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of products in this category
    /// </summary>
    public int ProductCount { get; set; }
}
