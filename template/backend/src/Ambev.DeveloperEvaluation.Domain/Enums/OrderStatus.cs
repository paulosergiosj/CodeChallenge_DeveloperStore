namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Represents the status of an order
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Order is pending confirmation
    /// </summary>
    Pending,

    /// <summary>
    /// Order is confirmed
    /// </summary>
    Confirmed,

    /// <summary>
    /// Order has been cancelled
    /// </summary>
    Cancelled
}



