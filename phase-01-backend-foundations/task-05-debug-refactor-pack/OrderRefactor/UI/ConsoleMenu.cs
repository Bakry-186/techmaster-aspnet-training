using OrderRefactor.Models;
using OrderRefactor.Services;

namespace OrderRefactor.UI;

public class ConsoleMenu
{
    private readonly OrderCalculator _calculator = new();

    public void Run()
    {
        try
        {
            var order = ReadOrder();
            var result = _calculator.GetBreakdown(order);
            ReceiptPrinter.Print(order, result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static Order ReadOrder()
    {
        Console.Write("Customer name: ");
        string customerName = Console.ReadLine() ?? string.Empty;

        Console.Write("Product name: ");
        string productName = Console.ReadLine() ?? string.Empty;

        Console.Write("Product price: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            throw new ArgumentException("Invalid price.");

        Console.Write("Quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity))
            throw new ArgumentException("Invalid quantity.");

        Console.Write("Customer type (Regular/Silver/Gold/VIP): ");
        string typeInput = Console.ReadLine() ?? "Regular";
        if (!Enum.TryParse<CustomerType>(typeInput, true, out var type))
            type = CustomerType.Regular;

        return new Order
        {
            Customer = new Customer { Name = customerName, Type = type },
            ProductName = productName,
            Price = price,
            Quantity = quantity
        };
    }
}

public static class ReceiptPrinter
{
    public static void Print(Order order, (decimal subtotal, decimal discount, decimal tax, decimal shipping, decimal finalTotal) result)
    {
        Console.WriteLine("\n--- Order Receipt ---");
        Console.WriteLine($"Customer: {order.Customer.Name}");
        Console.WriteLine($"Product: {order.ProductName}");
        Console.WriteLine($"Price: {order.Price:C}");
        Console.WriteLine($"Quantity: {order.Quantity}");
        Console.WriteLine($"Subtotal: {result.subtotal:C}");
        Console.WriteLine($"Discount: {result.discount:C}");
        Console.WriteLine($"Tax: {result.tax:C}");
        Console.WriteLine($"Shipping: {result.shipping:C}");
        Console.WriteLine($"Final Total: {result.finalTotal:C}");
    }
}
