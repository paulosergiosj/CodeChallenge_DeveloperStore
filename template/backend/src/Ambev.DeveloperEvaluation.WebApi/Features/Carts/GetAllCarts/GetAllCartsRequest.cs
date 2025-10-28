using Ambev.DeveloperEvaluation.Common.Pagination;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetAllCarts;

/// <summary>
/// Request for retrieving all carts with pagination and sorting
/// </summary>
public class GetAllCartsRequest
{
    /// <summary>
    /// Gets or sets the page number (default: 1)
    /// </summary>
    public int _page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size (default: 10)
    /// </summary>
    public int _size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the sort string (e.g., "id desc, userId asc")
    /// </summary>
    public string? _order { get; set; }
}



