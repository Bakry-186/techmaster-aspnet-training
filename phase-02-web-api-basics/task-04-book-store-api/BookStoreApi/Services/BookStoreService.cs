using BookStoreApi.Interfaces;
using BookStoreApi.Models;
using BookStoreApi.Models.DTOs;

namespace BookStoreApi.Services;

public class BookStoreService : IBookStoreService
{
    private readonly List<Author> _authors = new();
    private readonly List<Category> _categories = new();
    private readonly List<Book> _books = new();
    private int _nextAuthorId = 1;
    private int _nextCategoryId = 1;
    private int _nextBookId = 1;

    public BookStoreService()
    {
        SeedData();
    }

    public Task<IReadOnlyList<Author>> GetAuthors() =>
        Task.FromResult<IReadOnlyList<Author>>(_authors.AsReadOnly());

    public Task<Author?> CreateAuthor(CreateAuthorDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return Task.FromResult<Author?>(null);
        }

        var author = new Author { Id = _nextAuthorId++, Name = dto.Name.Trim(), Country = dto.Country.Trim() };
        _authors.Add(author);
        return Task.FromResult<Author?>(author);
    }

    public Task<bool> DeleteAuthor(int id)
    {
        if (_books.Any(b => b.AuthorId == id))
        {
            return Task.FromResult(false);
        }

        var author = _authors.FirstOrDefault(a => a.Id == id);
        if (author == null)
        {
            return Task.FromResult(false);
        }

        _authors.Remove(author);
        return Task.FromResult(true);
    }

    public Task<IReadOnlyList<Category>> GetCategories() =>
        Task.FromResult<IReadOnlyList<Category>>(_categories.AsReadOnly());

    public Task<Category?> CreateCategory(CreateCategoryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || _categories.Any(c => c.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return Task.FromResult<Category?>(null);
        }

        var category = new Category { Id = _nextCategoryId++, Name = dto.Name.Trim() };
        _categories.Add(category);
        return Task.FromResult<Category?>(category);
    }

    public Task<PagedBooksResponse> GetBooks(string? search, int? authorId, int? categoryId, int pageNumber, int pageSize)
    {
        var query = _books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b =>
                b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                b.Isbn.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (authorId.HasValue)
        {
            query = query.Where(b => b.AuthorId == authorId.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(b => b.CategoryId == categoryId.Value);
        }

        var filtered = query.ToList();
        return Task.FromResult(new PagedBooksResponse
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = filtered.Count,
            TotalPages = filtered.Count == 0 ? 0 : (int)Math.Ceiling((double)filtered.Count / pageSize),
            Items = filtered.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(MapBook).ToList()
        });
    }

    public Task<BookResponseDto?> GetBookById(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book == null ? null : MapBook(book));
    }

    public Task<BookResponseDto?> CreateBook(CreateBookDto dto)
    {
        if (!IsValidBook(dto.Title, dto.Isbn, dto.Price, dto.Stock, dto.AuthorId, dto.CategoryId, out _))
        {
            return Task.FromResult<BookResponseDto?>(null);
        }

        var book = new Book
        {
            Id = _nextBookId++,
            Title = dto.Title.Trim(),
            Isbn = dto.Isbn.Trim(),
            Price = dto.Price,
            Stock = dto.Stock,
            AuthorId = dto.AuthorId,
            CategoryId = dto.CategoryId,
            PublishedDate = dto.PublishedDate
        };

        _books.Add(book);
        return Task.FromResult<BookResponseDto?>(MapBook(book));
    }

    public Task<BookResponseDto?> UpdateBook(int id, UpdateBookDto dto)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return Task.FromResult<BookResponseDto?>(null);
        }

        if (_books.Any(b => b.Id != id && b.Isbn.Equals(dto.Isbn, StringComparison.OrdinalIgnoreCase)))
        {
            return Task.FromResult<BookResponseDto?>(null);
        }

        if (!IsValidBook(dto.Title, dto.Isbn, dto.Price, dto.Stock, dto.AuthorId, dto.CategoryId, out _))
        {
            return Task.FromResult<BookResponseDto?>(null);
        }

        book.Title = dto.Title.Trim();
        book.Isbn = dto.Isbn.Trim();
        book.Price = dto.Price;
        book.Stock = dto.Stock;
        book.AuthorId = dto.AuthorId;
        book.CategoryId = dto.CategoryId;
        book.PublishedDate = dto.PublishedDate;
        return Task.FromResult<BookResponseDto?>(MapBook(book));
    }

    public Task<bool> DeleteBook(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return Task.FromResult(false);
        }

        _books.Remove(book);
        return Task.FromResult(true);
    }

    public Task<BookSummaryReportDto> GetSummaryReport()
    {
        return Task.FromResult(new BookSummaryReportDto
        {
            TotalBooks = _books.Count,
            TotalAuthors = _authors.Count,
            TotalCategories = _categories.Count,
            TotalInventoryValue = _books.Sum(b => b.Price * b.Stock),
            OutOfStockBooks = _books.Count(b => b.Stock == 0)
        });
    }

    private bool IsValidBook(string title, string isbn, decimal price, int stock, int authorId, int categoryId, out string? error)
    {
        error = null;
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(isbn) || price <= 0 || stock < 0)
        {
            error = "Invalid book data.";
            return false;
        }

        if (_authors.All(a => a.Id != authorId) || _categories.All(c => c.Id != categoryId))
        {
            error = "Author or category does not exist.";
            return false;
        }

        if (_books.Any(b => b.Isbn.Equals(isbn, StringComparison.OrdinalIgnoreCase)))
        {
            error = "ISBN must be unique.";
            return false;
        }

        return true;
    }

    private BookResponseDto MapBook(Book book)
    {
        var author = _authors.First(a => a.Id == book.AuthorId);
        var category = _categories.First(c => c.Id == book.CategoryId);
        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            Price = book.Price,
            Stock = book.Stock,
            AuthorId = book.AuthorId,
            AuthorName = author.Name,
            CategoryId = book.CategoryId,
            CategoryName = category.Name,
            PublishedDate = book.PublishedDate
        };
    }

    private void SeedData()
    {
        _authors.AddRange(new[]
        {
            new Author { Id = _nextAuthorId++, Name = "Robert Martin", Country = "USA" },
            new Author { Id = _nextAuthorId++, Name = "Andrew Hunt", Country = "USA" },
            new Author { Id = _nextAuthorId++, Name = "Eric Evans", Country = "USA" }
        });

        _categories.AddRange(new[]
        {
            new Category { Id = _nextCategoryId++, Name = "Software Engineering" },
            new Category { Id = _nextCategoryId++, Name = "Architecture" },
            new Category { Id = _nextCategoryId++, Name = "Career" }
        });

        _books.AddRange(new[]
        {
            new Book { Id = _nextBookId++, Title = "Clean Code", Isbn = "978-0132350884", Price = 450m, Stock = 12, AuthorId = 1, CategoryId = 1, PublishedDate = new DateTime(2008, 8, 1) },
            new Book { Id = _nextBookId++, Title = "The Pragmatic Programmer", Isbn = "978-0135957059", Price = 420m, Stock = 8, AuthorId = 2, CategoryId = 1, PublishedDate = new DateTime(2019, 9, 13) },
            new Book { Id = _nextBookId++, Title = "Domain-Driven Design", Isbn = "978-0321125217", Price = 520m, Stock = 5, AuthorId = 3, CategoryId = 2, PublishedDate = new DateTime(2003, 8, 20) },
            new Book { Id = _nextBookId++, Title = "Clean Architecture", Isbn = "978-0134494166", Price = 480m, Stock = 0, AuthorId = 1, CategoryId = 2, PublishedDate = new DateTime(2017, 9, 20) },
            new Book { Id = _nextBookId++, Title = "Backend Career Guide", Isbn = "978-1000000001", Price = 300m, Stock = 15, AuthorId = 2, CategoryId = 3, PublishedDate = new DateTime(2024, 1, 10) }
        });
    }
}
