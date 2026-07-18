using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Interfaces;
using StudentManagementApi.Models.DTOs;

namespace StudentManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(IStudentService studentService) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<IActionResult> GetStudentStats()
    {
        var stats = await studentService.GetStudentStats();
        return Ok(stats);
    }

    [HttpGet("by-track/{trackName}")]
    public async Task<IActionResult> GetStudentsByTrack(string trackName, int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Page number and page size must be greater than 0."));
        }

        var students = await studentService.GetStudentsByTrackName(trackName, pageNumber, pageSize);
        return Ok(students);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await studentService.GetStudentById(id);
        if (student == null)
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Student with id {id} was not found."));
        }

        return Ok(student);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents(
        string? search,
        string? trackName,
        bool? isActive,
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Page number and page size must be greater than 0."));
        }

        var students = await studentService.GetAllStudents(search, trackName, isActive, pageNumber, pageSize);
        return Ok(students);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(CreateStudentDto createStudentDto)
    {
        var student = await studentService.CreateStudent(createStudentDto);
        if (student == null)
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Email must be unique."));
        }

        return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto updateStudentDto)
    {
        var existing = await studentService.GetStudentById(id);
        if (existing == null)
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Student with id {id} was not found."));
        }

        var student = await studentService.UpdateStudent(id, updateStudentDto);
        if (student == null)
        {
            return BadRequest(Error("Invalid request", "BAD_REQUEST", "Email must be unique."));
        }

        return Ok(student);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStudentStatus(int id, UpdateStudentStatus updateStudentStatus)
    {
        if (!await studentService.UpdateStudentActivation(id, updateStudentStatus))
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Student with id {id} was not found."));
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        if (!await studentService.DeleteStudent(id))
        {
            return NotFound(Error("Resource not found", "NOT_FOUND", $"Student with id {id} was not found."));
        }

        return NoContent();
    }

    private static ApiErrorResponse Error(string message, string code, params string[] details)
    {
        return new ApiErrorResponse
        {
            Message = message,
            Code = code,
            Details = details
        };
    }
}
