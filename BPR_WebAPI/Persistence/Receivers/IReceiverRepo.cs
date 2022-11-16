using BPR_RazorLibrary.Models;

namespace BPR_WebAPI.Persistence.Receivers;

public interface IReceiverRepo
{
    Task<WebResponse> AssignReceiverAsync(string serialNumber, string userName);
    Task<List<Receiver>> GetAllReceivers();
    Task<WebContent> GetReceiversByUserID(int userID);
    Task<WebContent> GetAllReceiversList();
    Task<WebResponse> AssignFieldToReceiver(int receiverID, int fieldID);
    Task<WebResponse> UpdateReceiverTimeInterval(int timeInterval, string serialNumber);
    Task<WebContent> GetReceiverBySerialNumber(string serialNumber);
}
