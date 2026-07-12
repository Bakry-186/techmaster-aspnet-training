namespace OrderRefactor.Models;

public class Order
{
    public Customer Customer { get; set; } = new();
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
