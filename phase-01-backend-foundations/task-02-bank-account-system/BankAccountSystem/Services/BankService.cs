using BankAccountSystem.Models;

namespace BankAccountSystem.Services;

public class BankService
{
    private readonly List<BankAccount> _accounts = new();

    public BankAccount CreateAccount(string fullName, string email, string phone, decimal initialBalance, AccountType type)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Customer name is required.");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required.");
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative.");

        var customer = new Customer
        {
            FullName = fullName.Trim(),
            Email = email.Trim(),
            PhoneNumber = phone.Trim()
        };

        string accountNumber = GenerateAccountNumber();
        var account = new BankAccount
        {
            AccountNumber = accountNumber,
            Customer = customer,
            AccountType = type
        };

        if (initialBalance > 0)
            account.Deposit(initialBalance, "Initial deposit");

        _accounts.Add(account);
        return account;
    }

    public BankAccount? GetAccount(string accountNumber)
        => _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

    public IReadOnlyList<BankAccount> GetAllAccounts() => _accounts;

    public void Deposit(string accountNumber, decimal amount)
    {
        var account = GetAccount(accountNumber) ?? throw new InvalidOperationException("Account not found.");
        account.Deposit(amount);
    }

    public void Withdraw(string accountNumber, decimal amount)
    {
        var account = GetAccount(accountNumber) ?? throw new InvalidOperationException("Account not found.");
        account.Withdraw(amount);
    }

    public void Transfer(string from, string to, decimal amount)
    {
        if (from == to)
            throw new InvalidOperationException("Source and destination cannot be the same.");

        var source = GetAccount(from) ?? throw new InvalidOperationException("Source account not found.");
        var dest = GetAccount(to) ?? throw new InvalidOperationException("Destination account not found.");

        source.SendTransfer(amount, $"Transfer to {to}");
        dest.ReceiveTransfer(amount, $"Transfer from {from}");
    }

    public IEnumerable<Transaction> GetTransactionHistory(string accountNumber)
    {
        var account = GetAccount(accountNumber) ?? throw new InvalidOperationException("Account not found.");
        return account.Transactions.OrderByDescending(t => t.TransactionDate);
    }

    private string GenerateAccountNumber()
    {
        string number;
        do
        {
            number = Random.Shared.Next(100000, 999999).ToString();
        } while (_accounts.Any(a => a.AccountNumber == number));

        return number;
    }
}
