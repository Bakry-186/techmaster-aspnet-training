namespace CSharpDrills.Drills;

public static class Drill05_MaxMinFinder
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 05 - Max/Min Finder ===");
        Console.Write("Enter numbers separated by comma: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("List cannot be empty.");
            return;
        }

        string[] parts = input.Split(',');
        List<int> numbers = new();

        foreach (string part in parts)
        {
            if (!int.TryParse(part.Trim(), out int n))
            {
                Console.WriteLine("Invalid number in list.");
                return;
            }
            numbers.Add(n);
        }

        int max = numbers[0];
        int min = numbers[0];

        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] > max) max = numbers[i];
            if (numbers[i] < min) min = numbers[i];
        }

        Console.WriteLine($"Max: {max} | Min: {min}");

        int linqMax = numbers.Max();
        int linqMin = numbers.Min();
        Console.WriteLine($"LINQ check -> Max: {linqMax} | Min: {linqMin}");
    }
}
