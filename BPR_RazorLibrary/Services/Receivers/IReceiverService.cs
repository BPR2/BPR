using BPR_RazorLibrary.Models;
namespace BPR_RazorLibrary.Services.Receivers;

public interface IReceiverService
{
    Task<string> AssignReceiver(string serialNumber, string username);
    Task<List<Receiver>> GetAllReceivers();
	Task<List<Receiver>> GetReceiversByUserID(int userID);
	Task<string> AssignFieldToReceiver(Receiver receiver);
}
