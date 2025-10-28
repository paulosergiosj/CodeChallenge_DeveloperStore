using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid UserRefId { get; private set; }

    public ICollection<CartItem> Items { get; private set; } = [];

    public CartStatus Status { get; private set; } = CartStatus.Active;

    private Cart(Guid userRefId)
    {
        UserRefId = userRefId;
        CreatedAt = DateTime.UtcNow;
    }

    public static Cart Create(Guid userRefId)
    {
        return new Cart(userRefId);
    }

    public void SetCheckedOut()
    {
        Status = CartStatus.CheckedOut;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds an item to the cart with business rules validation
    /// </summary>
    /// <param name="productRefId">The product reference ID</param>
    /// <param name="productRefNumber">The product reference number</param>
    /// <param name="unitPrice">The unit price of the product</param>
    /// <param name="quantity">The quantity to add</param>
    /// <returns>The created cart item</returns>
    public CartItem AddItem(Guid productRefId, int productRefNumber, decimal unitPrice, int quantity)
    {
        var existingItem = Items.FirstOrDefault(item => item.ProductRefNumber == productRefNumber);

        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            return existingItem;
        }

        // Create new cart item using factory method
        var cartItem = CartItem.Create(Id, productRefId, productRefNumber, unitPrice, quantity);
        Items.Add(cartItem);

        return cartItem;
    }

    /// <summary>
    /// Removes an item from the cart
    /// </summary>
    /// <param name="productRefNumber">The product reference number</param>
    public void RemoveItem(int productRefNumber)
    {
        var item = Items.FirstOrDefault(i => i.ProductRefNumber == productRefNumber);
        if (item != null)
        {
            Items.Remove(item);
        }
    }

    /// <summary>
    /// Updates the quantity of an existing item
    /// </summary>
    /// <param name="productRefNumber">The product reference number</param>
    /// <param name="newQuantity">The new quantity</param>
    public void UpdateItemQuantity(int productRefNumber, int newQuantity)
    {
        var item = Items.FirstOrDefault(i => i.ProductRefNumber == productRefNumber);
        item?.UpdateQuantity(newQuantity);
    }

    /// <summary>
    /// Calculates the total amount of the cart including discounts
    /// </summary>
    /// <returns>The total amount</returns>
    public decimal GetTotalAmount()
    {
        return Items.Sum(item => item.TotalItemWithDiscount());
    }

    /// <summary>
    /// Gets the total number of items in the cart
    /// </summary>
    /// <returns>The total item count</returns>
    public int GetTotalItemCount()
    {
        return Items.Sum(item => item.Quantity);
    }

    /// <summary>
    /// Checks if the cart is empty
    /// </summary>
    /// <returns>True if cart is empty, false otherwise</returns>
    public bool IsEmpty()
    {
        return Items.Count == 0;
    }

    private bool IsActive => Status == CartStatus.Active;

    public bool CanDelete()
        => IsActive;

    public bool CanUpdate()
        => IsActive;

    public bool CanBeCheckedOut()
        => IsActive && !IsEmpty();

    public bool CanBeFinalized()
        => Status == CartStatus.CheckedOut;

    public void SetFinalized()
    {
        if (!CanBeFinalized())
            throw new InvalidOperationException("Cart cannot be finalized in its current state.");
        Status = CartStatus.Finalized;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Clears all items from the cart
    /// </summary>
    public void Clear()
    {
        Items.Clear();
    }
}
