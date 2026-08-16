using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Api.Common;
using TrainingCenter.Api.Constants;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Services;

namespace TrainingCenter.Api.Controllers;

[Authorize(Roles = AppRoles.Student)]
[ApiController]
[Route("api/student")]
public class StudentPortalController(StudentPortalService portalService) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var profile = await portalService.GetMyProfileAsync();
        return profile is null
            ? NotFound(ApiResponse<object>.Fail("Student profile not found."))
            : Ok(ApiResponse<StudentDetailsResponse>.Ok(profile));
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateStudentProfileRequest request)
    {
        var (data, error) = await portalService.UpdateProfileAsync(request);
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<StudentDetailsResponse>.Ok(data!, "Profile updated successfully."));
    }

    [HttpGet("my-enrollments")]
    public async Task<IActionResult> GetMyEnrollments()
    {
        var data = await portalService.GetMyEnrollmentsAsync();
        return Ok(ApiResponse<IReadOnlyList<EnrollmentListItemResponse>>.Ok(data));
    }

    [HttpGet("my-payments")]
    public async Task<IActionResult> GetMyPayments()
    {
        var data = await portalService.GetMyPaymentsAsync();
        return Ok(ApiResponse<IReadOnlyList<PaymentResponse>>.Ok(data));
    }

    [HttpGet("available-tracks")]
    public async Task<IActionResult> GetAvailableTracks()
    {
        var data = await portalService.GetAvailableTracksAsync();
        return Ok(ApiResponse<IReadOnlyList<TrackListItemResponse>>.Ok(data));
    }

    [HttpPost("enrollment-requests")]
    public async Task<IActionResult> RequestEnrollment(CreateEnrollmentRequestDto request)
    {
        var (data, error) = await portalService.RequestEnrollmentAsync(request);
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : CreatedAtAction(nameof(GetMyEnrollments), null,
                ApiResponse<EnrollmentDetailsResponse>.Ok(data!, "Enrollment request submitted."));
    }
}
