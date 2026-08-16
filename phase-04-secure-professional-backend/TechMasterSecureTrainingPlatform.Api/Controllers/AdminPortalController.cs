using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Api.Common;
using TrainingCenter.Api.Constants;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Services;

namespace TrainingCenter.Api.Controllers;

[Authorize(Roles = AppRoles.Admin)]
[ApiController]
[Route("api/admin")]
public class AdminActivityLogsController(AuditService auditService) : ControllerBase
{
    [HttpGet("activity-logs")]
    public async Task<IActionResult> GetActivityLogs(
        string? action, string? entityType, DateTime? from, DateTime? to,
        int pageNumber = 1, int pageSize = 20)
    {
        var (data, error) = await auditService.GetLogsAsync(action, entityType, from, to, pageNumber, pageSize);
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<IReadOnlyList<ActivityLogResponse>>.Ok(data));
    }
}

[Authorize(Roles = AppRoles.Admin)]
[ApiController]
[Route("api/admin/enrollments")]
public class AdminEnrollmentsController(AdminEnrollmentService adminEnrollmentService) : ControllerBase
{
    [HttpPut("{id:int}/approve")]
    public async Task<IActionResult> ApproveEnrollment(int id)
    {
        var (data, error) = await adminEnrollmentService.ApproveEnrollmentAsync(id);
        if (error == "Enrollment not found.") return NotFound(ApiResponse<object>.Fail(error));
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<EnrollmentDetailsResponse>.Ok(data!, "Enrollment approved."));
    }
}
