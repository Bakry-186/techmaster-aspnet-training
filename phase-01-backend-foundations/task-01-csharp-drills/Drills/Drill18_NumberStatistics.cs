namespace CSharpDrills.Drills;

public static class Drill18_NumberStatistics
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 18 - Number Statistics ===");
        Console.Write("Enter numbers separated by comma: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("List cannot be empty.");
            return;
        }

        List<int> numbers = input.Split(',').Select(x => int.Parse(x.Trim())).ToList();

        int sum = 0;
        int positives = 0;
        int negatives = 0;
        int min = numbers[0];
        int max = numbers[0];

        foreach (int n in numbers)
        {
            sum += n;
            if (n > 0) positives++;
            if (n < 0) negatives++;
            if (n < min) min = n;
            if (n > max) max = n;
        }

        double average = (double)sum / numbers.Count;
        Console.WriteLine($"Count: {numbers.Count}, Sum: {sum}, Average: {average:F2}");
        Console.WriteLine($"Max: {max}, Min: {min}, Positives: {positives}, Negatives: {negatives}");
    }
}
