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
public class TracksController(TrackService service, EnrollmentService enrollmentService, AuditService auditService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTracks(
        string? keyword, string? level, string? status, int? instructorId)
    {
        var (data, error) = await service.GetAllAsync(keyword, level, status, instructorId);
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<IReadOnlyList<TrackListItemResponse>>.Ok(data!));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTrack(int id)
    {
        var data = await service.GetByIdAsync(id);
        return data is null
            ? NotFound(ApiResponse<object>.Fail("Track not found."))
            : Ok(ApiResponse<TrackDetailsResponse>.Ok(data));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrack(CreateTrackRequest request)
    {
        var (data, error) = await service.CreateAsync(request);
        if (data is not null)
            await auditService.LogAsync("CreateTrack", "TrainingTrack", data.TrainingTrackId, $"Track created: {data.Code}");
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : CreatedAtAction(nameof(GetTrack), new { id = data!.TrainingTrackId },
                ApiResponse<TrackDetailsResponse>.Ok(data!, "Track created successfully."));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTrack(int id, UpdateTrackRequest request)
    {
        var (data, error) = await service.UpdateAsync(id, request);
        if (error == "Track not found.") return NotFound(ApiResponse<object>.Fail(error));
        if (data is not null)
            await auditService.LogAsync("UpdateTrack", "TrainingTrack", data.TrainingTrackId, $"Track updated: {data.Code}");
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<TrackDetailsResponse>.Ok(data!, "Track updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTrack(int id)
    {
        var (success, error) = await service.SoftDeleteAsync(id);
        if (success) return NoContent();
        return error switch
        {
            "Track not found." => NotFound(ApiResponse<object>.Fail(error)),
            "Cannot delete track with active enrollments." => Conflict(ApiResponse<object>.Fail(error)),
            _ => BadRequest(ApiResponse<object>.Fail(error!))
        };
    }

    [HttpGet("{id:int}/students")]
    public async Task<IActionResult> GetTrackStudents(int id)
    {
        if (await service.GetByIdAsync(id) is null)
            return NotFound(ApiResponse<object>.Fail("Track not found."));
        var students = await enrollmentService.GetStudentsByTrackAsync(id);
        return Ok(ApiResponse<IReadOnlyList<TrackStudentResponse>>.Ok(students));
    }
}
