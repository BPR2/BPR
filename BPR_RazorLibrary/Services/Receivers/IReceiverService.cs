using BPR_RazorLibrary.Models;
namespace BPR_RazorLibrary.Services.Receivers;

public interface IReceiverService
{
    Task<string> AssignReceiver(string serialNumber, string username);
    Task<List<Receiver>> GetAllReceivers();
	Task<WebContent> GetReceiversByUserID(int userID);
	Task<WebResponse> AssignFieldToReceiver(int receiverID, int fieldID);
}
