using Microsoft.AspNetCore.Mvc;

namespace RestRoutingDrills.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    [HttpGet("calculate")]
    public IActionResult Calculate([FromQuery] int score)
    {
        if (score < 0 || score > 100)
        {
            return BadRequest(new { error = "Score must be between 0 and 100." });
        }

        return Ok(new { score, grade = GetGrade(score) });
    }

    private static string GetGrade(int score)
    {
        if (score >= 90) return "A";
        if (score >= 80) return "B";
        if (score >= 70) return "C";
        if (score >= 60) return "D";
        return "F";
    }
}
