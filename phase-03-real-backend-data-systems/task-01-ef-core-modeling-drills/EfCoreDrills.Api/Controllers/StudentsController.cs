using EfCoreDrills.Api.DTOs;
using EfCoreDrills.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreDrills.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(StudentService studentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStudents(
        int pageNumber = 1,
        int pageSize = 10,
        bool includeDeleted = false)
    {
        if (pageNumber <= 0)
        {
            return BadRequest(new { message = "pageNumber must be greater than 0." });
        }

        if (pageSize is < 1 or > 50)
        {
            return BadRequest(new { message = "pageSize must be between 1 and 50." });
        }

        var result = await studentService.GetPagedAsync(pageNumber, pageSize, includeDeleted);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var student = await studentService.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound(new { message = $"Student {id} was not found." });
        }

        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(CreateStudentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { message = "FullName and Email are required." });
        }

        var student = await studentService.CreateAsync(request);
        return CreatedAtAction(nameof(GetStudent), new { id = student!.Id }, student);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStudent(int id, UpdateStudentRequest request)
    {
        var student = await studentService.UpdateAsync(id, request);
        if (student is null)
        {
            return NotFound(new { message = $"Student {id} was not found." });
        }

        return Ok(student);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var deleted = await studentService.SoftDeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new { message = $"Student {id} was not found." });
        }

        return NoContent();
    }
}
