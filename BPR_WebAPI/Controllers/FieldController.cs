using BPR_WebAPI.Services.Field;
using Microsoft.AspNetCore.Mvc;
using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FieldController : ControllerBase
{
    private IFieldService fieldService;

	public FieldController(IFieldService fieldService)
	{
		this.fieldService = fieldService;
	}

	[HttpGet("getAllFieldsByUserId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WebContent>> GetAllFieldsByUserId([FromQuery] int userId)
    {
        var result = await fieldService.GetAllFieldsByUserId(userId);
        return Ok(result);
    }

    [HttpPut("updateField")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WebContent>> UpdateField(Field field)
    {
        var result = await fieldService.UpdateFieldAsync(field);
        return Ok(result);
    }

    [HttpPut("unassignReceiver")]
    public async Task<ActionResult<WebResponse>> UnassignReceiver([FromQuery] int fieldId, [FromQuery] int receiverId)
    {
        var result = await fieldService.UnassignReceiver(fieldId, receiverId);
        return Ok(result);
    }

	[HttpPost("createField")]
	public async Task<ActionResult<WebResponse>> CreateField([FromBody] Field field)
	{
		var result = await fieldService.CreateFieldAsync(field);
		return Ok(result);
	}

	[HttpGet("getLatestFieldByUserId")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<WebContent>> GetLatestFieldByUserId([FromQuery] int userId)
	{
		var result = await fieldService.GetLatestFieldByUserId(userId);
		return Ok(result);
	}
}
