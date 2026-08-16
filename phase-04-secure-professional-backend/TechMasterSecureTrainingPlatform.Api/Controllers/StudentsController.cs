using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Api.Common;
using TrainingCenter.Api.Constants;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Services;

namespace TrainingCenter.Api.Controllers;

[Authorize(Roles = AppRoles.Admin)]
[ApiController]
[Route("api/[controller]")]
public class StudentsController(StudentService service, AuditService auditService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStudents(
        string? search, bool? isActive, bool includeDeleted = false,
        int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize is < 1 or > 50)
            return BadRequest(ApiResponse<object>.Fail("Invalid pagination parameters."));

        var result = await service.GetPagedAsync(search, isActive, includeDeleted, pageNumber, pageSize);
        return Ok(ApiResponse<PaginationResult<StudentListItemResponse>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var student = await service.GetByIdAsync(id);
        return student is null
            ? NotFound(ApiResponse<object>.Fail("Student not found."))
            : Ok(ApiResponse<StudentDetailsResponse>.Ok(student));
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(CreateStudentRequest request)
    {
        var (data, error) = await service.CreateAsync(request);
        if (data is not null)
            await auditService.LogAsync("CreateStudent", "Student", data.StudentId, $"Student created: {data.Email}");
        return error switch
        {
            "Email must be unique." => Conflict(ApiResponse<object>.Fail(error)),
            not null => BadRequest(ApiResponse<object>.Fail(error)),
            _ => CreatedAtAction(nameof(GetStudent), new { id = data!.StudentId },
                ApiResponse<StudentDetailsResponse>.Ok(data!, "Student created successfully."))
        };
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateStudent(int id, UpdateStudentRequest request)
    {
        var (data, error) = await service.UpdateAsync(id, request);
        if (error == "Student not found.") return NotFound(ApiResponse<object>.Fail(error));
        return error switch
        {
            "Email must be unique." => Conflict(ApiResponse<object>.Fail(error)),
            not null => BadRequest(ApiResponse<object>.Fail(error)),
            _ => Ok(ApiResponse<StudentDetailsResponse>.Ok(data!, "Student updated successfully."))
        };
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var (success, error) = await service.SoftDeleteAsync(id);
        return success ? NoContent() : NotFound(ApiResponse<object>.Fail(error!));
    }

    [HttpGet("{id:int}/enrollments")]
    public async Task<IActionResult> GetStudentEnrollments(int id, EnrollmentService enrollmentService)
    {
        if (await service.GetByIdAsync(id) is null)
            return NotFound(ApiResponse<object>.Fail("Student not found."));
        var enrollments = await enrollmentService.GetByStudentAsync(id);
        return Ok(ApiResponse<IReadOnlyList<EnrollmentListItemResponse>>.Ok(enrollments));
    }
}
