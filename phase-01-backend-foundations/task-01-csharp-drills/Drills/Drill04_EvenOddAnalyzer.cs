namespace CSharpDrills.Drills;

public static class Drill04_EvenOddAnalyzer
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 04 - Even/Odd Analyzer ===");
        Console.Write("How many numbers? ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Count must be a positive number.");
            return;
        }

        List<int> evens = new();
        List<int> odds = new();

        for (int i = 0; i < count; i++)
        {
            Console.Write($"Number {i + 1}: ");
            if (!int.TryParse(Console.ReadLine(), out int num))
            {
                Console.WriteLine("Invalid number.");
                return;
            }

            if (num % 2 == 0) evens.Add(num);
            else odds.Add(num);
        }

        Console.WriteLine($"Even: {string.Join(",", evens)} | Odd: {string.Join(",", odds)}");
        Console.WriteLine($"Even count: {evens.Count} | Odd count: {odds.Count}");
    }
}
