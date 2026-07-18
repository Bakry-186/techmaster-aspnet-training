namespace BookStoreApi.Models.DTOs;

public class ApiErrorResponse
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string[] Details { get; set; } = [];
}

public class CreateAuthorDto
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
}

public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }
    public DateTime PublishedDate { get; set; }
}

public class UpdateBookDto : CreateBookDto;

public class BookResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
}

public class PagedBooksResponse
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public List<BookResponseDto> Items { get; set; } = [];
}

public class BookSummaryReportDto
{
    public int TotalBooks { get; set; }
    public int TotalAuthors { get; set; }
    public int TotalCategories { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public int OutOfStockBooks { get; set; }
}
