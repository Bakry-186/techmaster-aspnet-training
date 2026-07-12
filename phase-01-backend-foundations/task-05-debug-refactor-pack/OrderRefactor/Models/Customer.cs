namespace OrderRefactor.Models;

public class Customer
{
    public string Name { get; set; } = string.Empty;
    public CustomerType Type { get; set; }
}
