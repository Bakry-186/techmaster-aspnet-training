namespace CSharpDrills.Drills;

public static class Drill03_LoginValidator
{
    private const string Username = "admin";
    private const string Password = "1234";

    public static void Run()
    {
        Console.WriteLine("=== Drill 03 - Login Validator ===");

        // Allow up to 3 attempts before locking the account
        for (int attempt = 1; attempt <= 3; attempt++)
        {
            Console.Write("Username: ");
            string? username = Console.ReadLine();
            Console.Write("Password: ");
            string? password = Console.ReadLine();

            bool userOk = string.Equals(username, Username, StringComparison.OrdinalIgnoreCase);
            bool passOk = string.Equals(password, Password, StringComparison.Ordinal);

            if (userOk && passOk)
            {
                Console.WriteLine("Login successful.");
                return;
            }

            Console.WriteLine($"Attempt {attempt} failed.");
        }

        Console.WriteLine("Account locked. Too many failed attempts.");
    }
}
