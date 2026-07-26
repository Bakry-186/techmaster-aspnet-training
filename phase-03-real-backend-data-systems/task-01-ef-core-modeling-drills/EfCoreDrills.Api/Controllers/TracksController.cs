using EfCoreDrills.Api.DTOs;
using EfCoreDrills.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreDrills.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController(TrackService trackService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTrack(int id)
    {
        var track = await trackService.GetByIdAsync(id);
        if (track is null)
        {
            return NotFound(new { message = $"Track {id} was not found." });
        }

        return Ok(track);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrack(CreateTrackRequest request)
    {
        var (track, error) = await trackService.CreateAsync(request);
        if (error is not null)
        {
            return BadRequest(new { message = error });
        }

        return CreatedAtAction(nameof(GetTrack), new { id = track!.Id }, track);
    }
}
