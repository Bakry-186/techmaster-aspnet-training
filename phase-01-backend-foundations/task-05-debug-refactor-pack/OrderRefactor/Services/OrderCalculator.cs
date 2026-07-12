using OrderRefactor.Models;

namespace OrderRefactor.Services;

public class OrderCalculator
{
    private const decimal TaxRate = 0.14m;
    private const decimal ShippingFee = 50m;
    private const decimal FreeShippingThreshold = 1000m;

    public decimal CalculateSubtotal(decimal price, int quantity) => price * quantity;

    public decimal CalculateDiscount(decimal subtotal, CustomerType type)
    {
        return type switch
        {
            CustomerType.Silver => subtotal * 0.05m,
            CustomerType.Gold => subtotal * 0.10m,
            CustomerType.VIP => subtotal * 0.15m,
            _ => 0m
        };
    }

    public decimal CalculateTax(decimal afterDiscount) => afterDiscount * TaxRate;

    public decimal CalculateShipping(decimal afterDiscount)
        => afterDiscount >= FreeShippingThreshold ? 0m : ShippingFee;

    public decimal CalculateFinalTotal(Order order)
    {
        ValidationHelper.ValidateOrder(order);

        decimal subtotal = CalculateSubtotal(order.Price, order.Quantity);
        decimal discount = CalculateDiscount(subtotal, order.Customer.Type);
        decimal afterDiscount = subtotal - discount;
        decimal tax = CalculateTax(afterDiscount);
        decimal shipping = CalculateShipping(afterDiscount);
        return afterDiscount + tax + shipping;
    }

    public (decimal subtotal, decimal discount, decimal tax, decimal shipping, decimal finalTotal) GetBreakdown(Order order)
    {
        ValidationHelper.ValidateOrder(order);

        decimal subtotal = CalculateSubtotal(order.Price, order.Quantity);
        decimal discount = CalculateDiscount(subtotal, order.Customer.Type);
        decimal afterDiscount = subtotal - discount;
        decimal tax = CalculateTax(afterDiscount);
        decimal shipping = CalculateShipping(afterDiscount);
        decimal finalTotal = afterDiscount + tax + shipping;
        return (subtotal, discount, tax, shipping, finalTotal);
    }
}
