namespace CSharpDrills.Drills;

public class Expense
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public static class Drill14_SimpleExpenseTracker
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 14 - Simple Expense Tracker ===");
        Console.Write("How many expenses? ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Invalid count.");
            return;
        }

        List<Expense> expenses = new();

        for (int i = 0; i < count; i++)
        {
            Console.Write($"Expense {i + 1} name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write($"Expense {i + 1} amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            expenses.Add(new Expense { Name = name, Amount = amount });
        }

        decimal total = 0;
        decimal highest = expenses[0].Amount;
        string highestName = expenses[0].Name;

        foreach (var e in expenses)
        {
            total += e.Amount;
            if (e.Amount > highest)
            {
                highest = e.Amount;
                highestName = e.Name;
            }
        }

        decimal average = total / expenses.Count;
        Console.WriteLine($"Total: {total}, Average: {average}, Highest: {highestName} ({highest})");
    }
}
