using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Api.Common;
using TrainingCenter.Api.Constants;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Services;

namespace TrainingCenter.Api.Controllers;

[Authorize(Roles = AppRoles.Instructor)]
[ApiController]
[Route("api/instructor")]
public class InstructorPortalController(InstructorPortalService portalService) : ControllerBase
{
    [HttpGet("my-tracks")]
    public async Task<IActionResult> GetMyTracks()
    {
        var data = await portalService.GetMyTracksAsync();
        return Ok(ApiResponse<IReadOnlyList<TrackListItemResponse>>.Ok(data));
    }

    [HttpGet("tracks/{trackId:int}/students")]
    public async Task<IActionResult> GetTrackStudents(int trackId)
    {
        var (data, error) = await portalService.GetTrackStudentsAsync(trackId);
        return error switch
        {
            "Track not found." => NotFound(ApiResponse<object>.Fail(error)),
            not null => Forbid(),
            _ => Ok(ApiResponse<IReadOnlyList<TrackStudentResponse>>.Ok(data!))
        };
    }

    [HttpGet("tracks/{trackId:int}/sessions")]
    public async Task<IActionResult> GetTrackSessions(int trackId)
    {
        var data = await portalService.GetTrackSessionsAsync(trackId);
        return Ok(ApiResponse<IReadOnlyList<TrackSessionResponse>>.Ok(data));
    }

    [HttpPost("tracks/{trackId:int}/sessions")]
    public async Task<IActionResult> CreateSession(int trackId, CreateTrackSessionRequest request)
    {
        var (data, error) = await portalService.CreateSessionAsync(trackId, request);
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : CreatedAtAction(nameof(GetTrackSessions), new { trackId },
                ApiResponse<TrackSessionResponse>.Ok(data!, "Session created successfully."));
    }

    [HttpPut("sessions/{sessionId:int}")]
    public async Task<IActionResult> UpdateSession(int sessionId, UpdateTrackSessionRequest request)
    {
        var (data, error) = await portalService.UpdateSessionAsync(sessionId, request);
        if (error == "Session not found.") return NotFound(ApiResponse<object>.Fail(error));
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<TrackSessionResponse>.Ok(data!, "Session updated successfully."));
    }

    [HttpGet("tracks/{trackId:int}/progress")]
    public async Task<IActionResult> GetTrackProgress(int trackId)
    {
        var (data, error) = await portalService.GetTrackProgressAsync(trackId);
        if (error == "Track not found.") return NotFound(ApiResponse<object>.Fail(error));
        return error is not null
            ? Forbid()
            : Ok(ApiResponse<TrackProgressResponse>.Ok(data!));
    }
}
