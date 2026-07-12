namespace CSharpDrills.Drills;

public static class Drill19_TicketPriceCalculator
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 19 - Ticket Price Calculator ===");
        const decimal basePrice = 100m;

        Console.Write("Enter age: ");
        if (!int.TryParse(Console.ReadLine(), out int age) || age < 0)
        {
            Console.WriteLine("Invalid age.");
            return;
        }

        Console.Write("Are you a student? (yes/no): ");
        string? studentInput = Console.ReadLine() ?? string.Empty;
        bool isStudent = studentInput.Equals("yes", StringComparison.OrdinalIgnoreCase);

        decimal discount = 0m;
        if (age < 12) discount = Math.Max(discount, 0.50m);
        if (age > 60) discount = Math.Max(discount, 0.30m);
        if (isStudent) discount = Math.Max(discount, 0.20m);

        decimal finalPrice = basePrice * (1 - discount);
        Console.WriteLine($"Final ticket price: {finalPrice:C}");
    }
}
