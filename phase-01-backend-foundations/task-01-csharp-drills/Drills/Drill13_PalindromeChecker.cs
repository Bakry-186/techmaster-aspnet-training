namespace CSharpDrills.Drills;

public static class Drill13_PalindromeChecker
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 13 - Palindrome Checker ===");
        Console.Write("Enter text: ");
        string? text = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Input cannot be empty.");
            return;
        }

        string cleaned = text.ToLower().Replace(" ", "");
        char[] chars = cleaned.ToCharArray();
        Array.Reverse(chars);
        string reversed = new string(chars);

        if (cleaned == reversed)
            Console.WriteLine("Palindrome");
        else
            Console.WriteLine("Not Palindrome");
    }
}
