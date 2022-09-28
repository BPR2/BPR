using BPR_RazorLib.Models;

namespace BPR_WebAPI.Persistence.Receiver;

public interface IReceiverRepo
{
    Task<WebResponse> AssignReceiverAsync(string serialNumber, string userName);
    Task<List<BPR_RazorLibrary.Models.Receiver>> GetAllReceivers();
}
