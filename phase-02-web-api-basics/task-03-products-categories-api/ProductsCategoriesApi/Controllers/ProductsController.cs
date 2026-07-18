using Microsoft.AspNetCore.Mvc;
using ProductsCategoriesApi.Interfaces;
using ProductsCategoriesApi.Models.DTOs;

namespace ProductsCategoriesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        var products = await productService.GetLowStockProducts();
        return Ok(products);
    }

    [HttpGet("reports/stock-value")]
    public async Task<IActionResult> GetStockValueReport()
    {
        var report = await productService.GetStockValueReport();
        return Ok(report);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await productService.GetProductById(id);
        if (product == null)
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Product with id {id} was not found."));
        }

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        string? search,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        bool? isAvailable,
        bool? lowStock,
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Page number and page size must be greater than 0."));
        }

        var products = await productService.GetProducts(search, categoryId, minPrice, maxPrice, isAvailable, lowStock, pageNumber, pageSize);
        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var product = await productService.CreateProduct(dto);
        if (product == null)
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Invalid product data or category does not exist."));
        }

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var product = await productService.UpdateProduct(id, dto);
        if (product == null)
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Product with id {id} was not found or data is invalid."));
        }

        return Ok(product);
    }

    [HttpPatch("{id:int}/stock")]
    public async Task<IActionResult> UpdateStock(int id, UpdateStockDto dto)
    {
        var product = await productService.UpdateStock(id, dto);
        if (product == null)
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Product with id {id} was not found or stock is invalid."));
        }

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await productService.DeleteProduct(id))
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Product with id {id} was not found."));
        }

        return NoContent();
    }

    private static ApiErrorResponse Error(string message, string code, params string[] details) =>
        new() { Message = message, Code = code, Details = details };
}
