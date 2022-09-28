using BPR_RazorLibrary.Models;
using BPR_WebAPI.Services.Sensor;
using Microsoft.AspNetCore.Mvc;

namespace BPR_WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SensorController : ControllerBase
{
    private ISensorService sensorService;

    public SensorController(ISensorService sensorService)
    {
        this.sensorService = sensorService;
    }

    [HttpPost("addNewSensor")]
    public async Task<ActionResult<WebResponse>> AddNewSensor([FromQuery] string tagNumber, [FromQuery] string serialNumber)
    {
        var result = await sensorService.AddNewSensorAsync(tagNumber, serialNumber);
        return Ok(result);
    }
}
