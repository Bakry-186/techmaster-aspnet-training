namespace CSharpDrills.Drills;

public static class Drill12_EmailValidator
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 12 - Email Validator ===");
        Console.Write("Enter email: ");
        string? email = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Invalid - email is empty.");
            return;
        }

        if (email.Contains(' '))
        {
            Console.WriteLine("Invalid - email cannot contain spaces.");
            return;
        }

        if (email.StartsWith('@') || email.EndsWith('@'))
        {
            Console.WriteLine("Invalid - email cannot start or end with @.");
            return;
        }

        if (!email.Contains('@'))
        {
            Console.WriteLine("Invalid - missing @.");
            return;
        }

        if (!email.Contains('.'))
        {
            Console.WriteLine("Invalid - missing dot.");
            return;
        }

        Console.WriteLine("Valid email.");
    }
}
