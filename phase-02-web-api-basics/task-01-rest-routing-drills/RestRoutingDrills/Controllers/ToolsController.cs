using Microsoft.AspNetCore.Mvc;

namespace RestRoutingDrills.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToolsController : ControllerBase
{
    [HttpGet("echo/{name}")]
    public IActionResult Echo(string name)
    {
        return Ok(new { message = $"Hello, {name}!" });
    }
}
