namespace ProductCatalogLinq.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAvailable { get; set; }
    public string SupplierName { get; set; } = string.Empty;
}

public class ProductSummary
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StockStatus { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class SupplierReport
{
    public string SupplierName { get; set; } = string.Empty;
    public int ProductCount { get; set; }
    public decimal StockValue { get; set; }
    public decimal AveragePrice { get; set; }
}

public class CategoryStats
{
    public string Category { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal MinPrice { get; set; }
    public decimal TotalStockValue { get; set; }
}
