using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in an order (sale)
/// </summary>
public class OrderItem : BaseEntity
{
    /// <summary>
    /// Gets the order ID
    /// </summary>
    public Guid OrderId { get; private set; }

    /// <summary>
    /// Gets the product reference ID (external identity)
    /// </summary>
    public Guid ProductRefId { get; private set; }

    /// <summary>
    /// Gets the product reference number (external identity)
    /// </summary>
    public int ProductRefNumber { get; private set; }

    /// <summary>
    /// Gets the unit price at the time of purchase
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the quantity purchased
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the discount amount applied
    /// </summary>
    public decimal DiscountAmount { get; private set; }

    /// <summary>
    /// Gets the total amount for this item
    /// </summary>
    public decimal TotalAmount { get; private set; }

    private OrderItem(Guid orderId, Guid productRefId, int productRefNumber, decimal unitPrice, int quantity, decimal discountAmount)
    {
        OrderId = orderId;
        ProductRefId = productRefId;
        ProductRefNumber = productRefNumber;
        UnitPrice = unitPrice;
        Quantity = quantity;
        DiscountAmount = discountAmount;
        TotalAmount = (unitPrice * quantity) - discountAmount;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Factory method to create a new order item
    /// </summary>
    /// <param name="orderId">The order ID</param>
    /// <param name="productRefId">The product reference ID</param>
    /// <param name="productRefNumber">The product reference number</param>
    /// <param name="unitPrice">The unit price at the time of purchase</param>
    /// <param name="quantity">The quantity purchased</param>
    /// <param name="discountAmount">The discount amount applied</param>
    /// <returns>A new order item</returns>
    internal static OrderItem Create(
        Guid orderId,
        Guid productRefId,
        int productRefNumber,
        decimal unitPrice,
        int quantity,
        decimal discountAmount)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be greater than zero.");

        if (discountAmount < 0)
            throw new ArgumentException("Discount amount cannot be negative.");

        return new OrderItem(orderId, productRefId, productRefNumber, unitPrice, quantity, discountAmount);
    }
}



