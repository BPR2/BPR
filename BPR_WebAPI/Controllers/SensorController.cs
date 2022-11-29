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

    [HttpPut("updateSensor")]
    public async Task<ActionResult<WebResponse>> UpdateSensor([FromQuery] string tagNumber, [FromQuery] string serialNumber)
    {
        var result = await sensorService.UpdateSensorAsync(tagNumber, serialNumber);
        return Ok(result);
    }

    [HttpPut("unassignSensor")]
    public async Task<ActionResult<WebResponse>> UnassignSensor([FromQuery] string tagNumber)
    {
        var result = await sensorService.UnassignSensorAsync(tagNumber);
        return Ok(result);
    }
}
