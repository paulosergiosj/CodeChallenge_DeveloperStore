namespace Ambev.DeveloperEvaluation.Common.Pagination;

/// <summary>
/// Parameters for pagination functionality
/// </summary>
public class PagingParameters
{
    private const int MaxPageSize = 100;
    private int _page = 1;
    private int _pageSize = 10;

    /// <summary>
    /// Gets or sets the page number (1-based). Default is 1.
    /// </summary>
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    /// <summary>
    /// Gets or sets the page size. Default is 10, maximum is 100.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value < 1 ? 1 : value;
    }
}






