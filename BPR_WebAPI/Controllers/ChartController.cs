using BPR_RazorLibrary.Models;
using BPR_WebAPI.Services.Charts;
using Microsoft.AspNetCore.Mvc;

namespace BPR_WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ChartController : ControllerBase
{
    private IChartService chartService;

    public ChartController(IChartService chartService)
    {
        this.chartService = chartService;
    }

    [HttpGet("getChartDataByFieldId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WebContent>> GetChartDataByFieldId([FromQuery] int fieldId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await chartService.GetChartDataByFieldId(fieldId,startDate,endDate.AddDays(1));
        return Ok(result);
    }
}
