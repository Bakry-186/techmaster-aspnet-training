namespace BankAccountSystem.Models;

public class Customer
{
    public string CustomerId { get; set; } = Guid.NewGuid().ToString("N")[..8].ToUpper();
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
