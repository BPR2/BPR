using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Persistence.Receivers;

public interface IReceiverRepo
{
    Task<WebResponse> AssignReceiverAsync(string serialNumber, string userName);
    Task<List<Receiver>> GetAllReceivers();
}
