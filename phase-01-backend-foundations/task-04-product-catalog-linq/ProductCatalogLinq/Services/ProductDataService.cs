using ProductCatalogLinq.Models;

namespace ProductCatalogLinq.Services;

public class ProductDataService
{
    public List<Product> GetProducts() =>
    [
        new() { ProductId = 1, Name = "Laptop Pro 14", Category = "Electronics", Price = 45000, StockQuantity = 5, CreatedAt = new DateTime(2026,1,10), IsAvailable = true, SupplierName = "TechSupplier" },
        new() { ProductId = 2, Name = "Wireless Mouse", Category = "Electronics", Price = 650, StockQuantity = 50, CreatedAt = new DateTime(2026,2,1), IsAvailable = true, SupplierName = "TechSupplier" },
        new() { ProductId = 3, Name = "Office Chair", Category = "Furniture", Price = 3500, StockQuantity = 10, CreatedAt = new DateTime(2025,12,15), IsAvailable = true, SupplierName = "HomeSupplier" },
        new() { ProductId = 4, Name = "Standing Desk", Category = "Furniture", Price = 8000, StockQuantity = 3, CreatedAt = new DateTime(2026,3,5), IsAvailable = true, SupplierName = "HomeSupplier" },
        new() { ProductId = 5, Name = "Notebook Pack", Category = "Stationery", Price = 120, StockQuantity = 100, CreatedAt = new DateTime(2026,1,20), IsAvailable = true, SupplierName = "PaperSupplier" },
        new() { ProductId = 6, Name = "Pen Set", Category = "Stationery", Price = 75, StockQuantity = 200, CreatedAt = new DateTime(2026,1,25), IsAvailable = true, SupplierName = "PaperSupplier" },
        new() { ProductId = 7, Name = "Gaming Keyboard", Category = "Electronics", Price = 2500, StockQuantity = 7, CreatedAt = new DateTime(2026,2,12), IsAvailable = true, SupplierName = "TechSupplier" },
        new() { ProductId = 8, Name = "Monitor 27 inch", Category = "Electronics", Price = 9000, StockQuantity = 4, CreatedAt = new DateTime(2026,2,20), IsAvailable = true, SupplierName = "TechSupplier" },
        new() { ProductId = 9, Name = "Desk Lamp", Category = "Furniture", Price = 650, StockQuantity = 0, CreatedAt = new DateTime(2025,11,1), IsAvailable = false, SupplierName = "HomeSupplier" },
        new() { ProductId = 10, Name = "Backpack", Category = "Accessories", Price = 1200, StockQuantity = 15, CreatedAt = new DateTime(2026,3,10), IsAvailable = true, SupplierName = "BagSupplier" },
        new() { ProductId = 11, Name = "USB-C Hub", Category = "Electronics", Price = 1250, StockQuantity = 12, CreatedAt = new DateTime(2026,4,1), IsAvailable = true, SupplierName = "TechSupplier" },
        new() { ProductId = 12, Name = "Whiteboard Markers", Category = "Stationery", Price = 95, StockQuantity = 80, CreatedAt = new DateTime(2026,2,15), IsAvailable = true, SupplierName = "PaperSupplier" },
        new() { ProductId = 13, Name = "Ergonomic Mouse Pad", Category = "Accessories", Price = 350, StockQuantity = 25, CreatedAt = new DateTime(2026,5,1), IsAvailable = true, SupplierName = "BagSupplier" },
        new() { ProductId = 14, Name = "Meeting Table", Category = "Furniture", Price = 12500, StockQuantity = 2, CreatedAt = new DateTime(2025,10,20), IsAvailable = true, SupplierName = "HomeSupplier" },
        new() { ProductId = 15, Name = "HD Webcam", Category = "Electronics", Price = 1800, StockQuantity = 6, CreatedAt = new DateTime(2026,4,17), IsAvailable = true, SupplierName = "TechSupplier" },
        new() { ProductId = 16, Name = "Printer Paper Box", Category = "Stationery", Price = 450, StockQuantity = 30, CreatedAt = new DateTime(2026,2,28), IsAvailable = true, SupplierName = "PaperSupplier" },
        new() { ProductId = 17, Name = "Laptop Stand", Category = "Accessories", Price = 950, StockQuantity = 9, CreatedAt = new DateTime(2026,3,30), IsAvailable = true, SupplierName = "BagSupplier" },
        new() { ProductId = 18, Name = "Network Cable 5m", Category = "Electronics", Price = 150, StockQuantity = 60, CreatedAt = new DateTime(2026,1,5), IsAvailable = true, SupplierName = "TechSupplier" },
        new() { ProductId = 19, Name = "Storage Cabinet", Category = "Furniture", Price = 6000, StockQuantity = 1, CreatedAt = new DateTime(2025,9,10), IsAvailable = true, SupplierName = "HomeSupplier" },
        new() { ProductId = 20, Name = "Sticky Notes", Category = "Stationery", Price = 60, StockQuantity = 0, CreatedAt = new DateTime(2026,5,10), IsAvailable = false, SupplierName = "PaperSupplier" },
        new() { ProductId = 21, Name = "Noise Cancelling Headset", Category = "Electronics", Price = 5200, StockQuantity = 4, CreatedAt = new DateTime(2026,3,22), IsAvailable = true, SupplierName = "TechSupplier" },
        new() { ProductId = 22, Name = "Desk Organizer", Category = "Accessories", Price = 300, StockQuantity = 40, CreatedAt = new DateTime(2026,6,1), IsAvailable = true, SupplierName = "BagSupplier" },
        new() { ProductId = 23, Name = "Projector", Category = "Electronics", Price = 22000, StockQuantity = 2, CreatedAt = new DateTime(2026,4,28), IsAvailable = true, SupplierName = "TechSupplier" },
        new() { ProductId = 24, Name = "Office Sofa", Category = "Furniture", Price = 15500, StockQuantity = 1, CreatedAt = new DateTime(2025,8,18), IsAvailable = true, SupplierName = "HomeSupplier" },
        new() { ProductId = 25, Name = "Calculator", Category = "Stationery", Price = 250, StockQuantity = 35, CreatedAt = new DateTime(2026,1,12), IsAvailable = true, SupplierName = "PaperSupplier" }
    ];
}
