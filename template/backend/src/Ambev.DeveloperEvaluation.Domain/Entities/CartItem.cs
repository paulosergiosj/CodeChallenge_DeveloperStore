using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class CartItem : BaseEntity
{
    public Guid CartId { get; private set; }
    public Guid ProductRefId { get; private set; }
    public int ProductRefNumber { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public PurchaseDiscount PurchaseDiscount { get; private set; }

    private CartItem(Guid cartId, Guid productRefId, int productRefNumber, decimal unitPrice, int quantity)
    {
        CartId = cartId;
        ProductRefId = productRefId;
        ProductRefNumber = productRefNumber;
        UnitPrice = unitPrice;
        Quantity = quantity;
        PurchaseDiscount = new PurchaseDiscount(Quantity, UnitPrice);
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Factory method to create a new cart item with business rules validation
    /// </summary>
    /// <param name="cartId">The cart ID</param>
    /// <param name="productRefId">The product reference ID</param>
    /// <param name="productRefNumber">The product reference number</param>
    /// <param name="unitPrice">The unit price</param>
    /// <param name="quantity">The quantity</param>
    /// <returns>A new cart item</returns>
    public static CartItem Create(Guid cartId, Guid productRefId, int productRefNumber, decimal unitPrice, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        if (quantity > 20)
            throw new ArgumentException("Quantity cannot exceed 20 units per item.");

        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be greater than zero.");

        return new(cartId, productRefId, productRefNumber, unitPrice, quantity);
    }

    /// <summary>
    /// Updates the quantity of the item and recalculates discount
    /// </summary>
    /// <param name="newQuantity">The new quantity</param>
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        if (newQuantity > 20)
            throw new ArgumentException("Quantity cannot exceed 20 units per item.");

        Quantity = newQuantity;
        PurchaseDiscount = new PurchaseDiscount(Quantity, UnitPrice);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the total amount for this item including discount
    /// </summary>
    /// <returns>The total amount with discount applied</returns>
    public decimal TotalItemWithDiscount()
    {
        return (UnitPrice * Quantity) - PurchaseDiscount.Value;
    }

    /// <summary>
    /// Calculates the total amount for this item without discount
    /// </summary>
    /// <returns>The total amount without discount</returns>
    public decimal TotalItemWithoutDiscount()
    {
        return UnitPrice * Quantity;
    }
}
