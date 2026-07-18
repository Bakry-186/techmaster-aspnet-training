using RefactoredApi.Models;
using RefactoredApi.Models.DTOs;

namespace RefactoredApi.Interfaces;

public interface IProductService
{
    Task<IReadOnlyList<ProductResponseDto>> GetAll(int pageNumber, int pageSize);
    Task<ProductResponseDto?> GetById(int id);
    Task<ProductResponseDto?> Create(CreateProductDto dto);
    Task<ProductResponseDto?> Update(int id, UpdateProductDto dto);
    Task<bool> Delete(int id);
}
