using OrderRefactor.Models;

namespace OrderRefactor.Services;

public static class ValidationHelper
{
    public static void ValidateOrder(Order order)
    {
        if (string.IsNullOrWhiteSpace(order.Customer.Name))
            throw new ArgumentException("Customer name is required.");
        if (string.IsNullOrWhiteSpace(order.ProductName))
            throw new ArgumentException("Product name is required.");
        if (order.Price <= 0)
            throw new ArgumentException("Price must be positive.");
        if (order.Quantity <= 0)
            throw new ArgumentException("Quantity must be positive.");
    }
}
