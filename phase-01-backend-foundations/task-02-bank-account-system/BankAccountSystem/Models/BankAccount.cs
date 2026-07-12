namespace BankAccountSystem.Models;

public class BankAccount
{
    public string AccountNumber { get; set; } = string.Empty;
    public Customer Customer { get; set; } = new();
    public decimal Balance { get; private set; }
    public AccountType AccountType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public List<Transaction> Transactions { get; } = new();

    public void Deposit(decimal amount, string description = "Deposit")
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");

        Balance += amount;
        AddTransaction(TransactionType.Deposit, amount, description);
    }

    public void Withdraw(decimal amount, string description = "Withdrawal")
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient balance.");

        Balance -= amount;
        AddTransaction(TransactionType.Withdraw, amount, description);
    }

    public void ReceiveTransfer(decimal amount, string description)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");

        Balance += amount;
        AddTransaction(TransactionType.TransferIn, amount, description);
    }

    public void SendTransfer(decimal amount, string description)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient balance.");

        Balance -= amount;
        AddTransaction(TransactionType.TransferOut, amount, description);
    }

    private void AddTransaction(TransactionType type, decimal amount, string description)
    {
        Transactions.Add(new Transaction
        {
            AccountNumber = AccountNumber,
            TransactionType = type,
            Amount = amount,
            Description = description,
            BalanceAfterTransaction = Balance
        });
    }
}
