using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Api.Common;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Services;

namespace TrainingCenter.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController(EnrollmentService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEnrollments(
        string? status, int? trackId, int? studentId, string? paymentStatus)
    {
        var (data, error) = await service.GetAllAsync(status, trackId, studentId, paymentStatus);
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<IReadOnlyList<EnrollmentListItemResponse>>.Ok(data!));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEnrollment(int id)
    {
        var data = await service.GetByIdAsync(id);
        return data is null
            ? NotFound(ApiResponse<object>.Fail("Enrollment not found."))
            : Ok(ApiResponse<EnrollmentDetailsResponse>.Ok(data));
    }

    [HttpPost]
    public async Task<IActionResult> CreateEnrollment(CreateEnrollmentRequest request)
    {
        var (data, error) = await service.CreateAsync(request);
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : CreatedAtAction(nameof(GetEnrollment), new { id = data!.EnrollmentId },
                ApiResponse<EnrollmentDetailsResponse>.Ok(data!, "Enrollment created successfully."));
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateEnrollmentStatus(int id, UpdateEnrollmentStatusRequest request)
    {
        var (data, error) = await service.UpdateStatusAsync(id, request);
        if (error == "Enrollment not found.") return NotFound(ApiResponse<object>.Fail(error));
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<EnrollmentDetailsResponse>.Ok(data!, "Enrollment status updated."));
    }
}
