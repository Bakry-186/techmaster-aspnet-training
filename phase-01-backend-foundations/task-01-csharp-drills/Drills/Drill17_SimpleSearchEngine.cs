namespace CSharpDrills.Drills;

public static class Drill17_SimpleSearchEngine
{
    private static readonly List<string> Names =
    [
        "Ali Hassan",
        "Khaled Ali",
        "Sara Adel",
        "Mohamed Ayman",
        "Nour Emad"
    ];

    public static void Run()
    {
        Console.WriteLine("=== Drill 17 - Simple Search Engine ===");
        Console.Write("Search keyword: ");
        string? keyword = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(keyword))
        {
            Console.WriteLine("Keyword cannot be empty.");
            return;
        }

        var results = Names
            .Where(n => n.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (results.Count == 0)
            Console.WriteLine("No results found.");
        else
            foreach (string name in results)
                Console.WriteLine(name);
    }
}
