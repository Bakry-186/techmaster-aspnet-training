namespace CSharpDrills.Drills;

public static class Drill06_WordCounter
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 06 - Word Counter ===");
        Console.Write("Enter a sentence: ");
        string? sentence = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(sentence))
        {
            Console.WriteLine("Sentence cannot be empty.");
            return;
        }

        string[] words = sentence.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Console.WriteLine($"Word count: {words.Length}");
    }
}
