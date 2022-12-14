using BPR_RazorLibrary.Models;
using BPR_WebAPI.Services.Receiver;
using Microsoft.AspNetCore.Mvc;

namespace BPR_WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReceiverController : ControllerBase
{
    private IReceiverService receiverService;

    public ReceiverController(IReceiverService receiverService)
    {
        this.receiverService = receiverService;
    }

    [HttpPost("assignReceiver")]
    public async Task<ActionResult<WebResponse>> AssignReceiver([FromQuery] string serialNumber, [FromQuery] string username, [FromQuery] int maxTransmission, [FromQuery] int leftTransmission)
    {
        var result = await receiverService.AssignReceiverAsync(serialNumber, username, maxTransmission, leftTransmission);
        return Ok(result);
    }

    [HttpGet("allReceivers")]
    public async Task<ActionResult<WebResponse>> GetAllReceivers()
    {
        var result = await receiverService.GetAllReceiversAsync();
        return Ok(result);
    }

    [HttpGet("allReceiversList")]
    public async Task<ActionResult<WebResponse>> GetAllReceiversList()
    {
        var result = await receiverService.GetAllReceiversList();
        return Ok(result);
    }

    [HttpGet("receiver")]
    public async Task<ActionResult<WebResponse>> GetReceiverByUserID([FromQuery] int userID)
    {
        var result = await receiverService.GetReceiversByUserID(userID);
        return Ok(result);
    }

    [HttpPut("assignField")]
    public async Task<ActionResult<WebResponse>> AssignFieldToReceiver([FromBody] Receiver receiver)
    {
        var result = await receiverService.AssignFieldToReceiver(receiver.ReceiverId, (int)receiver.FieldId);
        return Ok(result);
    }

    [HttpPut("updateTimeInterval")]
    public async Task<ActionResult<WebResponse>> UpdateTimeInterval([FromBody] int timeInterval, [FromQuery] string serialNumber)
    {
        var result = await receiverService.UpdateReceiverTimeInterval(timeInterval, serialNumber);
        return Ok(result);
    }

    [HttpGet("getReceiverBySerialNumber")]
    public async Task<ActionResult<WebResponse>> GetReceiverBySerialNumber([FromQuery] string serialNumber)
    {
        var result = await receiverService.GetReceiverBySerialNumber(serialNumber);
        return Ok(result);
    }

    [HttpPut("unassignReceiverFromUser")]
    public async Task<ActionResult<WebResponse>> UnAssignReceiverFromUser([FromQuery] string serialNumber)
    {
        var result = await receiverService.UnassignReceiverFromUser(serialNumber);
        return Ok(result);
    }

    [HttpPut("updateReceiverToUser")]
    public async Task<ActionResult<WebResponse>> UpdateReceiverToUser([FromQuery] string serialNumber, [FromQuery] string userName)
    {
        var result = await receiverService.UpdateReceiverToUser(serialNumber, userName);
        return Ok(result);
    }
}
