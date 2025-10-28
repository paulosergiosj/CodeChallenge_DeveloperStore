using MediatR;
using Ambev.DeveloperEvaluation.Common.Pagination;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetAllCarts;

/// <summary>
/// Query for retrieving all carts with pagination and sorting
/// </summary>
public class GetAllCartsQuery : IRequest<PaginatedList<GetAllCartsResult>>
{
    /// <summary>
    /// Gets or sets the pagination parameters
    /// </summary>
    public PagingParameters PagingParameters { get; set; } = new();

    /// <summary>
    /// Gets or sets the sort string (e.g., "id desc, userId asc")
    /// </summary>
    public string? SortString { get; set; }
}

