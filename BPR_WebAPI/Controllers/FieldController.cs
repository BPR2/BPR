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
    public async Task<ActionResult<WebContent>> UpdateFieldTest([FromQuery] int FieldId, [FromQuery] string FieldName, [FromQuery] string FieldDescription, [FromQuery] int FieldPawLevel, [FromQuery] string SerialNumber, [FromQuery] string unassignReceiver)
    {
        var result = await fieldService.UpdateField(FieldId, FieldName, FieldDescription, FieldPawLevel, SerialNumber, unassignReceiver);
        return Ok(result);
    }

    [HttpPut("unassignReceiver")]
    public async Task<ActionResult<WebResponse>> UnassignReceiver([FromQuery] string receiverSerialNumber)
    {
        var result = await fieldService.UnassignReceiver(receiverSerialNumber);
        return Ok(result);
    }

    [HttpPut("removeField")]
    public async Task<ActionResult<WebResponse>> removeField([FromQuery] int fieldId)
    {
        var result = await fieldService.RemoveFieldFromUser(fieldId);
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

    [HttpGet("getLatestFieldByUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WebContent>> GetLatestFieldByUser([FromQuery] string fieldName, [FromQuery] string description, [FromQuery] int pawLevelLimit)
    {
        var result = await fieldService.GetLatestFieldByUser(fieldName, description, pawLevelLimit);
        return Ok(result);
    }
}
