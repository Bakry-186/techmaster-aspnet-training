using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Api.Common;
using TrainingCenter.Api.DTOs;
using TrainingCenter.Api.Services;

namespace TrainingCenter.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorsController(InstructorService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetInstructors()
    {
        var data = await service.GetAllAsync();
        return Ok(ApiResponse<IReadOnlyList<InstructorResponse>>.Ok(data));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetInstructor(int id)
    {
        var data = await service.GetByIdAsync(id);
        return data is null
            ? NotFound(ApiResponse<object>.Fail("Instructor not found."))
            : Ok(ApiResponse<InstructorResponse>.Ok(data));
    }

    [HttpPost]
    public async Task<IActionResult> CreateInstructor(CreateInstructorRequest request)
    {
        var (data, error) = await service.CreateAsync(request);
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : CreatedAtAction(nameof(GetInstructor), new { id = data!.InstructorId },
                ApiResponse<InstructorResponse>.Ok(data!, "Instructor created successfully."));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateInstructor(int id, UpdateInstructorRequest request)
    {
        var (data, error) = await service.UpdateAsync(id, request);
        if (error == "Instructor not found.") return NotFound(ApiResponse<object>.Fail(error));
        return error is not null
            ? BadRequest(ApiResponse<object>.Fail(error))
            : Ok(ApiResponse<InstructorResponse>.Ok(data!, "Instructor updated successfully."));
    }

    [HttpGet("{id:int}/tracks")]
    public async Task<IActionResult> GetInstructorTracks(int id)
    {
        if (await service.GetByIdAsync(id) is null)
            return NotFound(ApiResponse<object>.Fail("Instructor not found."));
        var tracks = await service.GetTracksAsync(id);
        return Ok(ApiResponse<IReadOnlyList<TrackListItemResponse>>.Ok(tracks));
    }
}
