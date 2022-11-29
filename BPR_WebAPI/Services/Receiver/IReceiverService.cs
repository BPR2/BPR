using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Services.Receiver;

public interface IReceiverService
{
    Task<WebResponse> AssignReceiverAsync(string serialNumber, string userName, int maxTransmission, int leftTransmission);
    Task<List<BPR_RazorLibrary.Models.Receiver>> GetAllReceiversAsync();
	Task<WebContent> GetReceiversByUserID(int userID);
	Task<WebResponse> AssignFieldToReceiver(int receiverID, int fieldID);
    Task<WebContent> GetAllReceiversList();
    Task<WebResponse> UpdateReceiverTimeInterval(int timeInterval, string serialNumber);
    Task<WebContent> GetReceiverBySerialNumber(string serialNumber);

    Task<WebResponse> UnassignReceiverFromUser(string serialNumber);
    Task<WebResponse> UpdateReceiverToUser(string serialNumber, string userName);
}
