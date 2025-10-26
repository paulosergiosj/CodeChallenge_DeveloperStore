namespace Ambev.DeveloperEvaluation.Application.Products.GetCategories;

/// <summary>
/// Result for category information
/// </summary>
public class GetCategoriesResult
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
