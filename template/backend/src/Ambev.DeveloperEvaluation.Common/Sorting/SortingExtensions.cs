using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.Common.Sorting;

/// <summary>
/// Extension methods for applying dynamic sorting to IQueryable
/// </summary>
public static class SortingExtensions
{
    /// <summary>
    /// Applies dynamic sorting to an IQueryable based on a sort string.
    /// Format: "property1 desc, property2 asc"
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    /// <param name="source">The source queryable</param>
    /// <param name="sortString">The sort string</param>
    /// <param name="propertyMapping">Mapping between string property names and actual properties</param>
    /// <returns>The ordered queryable</returns>
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, string? sortString, 
        Dictionary<string, Expression<Func<T, object>>> propertyMapping)
    {
        if (string.IsNullOrWhiteSpace(sortString))
            return source;

        var sortParts = sortString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var sortPart in sortParts)
        {
            var parts = sortPart.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            if (parts.Length == 0)
                continue;

            var propertyName = parts[0].ToLower();
            var isDescending = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            if (!propertyMapping.TryGetValue(propertyName, out var propertyExpression))
                continue;

            if (orderedQuery == null)
            {
                orderedQuery = isDescending 
                    ? source.OrderByDescending(propertyExpression)
                    : source.OrderBy(propertyExpression);
            }
            else
            {
                orderedQuery = isDescending
                    ? orderedQuery.ThenByDescending(propertyExpression)
                    : orderedQuery.ThenBy(propertyExpression);
            }
        }

        return orderedQuery ?? source;
    }
}






