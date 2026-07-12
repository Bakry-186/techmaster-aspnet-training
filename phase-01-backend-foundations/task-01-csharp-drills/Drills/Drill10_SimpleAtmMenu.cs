namespace CSharpDrills.Drills;

public static class Drill10_SimpleAtmMenu
{
    public static void Run()
    {
        Console.WriteLine("=== Drill 10 - Simple ATM Menu ===");
        decimal balance = 1000m;
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n1. Check Balance");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Exit");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid option.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    PrintBalance(balance);
                    break;
                case 2:
                    balance = Deposit(balance);
                    break;
                case 3:
                    balance = Withdraw(balance);
                    break;
                case 4:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static void PrintBalance(decimal balance)
    {
        Console.WriteLine($"Balance: {balance:C}");
    }

    private static decimal Deposit(decimal balance)
    {
        Console.Write("Deposit amount: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
        {
            Console.WriteLine("Amount must be positive.");
            return balance;
        }

        return balance + amount;
    }

    private static decimal Withdraw(decimal balance)
    {
        Console.Write("Withdraw amount: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
        {
            Console.WriteLine("Amount must be positive.");
            return balance;
        }

        if (amount > balance)
        {
            Console.WriteLine("Insufficient balance.");
            return balance;
        }

        return balance - amount;
    }
}
