﻿using BPR_RazorLibrary.Models;
using BPR_WebAPI.Services.Users;
using BPR_WebAPI.Services.Receiver;
using Microsoft.AspNetCore.Mvc;

namespace BPR_WebAPI.Controllers
{
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
        public async Task<ActionResult<WebResponse>> AssignReceiver([FromQuery] string serialNumber, [FromQuery] string username)
        {
            var result = await receiverService.AssignReceiverAsync(serialNumber,username);
            return Ok(result);
        }

        [HttpGet("allReceivers")]
        public async Task<ActionResult<WebResponse>> GetAllReceivers()
        {
            var result = await receiverService.GetAllReceiversAsync();
            return Ok(result);
        }
    }
}