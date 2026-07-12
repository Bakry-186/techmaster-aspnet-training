namespace CSharpDrills.Drills;

// Refactored versions of drills 02, 03, and 10 with smaller methods
public static class Drill20_MethodRefactoringChallenge
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 20 - Method Refactoring Challenge ===");
        Console.WriteLine("1. Grade Calculator (refactored)");
        Console.WriteLine("2. Login Validator (refactored)");
        Console.WriteLine("3. ATM Menu (refactored)");
        Console.Write("Choose: ");

        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1": RunGradeRefactored(); break;
            case "2": RunLoginRefactored(); break;
            case "3": RunAtmRefactored(); break;
            default: Console.WriteLine("Invalid choice."); break;
        }
    }

    private static void RunGradeRefactored()
    {
        int? score = ReadScore();
        if (score is null) return;

        if (!ValidateScore(score.Value))
            return;

        string grade = CalculateGrade(score.Value);
        PrintGrade(grade);
    }

    private static int? ReadScore()
    {
        Console.Write("Enter score (0-100): ");
        if (!int.TryParse(Console.ReadLine(), out int score))
        {
            Console.WriteLine("Score must be a valid number.");
            return null;
        }
        return score;
    }

    private static bool ValidateScore(int score)
    {
        if (score < 0 || score > 100)
        {
            Console.WriteLine("Score must be between 0 and 100.");
            return false;
        }
        return true;
    }

    private static string CalculateGrade(int score)
    {
        if (score >= 90) return "A";
        if (score >= 80) return "B";
        if (score >= 70) return "C";
        if (score >= 60) return "D";
        return "F";
    }

    private static void PrintGrade(string grade) => Console.WriteLine($"Grade: {grade}");

    private static void RunLoginRefactored()
    {
        const string user = "admin";
        const string pass = "1234";

        for (int i = 1; i <= 3; i++)
        {
            var creds = ReadCredentials();
            if (IsValidLogin(creds.user, creds.pass, user, pass))
            {
                Console.WriteLine("Login successful.");
                return;
            }
            Console.WriteLine($"Attempt {i} failed.");
        }

        Console.WriteLine("Account locked. Too many failed attempts.");
    }

    private static (string user, string pass) ReadCredentials()
    {
        Console.Write("Username: ");
        string u = Console.ReadLine() ?? string.Empty;
        Console.Write("Password: ");
        string p = Console.ReadLine() ?? string.Empty;
        return (u, p);
    }

    private static bool IsValidLogin(string user, string pass, string expectedUser, string expectedPass)
    {
        bool userOk = string.Equals(user, expectedUser, StringComparison.OrdinalIgnoreCase);
        bool passOk = string.Equals(pass, expectedPass, StringComparison.Ordinal);
        return userOk && passOk;
    }

    private static void RunAtmRefactored()
    {
        decimal balance = 1000m;
        bool exit = false;

        while (!exit)
        {
            ShowMenu();
            if (!int.TryParse(Console.ReadLine(), out int option))
            {
                Console.WriteLine("Invalid option.");
                continue;
            }

            switch (option)
            {
                case 1: ShowBalance(balance); break;
                case 2: balance = DoDeposit(balance); break;
                case 3: balance = DoWithdraw(balance); break;
                case 4: exit = true; break;
                default: Console.WriteLine("Invalid option."); break;
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("\n1. Check Balance");
        Console.WriteLine("2. Deposit");
        Console.WriteLine("3. Withdraw");
        Console.WriteLine("4. Exit");
        Console.Write("Choose: ");
    }

    private static void ShowBalance(decimal balance) => Console.WriteLine($"Balance: {balance:C}");

    private static decimal DoDeposit(decimal balance)
    {
        decimal? amount = ReadAmount("Deposit amount: ");
        if (amount is null or <= 0)
        {
            Console.WriteLine("Amount must be positive.");
            return balance;
        }
        return balance + amount.Value;
    }

    private static decimal DoWithdraw(decimal balance)
    {
        decimal? amount = ReadAmount("Withdraw amount: ");
        if (amount is null or <= 0)
        {
            Console.WriteLine("Amount must be positive.");
            return balance;
        }
        if (amount > balance)
        {
            Console.WriteLine("Insufficient balance.");
            return balance;
        }
        return balance - amount.Value;
    }

    private static decimal? ReadAmount(string prompt)
    {
        Console.Write(prompt);
        return decimal.TryParse(Console.ReadLine(), out decimal value) ? value : null;
    }
}
