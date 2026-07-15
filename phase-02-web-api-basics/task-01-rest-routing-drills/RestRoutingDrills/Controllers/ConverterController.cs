using RestRoutingDrills.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RestRoutingDrills.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConverterController(ITemperatureConverter temperatureConverter) : ControllerBase
{
    [HttpGet("celsius-to-fahrenheit")]
    public IActionResult ConvertToFahrenheit([FromQuery] double celsius)
    {
        return Ok(new { fahrenheit = temperatureConverter.ConvertToFahrenheit(celsius) });
    }
}
