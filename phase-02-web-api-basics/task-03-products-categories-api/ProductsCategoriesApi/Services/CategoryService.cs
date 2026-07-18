using ProductsCategoriesApi.Interfaces;
using ProductsCategoriesApi.Models;
using ProductsCategoriesApi.Models.DTOs;

namespace ProductsCategoriesApi.Services;

public class CategoryService : ICategoryService
{
    private readonly List<Category> _categories = new();
    private int _nextId = 1;

    public CategoryService()
    {
        _categories.AddRange(new[]
        {
            new Category { Id = _nextId++, Name = "Electronics", IsActive = true },
            new Category { Id = _nextId++, Name = "Furniture", IsActive = true },
            new Category { Id = _nextId++, Name = "Stationery", IsActive = true },
            new Category { Id = _nextId++, Name = "Accessories", IsActive = true }
        });
    }

    public Task<IReadOnlyList<Category>> GetAllCategories()
    {
        return Task.FromResult<IReadOnlyList<Category>>(_categories.AsReadOnly());
    }

    public Task<Category?> CreateCategory(CreateCategoryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return Task.FromResult<Category?>(null);
        }

        if (_categories.Any(c => c.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return Task.FromResult<Category?>(null);
        }

        var category = new Category { Id = _nextId++, Name = dto.Name.Trim(), IsActive = true };
        _categories.Add(category);
        return Task.FromResult<Category?>(category);
    }

    internal Category? GetById(int id) => _categories.FirstOrDefault(c => c.Id == id);
}
