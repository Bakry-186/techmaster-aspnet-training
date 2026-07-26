using EfCoreDrills.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreDrills.Api.Controllers;

[ApiController]
[Route("api/instructors")]
public class InstructorsController(InstructorService instructorService) : ControllerBase
{
    [HttpGet("{id:int}/tracks")]
    public async Task<IActionResult> GetInstructorTracks(int id)
    {
        var tracks = await instructorService.GetTracksAsync(id);
        return Ok(tracks);
    }
}
