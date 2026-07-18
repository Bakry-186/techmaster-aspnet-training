using BookStoreApi.Models;
using BookStoreApi.Models.DTOs;

namespace BookStoreApi.Interfaces;

public interface IBookStoreService
{
    Task<IReadOnlyList<Author>> GetAuthors();
    Task<Author?> CreateAuthor(CreateAuthorDto dto);
    Task<bool> DeleteAuthor(int id);

    Task<IReadOnlyList<Category>> GetCategories();
    Task<Category?> CreateCategory(CreateCategoryDto dto);

    Task<PagedBooksResponse> GetBooks(string? search, int? authorId, int? categoryId, int pageNumber, int pageSize);
    Task<BookResponseDto?> GetBookById(int id);
    Task<BookResponseDto?> CreateBook(CreateBookDto dto);
    Task<BookResponseDto?> UpdateBook(int id, UpdateBookDto dto);
    Task<bool> DeleteBook(int id);
    Task<BookSummaryReportDto> GetSummaryReport();
}
