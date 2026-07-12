using ProductCatalogLinq.Models;

namespace ProductCatalogLinq.Services;

public class ProductQueryService
{
    private readonly List<Product> _products;
    private readonly DateTime _today = new(2026, 7, 6);

    public ProductQueryService(List<Product> products)
    {
        _products = products;
    }

    public List<Product> GetAvailableProducts()
        => _products.Where(p => p.IsAvailable).ToList();

    public List<Product> FilterByCategory(string category)
        => _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

    public List<Product> FilterByPriceRange(decimal min, decimal max)
    {
        if (min < 0 || max < min)
            throw new ArgumentException("Invalid price range.");
        return _products.Where(p => p.Price >= min && p.Price <= max).ToList();
    }

    public List<Product> SearchByName(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            throw new ArgumentException("Keyword cannot be empty.");
        return _products.Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Product> SortByPriceAscending() => _products.OrderBy(p => p.Price).ToList();
    public List<Product> SortByPriceDescending() => _products.OrderByDescending(p => p.Price).ToList();

    public void PrintGroupedByCategory()
    {
        var groups = _products.GroupBy(p => p.Category);
        foreach (var g in groups)
        {
            Console.WriteLine($"\n[{g.Key}]");
            foreach (var p in g)
                Console.WriteLine($"  - {p.Name} ({p.Price:C})");
        }
    }

    public void PrintCountPerCategory()
    {
        foreach (var g in _products.GroupBy(p => p.Category))
            Console.WriteLine($"{g.Key}: {g.Count()}");
    }

    public decimal GetTotalStockValue()
        => _products.Sum(p => p.Price * p.StockQuantity);

    public void PrintStockValuePerCategory()
    {
        foreach (var g in _products.GroupBy(p => p.Category))
            Console.WriteLine($"{g.Key}: {g.Sum(p => p.Price * p.StockQuantity):C}");
    }

    public List<Product> GetTop5MostExpensive()
        => _products.OrderByDescending(p => p.Price).Take(5).ToList();

    public List<Product> GetLowStockProducts()
        => _products.Where(p => p.StockQuantity <= 5).ToList();

    public List<Product> GetOutOfStockProducts()
        => _products.Where(p => p.StockQuantity == 0 || !p.IsAvailable).ToList();

    public List<ProductSummary> GetProductSummaries()
        => _products.Select(p => new ProductSummary
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Price = p.Price,
            StockStatus = p.StockQuantity == 0 ? "Out of stock" : p.StockQuantity <= 5 ? "Low stock" : "In stock"
        }).ToList();

    public List<SupplierReport> GetSupplierReports()
        => _products.GroupBy(p => p.SupplierName)
            .Select(g => new SupplierReport
            {
                SupplierName = g.Key,
                ProductCount = g.Count(),
                StockValue = g.Sum(x => x.Price * x.StockQuantity),
                AveragePrice = g.Average(x => x.Price)
            }).ToList();

    public List<Product> GetRecentlyAddedProducts()
        => _products.Where(p => p.CreatedAt >= _today.AddDays(-60)).ToList();

    public List<CategoryStats> GetCategoryStatistics()
        => _products.GroupBy(p => p.Category)
            .Select(g => new CategoryStats
            {
                Category = g.Key,
                Count = g.Count(),
                AveragePrice = g.Average(x => x.Price),
                MaxPrice = g.Max(x => x.Price),
                MinPrice = g.Min(x => x.Price),
                TotalStockValue = g.Sum(x => x.Price * x.StockQuantity)
            }).ToList();

    public List<Product> GetProductsAboveAveragePrice()
    {
        var avg = _products.Average(p => p.Price);
        return _products.Where(p => p.Price > avg).ToList();
    }

    public List<Product> CombinedSearch(string? category, decimal? min, decimal? max, bool? available)
    {
        IEnumerable<Product> query = _products;
        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        if (min.HasValue)
            query = query.Where(p => p.Price >= min.Value);
        if (max.HasValue)
            query = query.Where(p => p.Price <= max.Value);
        if (available.HasValue)
            query = query.Where(p => p.IsAvailable == available.Value);
        return query.ToList();
    }

    public List<Product> GetPage(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            throw new ArgumentException("Page number and size must be positive.");
        return _products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public static void PrintProducts(IEnumerable<Product> products)
    {
        var list = products.ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("No products found.");
            return;
        }

        foreach (var p in list)
            Console.WriteLine($"{p.ProductId} | {p.Name} | {p.Category} | {p.Price:C} | Stock: {p.StockQuantity}");
    }
}
