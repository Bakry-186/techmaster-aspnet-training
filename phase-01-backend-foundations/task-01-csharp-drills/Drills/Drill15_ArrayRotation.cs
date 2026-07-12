namespace CSharpDrills.Drills;

public static class Drill15_ArrayRotation
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 15 - Array Rotation ===");
        Console.Write("Enter numbers separated by comma: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Array cannot be empty.");
            return;
        }

        int[] arr = input.Split(',').Select(x => int.Parse(x.Trim())).ToArray();

        if (arr.Length == 1)
        {
            Console.WriteLine($"[{string.Join(",", arr)}]");
            return;
        }

        int last = arr[^1];
        for (int i = arr.Length - 1; i > 0; i--)
            arr[i] = arr[i - 1];
        arr[0] = last;

        Console.WriteLine($"[{string.Join(",", arr)}]");
    }
}
