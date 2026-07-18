using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Interfaces;
using BookStoreApi.Models.DTOs;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController(IBookStoreService bookStoreService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await bookStoreService.GetAuthors());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAuthorDto dto)
    {
        var author = await bookStoreService.CreateAuthor(dto);
        if (author == null)
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Author name is required."));
        }

        return Created(string.Empty, author);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await bookStoreService.DeleteAuthor(id))
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Author not found or has related books."));
        }

        return NoContent();
    }

    private static ApiErrorResponse Error(string message, string code, params string[] details) =>
        new() { Message = message, Code = code, Details = details };
}

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(IBookStoreService bookStoreService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await bookStoreService.GetCategories());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var category = await bookStoreService.CreateCategory(dto);
        if (category == null)
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Category name is required and must be unique."));
        }

        return Created(string.Empty, category);
    }

    private static ApiErrorResponse Error(string message, string code, params string[] details) =>
        new() { Message = message, Code = code, Details = details };
}

[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookStoreService bookStoreService) : ControllerBase
{
    [HttpGet("reports/summary")]
    public async Task<IActionResult> GetSummaryReport()
    {
        return Ok(await bookStoreService.GetSummaryReport());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await bookStoreService.GetBookById(id);
        if (book == null)
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Book with id {id} was not found."));
        }

        return Ok(book);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? search, int? authorId, int? categoryId, int pageNumber = 1, int pageSize = 10)
    {
        var books = await bookStoreService.GetBooks(search, authorId, categoryId, pageNumber, pageSize);
        return Ok(books);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookDto dto)
    {
        var book = await bookStoreService.CreateBook(dto);
        if (book == null)
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Invalid book data, duplicate ISBN, or missing author/category."));
        }

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateBookDto dto)
    {
        var book = await bookStoreService.UpdateBook(id, dto);
        if (book == null)
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Book with id {id} was not found or data is invalid."));
        }

        return Ok(book);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await bookStoreService.DeleteBook(id))
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Book with id {id} was not found."));
        }

        return NoContent();
    }

    private static ApiErrorResponse Error(string message, string code, params string[] details) =>
        new() { Message = message, Code = code, Details = details };
}
