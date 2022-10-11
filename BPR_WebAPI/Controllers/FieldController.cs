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
}
