namespace CSharpDrills.Drills;

public static class Drill08_PasswordStrengthChecker
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 08 - Password Strength Checker ===");
        Console.Write("Enter password: ");
        string? password = Console.ReadLine() ?? string.Empty;

        bool hasLength = password.Length >= 8;
        bool hasUpper = false;
        bool hasLower = false;
        bool hasDigit = false;
        bool hasSpecial = false;

        // Loop characters once and set flags
        foreach (char c in password)
        {
            if (char.IsUpper(c)) hasUpper = true;
            else if (char.IsLower(c)) hasLower = true;
            else if (char.IsDigit(c)) hasDigit = true;
            else hasSpecial = true;
        }

        List<string> missing = new();
        if (!hasLength) missing.Add("length (8+)");
        if (!hasUpper) missing.Add("uppercase");
        if (!hasLower) missing.Add("lowercase");
        if (!hasDigit) missing.Add("digit");
        if (!hasSpecial) missing.Add("special character");

        if (missing.Count == 0)
            Console.WriteLine("Strong");
        else
            Console.WriteLine($"Weak - missing {string.Join(", ", missing)}");
    }
}
