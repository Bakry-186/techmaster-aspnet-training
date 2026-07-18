using RefactoredApi.Interfaces;
using RefactoredApi.Models;
using RefactoredApi.Models.DTOs;

namespace RefactoredApi.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products = new();
    private int _nextId = 1;

    public ProductService()
    {
        _products.Add(new Product { Id = _nextId++, Name = "Sample Mouse", Description = "Wireless mouse", Price = 450m, Stock = 10 });
    }

    public Task<IReadOnlyList<ProductResponseDto>> GetAll(int pageNumber, int pageSize)
    {
        var items = _products.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(Map).ToList();
        return Task.FromResult<IReadOnlyList<ProductResponseDto>>(items);
    }

    public Task<ProductResponseDto?> GetById(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product == null ? null : Map(product));
    }

    public Task<ProductResponseDto?> Create(CreateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0 || dto.Stock < 0)
        {
            return Task.FromResult<ProductResponseDto?>(null);
        }

        var product = new Product
        {
            Id = _nextId++,
            Name = dto.Name.Trim(),
            Description = dto.Description.Trim(),
            Price = dto.Price,
            Stock = dto.Stock
        };

        _products.Add(product);
        return Task.FromResult<ProductResponseDto?>(Map(product));
    }

    public Task<ProductResponseDto?> Update(int id, UpdateProductDto dto)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null || string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0 || dto.Stock < 0)
        {
            return Task.FromResult<ProductResponseDto?>(null);
        }

        product.Name = dto.Name.Trim();
        product.Description = dto.Description.Trim();
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        return Task.FromResult<ProductResponseDto?>(Map(product));
    }

    public Task<bool> Delete(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return Task.FromResult(false);
        }

        _products.Remove(product);
        return Task.FromResult(true);
    }

    private static ProductResponseDto Map(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price,
        Stock = product.Stock
    };
}
