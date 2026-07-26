using EfCoreDrills.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EfCoreDrills.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController(EnrollmentService enrollmentService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEnrollment(int id)
    {
        var enrollment = await enrollmentService.GetByIdAsync(id);
        if (enrollment is null)
        {
            return NotFound(new { message = $"Enrollment {id} was not found." });
        }

        return Ok(enrollment);
    }
}
