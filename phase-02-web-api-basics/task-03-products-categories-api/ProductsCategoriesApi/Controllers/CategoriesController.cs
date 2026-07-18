using Microsoft.AspNetCore.Mvc;
using ProductsCategoriesApi.Interfaces;
using ProductsCategoriesApi.Models.DTOs;

namespace ProductsCategoriesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await categoryService.GetAllCategories();
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var category = await categoryService.CreateCategory(dto);
        if (category == null)
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Category name is required and must be unique."));
        }

        return CreatedAtAction(nameof(GetAll), new { id = category.Id }, category);
    }

    private static ApiErrorResponse Error(string message, string code, params string[] details) =>
        new() { Message = message, Code = code, Details = details };
}
