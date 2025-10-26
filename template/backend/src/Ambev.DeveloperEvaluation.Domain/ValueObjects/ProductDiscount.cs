namespace Ambev.DeveloperEvaluation.Domain.ValueObjects;

public class ProductDiscount
{
    public decimal Value { get; private set; } = 0;

    public ProductDiscount(int quantity, decimal unitPrice)
    {
        if (quantity >= 4 && quantity < 10)
        {
            Value = unitPrice * quantity * 0.10m; // 10% discount
        }
        else if (quantity >= 10 && quantity <= 20)
        {
            Value = unitPrice * quantity * 0.20m; // 20% discount
        }
    }
}
