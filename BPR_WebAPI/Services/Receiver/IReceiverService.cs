﻿using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Receiver;

public interface IReceiverService
{
    Task<WebResponse> AssignReceiverAsync(string serialNumber, string userName);
    Task<List<BPR_RazorLibrary.Models.Receiver>> GetAllReceiversAsync();
}