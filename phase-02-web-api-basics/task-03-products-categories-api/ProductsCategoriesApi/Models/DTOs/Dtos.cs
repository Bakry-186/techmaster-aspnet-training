namespace ProductsCategoriesApi.Models.DTOs;

public class ApiErrorResponse
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string[] Details { get; set; } = [];
}

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
}

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
}

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
}

public class UpdateStockDto
{
    public int Stock { get; set; }
}

public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}

public class PagedProductsResponse
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public List<ProductResponseDto> Items { get; set; } = [];
}

public class StockValueReportDto
{
    public int TotalProducts { get; set; }
    public int TotalStockUnits { get; set; }
    public decimal TotalStockValue { get; set; }
}
