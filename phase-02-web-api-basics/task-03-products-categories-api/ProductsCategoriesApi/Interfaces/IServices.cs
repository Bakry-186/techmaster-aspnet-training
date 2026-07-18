using ProductsCategoriesApi.Models;
using ProductsCategoriesApi.Models.DTOs;

namespace ProductsCategoriesApi.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<Category>> GetAllCategories();
    Task<Category?> CreateCategory(CreateCategoryDto dto);
}

public interface IProductService
{
    Task<PagedProductsResponse> GetProducts(string? search, int? categoryId, decimal? minPrice, decimal? maxPrice, bool? isAvailable, bool? lowStock, int pageNumber, int pageSize);
    Task<ProductResponseDto?> GetProductById(int id);
    Task<ProductResponseDto?> CreateProduct(CreateProductDto dto);
    Task<ProductResponseDto?> UpdateProduct(int id, UpdateProductDto dto);
    Task<bool> DeleteProduct(int id);
    Task<ProductResponseDto?> UpdateStock(int id, UpdateStockDto dto);
    Task<IReadOnlyList<ProductResponseDto>> GetLowStockProducts(int threshold = 5);
    Task<StockValueReportDto> GetStockValueReport();
}
