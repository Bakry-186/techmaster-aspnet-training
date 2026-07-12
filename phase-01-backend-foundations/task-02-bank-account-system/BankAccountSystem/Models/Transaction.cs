namespace BankAccountSystem.Models;

public class Transaction
{
    public string TransactionId { get; set; } = Guid.NewGuid().ToString("N")[..10].ToUpper();
    public string AccountNumber { get; set; } = string.Empty;
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.Now;
    public string Description { get; set; } = string.Empty;
    public decimal BalanceAfterTransaction { get; set; }
}
