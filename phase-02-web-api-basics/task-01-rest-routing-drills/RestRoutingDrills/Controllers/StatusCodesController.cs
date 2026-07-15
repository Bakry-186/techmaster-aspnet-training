using Microsoft.AspNetCore.Mvc;

namespace RestRoutingDrills.Controllers;

[ApiController]
[Route("api/status-codes")]
public class StatusCodesController : ControllerBase
{
    [HttpGet("ok")]
    public IActionResult GetOk()
    {
        return Ok(new { statusCode = 200, message = "Request succeeded." });
    }

    [HttpPost("created")]
    public IActionResult PostCreated()
    {
        var resource = new { id = 1, name = "Sample resource" };
        return Created("/api/status-codes/created/1", resource);
    }

    [HttpDelete("no-content")]
    public IActionResult DeleteNoContent()
    {
        return NoContent();
    }

    [HttpGet("bad-request")]
    public IActionResult GetBadRequest()
    {
        return BadRequest(new { statusCode = 400, error = "Invalid request." });
    }

    [HttpGet("not-found")]
    public IActionResult GetNotFound()
    {
        return NotFound(new { statusCode = 404, error = "Resource not found." });
    }
}
