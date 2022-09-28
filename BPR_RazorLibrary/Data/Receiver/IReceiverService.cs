
namespace BPR_RazorLibrary.Data.Receiver;

public interface IReceiverService
{
    Task<string> AssignReceiver(string serialNumber, string username);
    Task<List<Models.Receiver>> GetAllReceivers();
}
