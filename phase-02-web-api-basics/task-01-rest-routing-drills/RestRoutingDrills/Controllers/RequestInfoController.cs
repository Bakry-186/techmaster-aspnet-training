using Microsoft.AspNetCore.Mvc;

namespace RestRoutingDrills.Controllers;

[ApiController]
[Route("api/request-info")]
public class RequestInfoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromHeader(Name = "X-Client-Name")] string? clientName)
    {
        return Ok(new
        {
            method = Request.Method,
            path = Request.Path.Value,
            queryString = Request.QueryString.Value,
            clientName = clientName ?? "(not provided)"
        });
    }
}
