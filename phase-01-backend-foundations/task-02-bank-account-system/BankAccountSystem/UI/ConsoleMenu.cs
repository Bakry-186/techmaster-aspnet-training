using BankAccountSystem.Models;
using BankAccountSystem.Services;

namespace BankAccountSystem.UI;

public class ConsoleMenu
{
    private readonly BankService _bank = new();

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\n====== TechMaster Bank System ======");
            Console.WriteLine("1. Create Customer Account");
            Console.WriteLine("2. Deposit Money");
            Console.WriteLine("3. Withdraw Money");
            Console.WriteLine("4. Transfer Money");
            Console.WriteLine("5. View Account Details");
            Console.WriteLine("6. View Transaction History");
            Console.WriteLine("7. View All Accounts");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid option.");
                continue;
            }

            try
            {
                switch (choice)
                {
                    case 1: CreateAccount(); break;
                    case 2: Deposit(); break;
                    case 3: Withdraw(); break;
                    case 4: Transfer(); break;
                    case 5: ViewAccount(); break;
                    case 6: ViewHistory(); break;
                    case 7: ViewAll(); break;
                    case 8: return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private void CreateAccount()
    {
        Console.Write("Full name: ");
        string name = Console.ReadLine() ?? string.Empty;
        Console.Write("Email: ");
        string email = Console.ReadLine() ?? string.Empty;
        Console.Write("Phone: ");
        string phone = Console.ReadLine() ?? string.Empty;
        Console.Write("Initial balance: ");
        decimal.TryParse(Console.ReadLine(), out decimal balance);
        Console.Write("Account type (1=Savings, 2=Checking): ");
        int.TryParse(Console.ReadLine(), out int typeChoice);
        var type = typeChoice == 2 ? AccountType.Checking : AccountType.Savings;

        var account = _bank.CreateAccount(name, email, phone, balance, type);
        Console.WriteLine($"Account created. Number: {account.AccountNumber}");
    }

    private void Deposit()
    {
        Console.Write("Account number: ");
        string acc = Console.ReadLine() ?? string.Empty;
        Console.Write("Amount: ");
        decimal.TryParse(Console.ReadLine(), out decimal amount);
        _bank.Deposit(acc, amount);
        Console.WriteLine("Deposit successful.");
    }

    private void Withdraw()
    {
        Console.Write("Account number: ");
        string acc = Console.ReadLine() ?? string.Empty;
        Console.Write("Amount: ");
        decimal.TryParse(Console.ReadLine(), out decimal amount);
        _bank.Withdraw(acc, amount);
        Console.WriteLine("Withdrawal successful.");
    }

    private void Transfer()
    {
        Console.Write("From account: ");
        string from = Console.ReadLine() ?? string.Empty;
        Console.Write("To account: ");
        string to = Console.ReadLine() ?? string.Empty;
        Console.Write("Amount: ");
        decimal.TryParse(Console.ReadLine(), out decimal amount);
        _bank.Transfer(from, to, amount);
        Console.WriteLine("Transfer successful.");
    }

    private void ViewAccount()
    {
        Console.Write("Account number: ");
        string acc = Console.ReadLine() ?? string.Empty;
        var account = _bank.GetAccount(acc);
        if (account is null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        Console.WriteLine($"Account: {account.AccountNumber}");
        Console.WriteLine($"Customer: {account.Customer.FullName}");
        Console.WriteLine($"Email: {account.Customer.Email}");
        Console.WriteLine($"Phone: {account.Customer.PhoneNumber}");
        Console.WriteLine($"Type: {account.AccountType}");
        Console.WriteLine($"Balance: {account.Balance:C}");
        Console.WriteLine($"Status: {(account.IsActive ? "Active" : "Inactive")}");
        Console.WriteLine($"Created: {account.CreatedAt:d}");
    }

    private void ViewHistory()
    {
        Console.Write("Account number: ");
        string acc = Console.ReadLine() ?? string.Empty;
        var history = _bank.GetTransactionHistory(acc).ToList();

        if (history.Count == 0)
        {
            Console.WriteLine("No transactions yet.");
            return;
        }

        foreach (var t in history)
        {
            Console.WriteLine($"{t.TransactionDate:g} | {t.TransactionType} | {t.Amount:C} | {t.Description} | Balance: {t.BalanceAfterTransaction:C}");
        }
    }

    private void ViewAll()
    {
        var accounts = _bank.GetAllAccounts();
        if (accounts.Count == 0)
        {
            Console.WriteLine("No accounts created.");
            return;
        }

        foreach (var a in accounts)
        {
            Console.WriteLine($"{a.AccountNumber} | {a.Customer.FullName} | {a.AccountType} | {a.Balance:C} | {(a.IsActive ? "Active" : "Inactive")}");
        }
    }
}
