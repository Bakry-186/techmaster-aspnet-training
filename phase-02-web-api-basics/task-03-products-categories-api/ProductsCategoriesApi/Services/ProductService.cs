using ProductsCategoriesApi.Interfaces;
using ProductsCategoriesApi.Models;
using ProductsCategoriesApi.Models.DTOs;

namespace ProductsCategoriesApi.Services;

public class ProductService : IProductService
{
    private readonly CategoryService _categoryService;
    private readonly List<Product> _products = new();
    private int _nextId = 1;

    public ProductService(CategoryService categoryService)
    {
        _categoryService = categoryService;
        SeedProducts();
    }

    public Task<PagedProductsResponse> GetProducts(string? search, int? categoryId, decimal? minPrice, decimal? maxPrice, bool? isAvailable, bool? lowStock, int pageNumber, int pageSize)
    {
        var query = ApplyFilters(search, categoryId, minPrice, maxPrice, isAvailable, lowStock);
        return Task.FromResult(ToPagedResult(query, pageNumber, pageSize));
    }

    public Task<ProductResponseDto?> GetProductById(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product == null ? null : MapToResponse(product));
    }

    public Task<ProductResponseDto?> CreateProduct(CreateProductDto dto)
    {
        if (!IsValidProductInput(dto.Name, dto.Price, dto.Stock, dto.CategoryId, out _))
        {
            return Task.FromResult<ProductResponseDto?>(null);
        }

        var product = new Product
        {
            Id = _nextId++,
            Name = dto.Name.Trim(),
            Description = dto.Description.Trim(),
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId
        };

        _products.Add(product);
        return Task.FromResult<ProductResponseDto?>(MapToResponse(product));
    }

    public Task<ProductResponseDto?> UpdateProduct(int id, UpdateProductDto dto)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null || !IsValidProductInput(dto.Name, dto.Price, dto.Stock, dto.CategoryId, out _))
        {
            return Task.FromResult<ProductResponseDto?>(null);
        }

        product.Name = dto.Name.Trim();
        product.Description = dto.Description.Trim();
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.CategoryId = dto.CategoryId;
        return Task.FromResult<ProductResponseDto?>(MapToResponse(product));
    }

    public Task<bool> DeleteProduct(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return Task.FromResult(false);
        }

        _products.Remove(product);
        return Task.FromResult(true);
    }

    public Task<ProductResponseDto?> UpdateStock(int id, UpdateStockDto dto)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null || dto.Stock < 0)
        {
            return Task.FromResult<ProductResponseDto?>(null);
        }

        product.Stock = dto.Stock;
        return Task.FromResult<ProductResponseDto?>(MapToResponse(product));
    }

    public Task<IReadOnlyList<ProductResponseDto>> GetLowStockProducts(int threshold = 5)
    {
        var items = _products
            .Where(p => p.Stock <= threshold)
            .Select(MapToResponse)
            .ToList();

        return Task.FromResult<IReadOnlyList<ProductResponseDto>>(items);
    }

    public Task<StockValueReportDto> GetStockValueReport()
    {
        return Task.FromResult(new StockValueReportDto
        {
            TotalProducts = _products.Count,
            TotalStockUnits = _products.Sum(p => p.Stock),
            TotalStockValue = _products.Sum(p => p.Price * p.Stock)
        });
    }

    private IEnumerable<Product> ApplyFilters(string? search, int? categoryId, decimal? minPrice, decimal? maxPrice, bool? isAvailable, bool? lowStock)
    {
        var query = _products.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        if (isAvailable.HasValue)
        {
            query = query.Where(p => p.IsAvailable == isAvailable.Value);
        }

        if (lowStock == true)
        {
            query = query.Where(p => p.Stock <= 5);
        }

        return query;
    }

    private PagedProductsResponse ToPagedResult(IEnumerable<Product> query, int pageNumber, int pageSize)
    {
        var filtered = query.ToList();
        return new PagedProductsResponse
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = filtered.Count,
            TotalPages = filtered.Count == 0 ? 0 : (int)Math.Ceiling((double)filtered.Count / pageSize),
            Items = filtered.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(MapToResponse).ToList()
        };
    }

    private bool IsValidProductInput(string name, decimal price, int stock, int categoryId, out string? error)
    {
        error = null;
        if (string.IsNullOrWhiteSpace(name) || price <= 0 || stock < 0 || _categoryService.GetById(categoryId) == null)
        {
            error = "Invalid product data or category does not exist.";
            return false;
        }

        return true;
    }

    private ProductResponseDto MapToResponse(Product product)
    {
        var category = _categoryService.GetById(product.CategoryId);
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CategoryName = category?.Name ?? string.Empty,
            IsAvailable = product.IsAvailable
        };
    }

    private void SeedProducts()
    {
        var products = new (string name, string desc, decimal price, int stock, int categoryId)[]
        {
            ("Laptop", "Dev laptop", 25000m, 8, 1),
            ("Mouse", "Wireless mouse", 450m, 30, 1),
            ("Keyboard", "Mechanical keyboard", 1200m, 15, 1),
            ("Monitor", "27 inch monitor", 7000m, 6, 1),
            ("Desk", "Office desk", 3500m, 4, 2),
            ("Chair", "Ergonomic chair", 4200m, 5, 2),
            ("Bookshelf", "Wooden bookshelf", 1800m, 3, 2),
            ("Notebook", "A5 notebook", 35m, 100, 3),
            ("Pen Set", "Blue/black pens", 25m, 80, 3),
            ("Marker Pack", "Whiteboard markers", 60m, 40, 3),
            ("Stapler", "Office stapler", 90m, 20, 3),
            ("Backpack", "Laptop backpack", 900m, 12, 4),
            ("USB Hub", "4-port USB hub", 350m, 18, 4),
            ("Headphones", "Noise cancelling", 2200m, 7, 4),
            ("Webcam", "HD webcam", 1100m, 9, 1),
            ("Lamp", "Desk lamp", 250m, 2, 2)
        };

        foreach (var item in products)
        {
            _products.Add(new Product
            {
                Id = _nextId++,
                Name = item.name,
                Description = item.desc,
                Price = item.price,
                Stock = item.stock,
                CategoryId = item.categoryId
            });
        }
    }
}
