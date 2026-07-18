using Microsoft.AspNetCore.Mvc;
using RefactoredApi.Interfaces;
using RefactoredApi.Models.DTOs;

namespace RefactoredApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
    {
        var products = await productService.GetAll(pageNumber, pageSize);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await productService.GetById(id);
        if (product == null)
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Product with id {id} was not found."));
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var product = await productService.Create(dto);
        if (product == null)
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Name is required and price/stock must be valid."));
        }

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var product = await productService.Update(id, dto);
        if (product == null)
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Product with id {id} was not found or data is invalid."));
        }

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await productService.Delete(id))
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Product with id {id} was not found."));
        }

        return NoContent();
    }

    private static ApiErrorResponse Error(string message, string code, params string[] details) =>
        new() { Message = message, Code = code, Details = details };
}
