namespace CSharpDrills.Drills;

public static class Drill07_NameFormatter
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 07 - Name Formatter ===");
        Console.Write("Enter full name: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        string[] parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        List<string> formatted = new();

        foreach (string part in parts)
        {
            string lower = part.ToLower();
            string result = char.ToUpper(lower[0]) + lower.Substring(1);
            formatted.Add(result);
        }

        Console.WriteLine(string.Join(' ', formatted));
    }
}
