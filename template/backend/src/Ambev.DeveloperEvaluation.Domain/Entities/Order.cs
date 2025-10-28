using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an order (sale) in the system
/// Orders are created from carts that have been checked out
/// </summary>
public class Order : BaseEntity
{
    /// <summary>
    /// Gets the order number
    /// </summary>
    public int OrderNumber { get; protected set; }

    /// <summary>
    /// Gets the date when the order was created
    /// </summary>
    public DateTime OrderDate { get; private set; }

    /// <summary>
    /// Gets the customer reference ID (external identity)
    /// </summary>
    public Guid CustomerRefId { get; private set; }

    /// <summary>
    /// Gets the branch reference ID (external identity)
    /// </summary>
    public Guid BranchRefId { get; private set; }

    /// <summary>
    /// Gets the total amount of the order
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets the order status
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// Gets the collection of items in the order
    /// </summary>
    public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

    /// <summary>
    /// Gets the cart ID from which this order was created
    /// </summary>
    public Guid CartRefId { get; private set; }

    // EF Core constructor
#pragma warning disable CS8618
    protected Order() { }
#pragma warning restore CS8618

    private Order(Guid customerRefId, Guid branchRefId, Guid cartRefId)
    {
        CustomerRefId = customerRefId;
        BranchRefId = branchRefId;
        CartRefId = cartRefId;
        OrderDate = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Creates a new order from a checked out cart
    /// </summary>
    /// <param name="cart">The checked out cart</param>
    /// <param name="customerRefId">The customer ID</param>
    /// <param name="branchRefId">The branch ID</param>
    /// <returns>A new order</returns>
    public static Order Create(Cart cart, Guid customerRefId, Guid branchRefId)
    {
        if (cart.Status != CartStatus.CheckedOut)
            throw new InvalidOperationException("Only checked out carts can create orders");

        if (cart.Items.Count == 0)
            throw new InvalidOperationException("Cannot create an order from an empty cart");

        var order = new Order(customerRefId, branchRefId, cart.Id);

        foreach (var cartItem in cart.Items)
        {
            var orderItem = OrderItem.Create(
                order.Id,
                cartItem.ProductRefId,
                cartItem.ProductRefNumber,
                cartItem.UnitPrice,
                cartItem.Quantity,
                cartItem.PurchaseDiscount.Value);

            order.Items.Add(orderItem);
        }

        order.TotalAmount = cart.GetTotalAmount();

        return order;
    }

    /// <summary>
    /// Confirms the order
    /// </summary>
    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed");

        Status = OrderStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the order
    /// </summary>
    public void Cancel()
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Order is already cancelled");

        if (Status == OrderStatus.Confirmed)
            throw new InvalidOperationException("Cannot cancel a confirmed order");

        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}
