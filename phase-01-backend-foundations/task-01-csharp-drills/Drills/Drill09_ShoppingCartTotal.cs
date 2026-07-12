namespace CSharpDrills.Drills;

public static class Drill09_ShoppingCartTotal
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 09 - Shopping Cart Total ===");
        Console.Write("How many items? ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Invalid item count.");
            return;
        }

        decimal total = 0;

        for (int i = 0; i < count; i++)
        {
            Console.Write($"Item {i + 1} price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            Console.Write($"Item {i + 1} quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                return;
            }

            total += price * qty;
        }

        decimal discount = total > 1000 ? total * 0.10m : 0;
        decimal final = total - discount;

        Console.WriteLine($"Subtotal: {total:C}");
        Console.WriteLine($"Discount: {discount:C}");
        Console.WriteLine($"Final: {final:C}");
    }
}
