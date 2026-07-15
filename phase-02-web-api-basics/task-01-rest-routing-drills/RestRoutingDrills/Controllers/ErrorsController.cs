using Microsoft.AspNetCore.Mvc;
using RestRoutingDrills.Models;

namespace RestRoutingDrills.Controllers;

[ApiController]
[Route("api/errors")]
public class ErrorsController : ControllerBase
{
    [HttpGet("demo")]
    public IActionResult GetBadRequestDemo()
    {
        var error = new ApiErrorResponse
        {
            Message = "Invalid request",
            Code = "BAD_REQUEST",
            Details = ["Name is required", "Score must be between 0 and 100"]
        };

        return BadRequest(error);
    }

    [HttpGet("not-found")]
    public IActionResult GetNotFoundDemo()
    {
        var error = new ApiErrorResponse
        {
            Message = "Resource not found",
            Code = "NOT_FOUND",
            Details = ["Note with id 99 was not found"]
        };

        return NotFound(error);
    }
}
