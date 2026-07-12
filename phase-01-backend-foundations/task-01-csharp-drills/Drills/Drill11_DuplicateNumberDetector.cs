namespace CSharpDrills.Drills;

public static class Drill11_DuplicateNumberDetector
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 11 - Duplicate Number Detector ===");
        Console.Write("Enter numbers separated by comma: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("List cannot be empty.");
            return;
        }

        List<int> numbers = input.Split(',')
            .Select(x => int.Parse(x.Trim()))
            .ToList();

        HashSet<int> seen = new();
        HashSet<int> duplicates = new();

        foreach (int n in numbers)
        {
            if (!seen.Add(n))
                duplicates.Add(n);
        }

        if (duplicates.Count == 0)
            Console.WriteLine("No duplicates found.");
        else
            Console.WriteLine($"Duplicates: {string.Join(", ", duplicates)}");
    }
}
