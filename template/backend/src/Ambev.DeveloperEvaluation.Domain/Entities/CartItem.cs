//using Ambev.DeveloperEvaluation.Domain.Common;
//using Ambev.DeveloperEvaluation.Domain.ValueObjects;

//namespace Ambev.DeveloperEvaluation.Domain.Entities;

//public class CartItem : BaseEntity
//{
//    public Guid CartId { get; private set; }
//    public Guid ProductRefId { get; private set; }
//    public int ProductRefNumber { get; private set; }
//    public string ProductName { get; private set; }
//    public decimal UnitPrice { get; private set; }
//    public int Quantity { get; private set; }
//    public ProductDiscount ProductDiscount { get; private set; }

//    protected CartItem(Guid cartId, Guid productRefId, decimal unitPrice, int quantity)
//    {
//        CartId = cartId;
//        ProductRefId = productRefId;
//        UnitPrice = unitPrice;
//        Quantity = quantity;
//        ProductDiscount = new ProductDiscount(Quantity, UnitPrice);
//    }

//    public decimal TotalItemWithDiscount()
//    {
//        return (UnitPrice * Quantity) - ProductDiscount.Value;
//    }

//    protected static CartItem Create(Guid cartId, Guid productRefId, decimal unitPrice, int quantity)
//    {
//        if (quantity <= 0)
//        {
//            throw new ArgumentException("Quantity must be greater than zero.");
//        }

//        if (quantity > 20)
//        {
//            throw new ArgumentException("Quantity cannot exceed 20 units per item.");
//        }

//        return new(cartId, productRefId, unitPrice, quantity);
//    }
//}
