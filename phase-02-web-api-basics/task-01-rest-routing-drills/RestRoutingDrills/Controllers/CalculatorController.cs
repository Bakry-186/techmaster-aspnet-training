using Microsoft.AspNetCore.Mvc;

namespace RestRoutingDrills.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    [HttpGet("add")]
    public IActionResult Add([FromQuery] double a, [FromQuery] double b)
    {
        return Ok(new { operation = "add", a = a, b = b, result = a + b });
    }
}
