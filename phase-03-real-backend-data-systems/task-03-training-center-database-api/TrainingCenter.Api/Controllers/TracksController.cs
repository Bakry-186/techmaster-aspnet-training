using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Api.Common;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Services;

namespace TrainingCenter.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController(TrackService service, EnrollmentService enrollmentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTracks(
        string? keyword, string? level, string? status, int? instructorId)
    {
        var data = await service.GetAllAsync(keyword, level, status, instructorId);
        return Ok(ApiResponse<IReadOnlyList<TrackListItemResponse>>.Ok(data));
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
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<TrackDetailsResponse>.Ok(data!, "Track updated successfully."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTrack(int id)
    {
        var (success, error) = await service.SoftDeleteAsync(id);
        return success ? NoContent() : BadRequest(ApiResponse<object>.Fail(error!));
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
