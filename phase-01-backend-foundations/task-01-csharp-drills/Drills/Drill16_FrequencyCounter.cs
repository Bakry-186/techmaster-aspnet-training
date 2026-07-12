namespace CSharpDrills.Drills;

public static class Drill16_FrequencyCounter
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 16 - Frequency Counter ===");
        Console.Write("Enter numbers separated by comma: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("List cannot be empty.");
            return;
        }

        int[] numbers = input.Split(',').Select(x => int.Parse(x.Trim())).ToArray();
        Dictionary<int, int> freq = new();

        // Count each number as we loop
        foreach (int n in numbers)
        {
            if (freq.ContainsKey(n))
                freq[n]++;
            else
                freq[n] = 1;
        }

        foreach (var pair in freq)
            Console.WriteLine($"{pair.Key} => {pair.Value}");
    }
}
