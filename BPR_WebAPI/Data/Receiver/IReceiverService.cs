using BPR_RazorLib.Models;

namespace BPR_WebAPI.Data.Receiver
{
    public interface IReceiverService
    {
        Task<WebResponse> AssignReceiverAsync(string serialNumber, string userName);
        Task<List<BPR_RazorLibrary.Models.Receiver>> GetAllReceiversAsync();
    }
}
