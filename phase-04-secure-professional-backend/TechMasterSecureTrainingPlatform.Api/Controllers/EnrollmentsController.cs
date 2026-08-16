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
public class EnrollmentsController(EnrollmentService service, AuditService auditService) : ControllerBase
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
        if (data is not null)
            await auditService.LogAsync("CreateEnrollment", "Enrollment", data.EnrollmentId,
                $"Enrollment created for student {data.StudentId} in track {data.TrainingTrackId}.");
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
        if (data is not null)
            await auditService.LogAsync("UpdateEnrollmentStatus", "Enrollment", data.EnrollmentId,
                $"Enrollment status changed to {data.Status}.");
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<EnrollmentDetailsResponse>.Ok(data!, "Enrollment status updated."));
    }
}
